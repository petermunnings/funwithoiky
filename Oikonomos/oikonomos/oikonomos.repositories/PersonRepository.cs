using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PersonRepository : RepositoryBase, IPersonRepository
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IChurchRepository _churchRepository;

        public PersonRepository(IPermissionRepository permissionRepository, IChurchRepository churchRepository)
        {
            _permissionRepository = permissionRepository;
            _churchRepository = churchRepository;
        }

        public Person UpdatePerson(PersonViewModel person, Person currentPerson, out bool sendWelcomeEmail, out Church church)
        {
            sendWelcomeEmail = false;

            //We need some settings from the Church table
            church = _churchRepository.GetChurch(currentPerson.ChurchId);

            var personToSave = new Person();
            if (person.PersonId != 0)
            {
                personToSave = FetchPerson(person.PersonId, currentPerson);
            }
            else
            {
                Context.AddToPeople(personToSave);
                personToSave.ChurchId = church.ChurchId;
                personToSave.Church = church;

                personToSave.Created = DateTime.Now;
                if (church.SendWelcome)
                {
                    sendWelcomeEmail = true;
                }

                if (person.GroupId > 0)
                {
                    var pg = new PersonGroup
                    {
                        GroupId = person.GroupId,
                        Person = personToSave,
                        Joined = DateTime.Now,
                        Created = DateTime.Now,
                        Changed = DateTime.Now
                    };
                    personToSave.PersonGroups.Add(pg);
                }
            }

            if (person.FamilyId == 0)
            {
                if (person.FindFamily)
                {
                    var family = (from f in Context.Families
                                  join p in Context.People
                                      on f.FamilyId equals p.FamilyId
                                  join g in Context.PersonGroups
                                      on p.PersonId equals g.PersonId
                                  where f.FamilyName == person.Surname
                                      && g.GroupId == person.GroupId
                                  select f).FirstOrDefault();
                    personToSave.Family = family ?? new Family {Created = DateTime.Now};
                }
                else
                {
                    personToSave.Family = new Family {Created = DateTime.Now};
                }
            }
            else
            {
                personToSave.Family = (from f in Context.Families
                                       where f.FamilyId == person.FamilyId
                                       select f).FirstOrDefault() ?? new Family {Created = DateTime.Now};
            }

            personToSave.Family.Changed = DateTime.Now;
            personToSave.Changed = DateTime.Now;

            personToSave.Firstname = person.Firstname;
            personToSave.Family.FamilyName = person.Surname;
            personToSave.Email = person.Email;
            personToSave.DateOfBirth = person.DateOfBirth_Value;
            Context.SaveChanges();
            return personToSave;
        }

        public bool SavePersonalDetails(PersonViewModel person, Person currentPerson, Person personToSave)
        {
            var anniversaryHasChanged = personToSave.Anniversary != person.Anniversary_Value;
            personToSave.Anniversary = person.Anniversary_Value;

            personToSave.Occupation = person.Occupation;
            var selectedSite = (from s in Context.Sites where s.ChurchId == currentPerson.ChurchId && s.Name == person.Site select s).FirstOrDefault();
            if (selectedSite == null)
            {
                personToSave.SiteId = null;
            }
            else
            {
                personToSave.SiteId = selectedSite.SiteId;
                if (person.FamilyId > 0)
                {
                    var familyMembers = Context.People.Where(p => p.FamilyId == person.FamilyId && p.SiteId == null);
                    foreach (var p in familyMembers)
                    {
                        p.SiteId = selectedSite.SiteId;
                    }
                }
            }

            personToSave.Family.Changed = DateTime.Now;
            personToSave.Changed = DateTime.Now;
            return anniversaryHasChanged;
        }

        public Person FetchPerson(int personId, Person currentPerson)
        {
            var sysAdmin = currentPerson != null && currentPerson.Permissions != null && currentPerson.HasPermission(Permissions.SystemAdministrator);
            return FetchPerson(personId, sysAdmin);
        }

        private Person FetchPerson(int personId, bool sysAdmin)
        {
            var person = Context.People.Include("PersonChurches.Role.PermissionRoles").First(p => p.PersonId == personId);
            _permissionRepository.SetupPermissions(person, sysAdmin);

            return person;
        }

        public Person FetchPerson(int personId)
        {
            return FetchPerson(personId, false);
        }

        public IEnumerable<Person> GetPeopleWithEmailAddress(string emailAddress)
        {
            return (from p in Context.People
             where p.Email == emailAddress
             select p).ToList();
        }

        public Guid UpdatePublicId(Person person)
        {
            var publicId = Guid.NewGuid();
            person.PublicId = publicId.ToString();
            Context.SaveChanges();
            return publicId;
        }

        public void SaveWindowsLiveId(PersonViewModel person, Person personToSave)
        {
            if (string.IsNullOrEmpty(person.WindowsLiveId)) return;
            var windowsLiveId = Context.PersonOptionalFields.FirstOrDefault(p => p.OptionalFieldId == (int)OptionalFields.WindowsLive && p.PersonId == personToSave.PersonId);
            if (windowsLiveId == null)
            {
                windowsLiveId = new PersonOptionalField
                {
                    Changed = DateTime.Now,
                    Created = DateTime.Now,
                    OptionalFieldId = (int)OptionalFields.WindowsLive,
                    PersonId = personToSave.PersonId,
                    Value = person.WindowsLiveId
                };
                Context.AddToPersonOptionalFields(windowsLiveId);
                Context.SaveChanges();
            }
            else
            {
                if (windowsLiveId.Value != person.WindowsLiveId)
                {
                    windowsLiveId.Value = person.WindowsLiveId;
                    Context.SaveChanges();
                }
            }
        }

        public Person FetchPersonFromPublicId(string publicId)
        {
            var person = (from p in Context.People.Include("PersonChurches")
                          where p.PublicId == publicId
                          select p).FirstOrDefault();

            if (person == null)
                return null;

            _permissionRepository.SetupPermissions(person, false);
            return person;
        }

        public Person FetchPersonFromWindowsLiveId(string liveId)
        {
            var person = (from p in Context.People
                          join po in Context.PersonOptionalFields
                            on p.PersonId equals po.PersonId
                          where po.OptionalFieldId == (int)OptionalFields.WindowsLive
                            && po.Value == liveId
                          select p).FirstOrDefault();

            if (person == null)
                return null;

            _permissionRepository.SetupPermissions(person, false);
            return person;
        }

        public Person FetchPersonFromFacebookId(long facebookId)
        {
            var facebookValue = facebookId.ToString();
            var person = (from p in Context.People
                          join po in Context.PersonOptionalFields
                            on p.PersonId equals po.PersonId
                          where po.OptionalFieldId == (int)OptionalFields.Facebook
                            && po.Value == facebookValue
                          select p).FirstOrDefault();

            if (person == null)
                return null;

            _permissionRepository.SetupPermissions(person, false);
            return person;
        }

        public IEnumerable<Person> FetchPersonFromName(string fullname, string firstname, string surname, string email)
        {
            var persons = (from p in Context.People.Include("Family")
                           join f in Context.Families
                             on p.FamilyId equals f.FamilyId
                           where p.Firstname == firstname
                             && f.FamilyName == surname
                             && p.Email == email
                           select p).ToList();

            if (persons.Count == 0)
            {
                var fullnames = fullname.Split(' ');
                if (fullnames.Length > 2)
                {
                    //Try and search for the person using the first two names in the full name
                    //Takes out the maiden surname
                    firstname = fullnames[0];
                    surname = fullnames[1];
                    persons = (from p in Context.People.Include("Family")
                               join f in Context.Families
                                 on p.FamilyId equals f.FamilyId
                               where p.Firstname == firstname
                                 && f.FamilyName == surname
                                 && p.Email == email
                               select p).ToList();

                    if (persons.Count == 0)
                    {
                        firstname = fullnames[0];
                        surname = fullnames[fullnames.Length - 1];
                        persons = (from p in Context.People.Include("Family")
                                   join f in Context.Families
                                     on p.FamilyId equals f.FamilyId
                                   where p.Firstname == firstname
                                     && f.FamilyName == surname
                                     && p.Email == email
                                   select p).ToList();
                    }
                }
            }

            foreach (var person in persons)
            {
                _permissionRepository.SetupPermissions(person, false);
            }

            return persons;
        }

        public IEnumerable<int> FetchPersonIdsFromEmailAddress(string emailAddress, int churchId)
        {
            var peopleInChurch = Context.PersonChurches.Where(p => p.ChurchId == churchId);
            return peopleInChurch.Where(p => p.Person.Email == emailAddress).Select(p => p.PersonId);
        }
    }
}