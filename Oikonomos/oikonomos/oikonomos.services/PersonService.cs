using System;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.data.Services;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonGroupRepository _personGroupRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPersonRoleRepository _personRoleRepository;
        private readonly IPersonOptionalFieldRepository _personOptionalFieldRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IChurchMatcherRepository _churchMatcherRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IEmailService _emailService;
        private readonly IAddressRepository _addressRepository;

        public PersonService(
            IPersonRepository personRepository, 
            IPersonGroupRepository personGroupRepository, 
            IPermissionRepository permissionRepository, 
            IPersonRoleRepository personRoleRepository, 
            IPersonOptionalFieldRepository personOptionalFieldRepository, 
            IRelationshipRepository relationshipRepository,
            IChurchMatcherRepository churchMatcherRepository,
            IGroupRepository groupRepository,
            IFamilyRepository familyRepository,
            IEmailService emailService,
            IAddressRepository addressRepository)
        {
            _personRepository = personRepository;
            _personGroupRepository = personGroupRepository;
            _permissionRepository = permissionRepository;
            _personRoleRepository = personRoleRepository;
            _personOptionalFieldRepository = personOptionalFieldRepository;
            _relationshipRepository = relationshipRepository;
            _churchMatcherRepository = churchMatcherRepository;
            _groupRepository = groupRepository;
            _familyRepository = familyRepository;
            _emailService = emailService;
            _addressRepository = addressRepository;
        }

        public void SavePersonToSampleChurch(string firstname, string surname, string liveId, string cellPhone, string email, int roleId)
        {
            var newPerson = new PersonViewModel
            {
                Firstname = firstname,
                Surname = surname,
                WindowsLiveId = liveId,
                CellPhone = cellPhone,
                Email = email,
                RoleId = roleId
            };

            var currentPerson = _personRepository.FetchPerson(1);
            currentPerson.ChurchId = 6;
            Save(newPerson, currentPerson);
        }

        public PersonViewModel FetchPersonViewModel(int personId, Person currentPerson)
        {
            if (currentPerson.HasPermission(Permissions.EditChurchPersonalDetails) ||
                currentPerson.HasPermission(Permissions.EditGroupPersonalDetails) ||
                currentPerson.HasPermission(Permissions.EditOwnDetails))
            {
                try
                {
                    _churchMatcherRepository.CheckThatChurchIdsMatch(personId, currentPerson);

                    var person = _personRepository.FetchPerson(personId, currentPerson);

                    if (person == null)
                        throw new ApplicationException("Invalid PersonId");

                    //SetupPermissions(context, person, currentPerson.Church);

                    var imageLink = string.Empty;
                    var faceBookId = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int) OptionalFields.Facebook) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int) OptionalFields.Facebook).Value;
                    var img = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int) OptionalFields.ImageLink) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int) OptionalFields.ImageLink).Value;
                    if (img != string.Empty)
                    {
                        imageLink = string.Format("https://www.oikonomos.co.za/Images/{0}", img);
                    }
                    else
                    {
                        if (faceBookId != string.Empty)
                        {
                            imageLink = string.Format("https://graph.facebook.com/{0}/picture", faceBookId);
                        }
                    }

                    var personViewModel = new PersonViewModel
                        {
                            PersonId = person.PersonId,
                            FamilyId = person.FamilyId,
                            Firstname = person.Firstname,
                            Surname = person.Family.FamilyName,
                            Email = person.Email,
                            DateOfBirth_Value = person.DateOfBirth,
                            Anniversary_Value = person.Anniversary,
                            HomePhone = person.Family.HomePhone,
                            CellPhone = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int) OptionalFields.CellPhone) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int) OptionalFields.CellPhone).Value,
                            WorkPhone = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int) OptionalFields.WorkPhone) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int) OptionalFields.WorkPhone).Value,
                            Skype = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int) OptionalFields.Skype) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int) OptionalFields.Skype).Value,
                            Twitter = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int) OptionalFields.Twitter) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int) OptionalFields.Twitter).Value,
                            ImageLink = imageLink,
                            FacebookId = faceBookId,
                            Occupation = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Occupation) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int)OptionalFields.Occupation).Value,
                            MaritalStatus = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.MaritalStatus) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int)OptionalFields.MaritalStatus).Value,
                            Gender = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Gender) == null ? string.Empty : person.PersonOptionalFields.First(c => c.OptionalFieldId == (int)OptionalFields.Gender).Value,
                            Address1 = person.Family.Address.Line1,
                            Address2 = person.Family.Address.Line2,
                            Address3 = person.Family.Address.Line3,
                            Address4 = person.Family.Address.Line4,
                            Lat = person.Family.Address.Lat,
                            Lng = person.Family.Address.Long,
                            HasUsername = person.Username != null,
                            FindFamily = false,
                            GroupId = 0,
                            Site = person.SiteId.HasValue ? person.Site.Name : "Select site...",
                            HeardAbout =
                                person.PersonOptionalFields.FirstOrDefault(
                                    c => c.OptionalFieldId == (int) OptionalFields.HeardAbout) == null
                                    ? string.Empty
                                    : person.PersonOptionalFields.First(
                                        c => c.OptionalFieldId == (int) OptionalFields.HeardAbout).Value,
                            RoleId = person.RoleId,
                            RoleName = person.Role.Name
                        };

                    _groupRepository.PopulateGroupId(personId, currentPerson, personViewModel);

                    personViewModel.FamilyMembers = _familyRepository.FetchFamilyMembers(personId, person.FamilyId);
                    personViewModel.SecurityRoles = _personRoleRepository.FetchSecurityRoles(currentPerson);

                    return personViewModel;
                }
                catch (Exception ex)
                {
                    _emailService.SendExceptionEmail(ex);
                    return null;
                }
            }
            throw new Exception(ExceptionMessage.InvalidCredentials);
        }

        public void LinkPersonToFamily(int personId, int familyId)
        {
            _personRepository.UpdateFamilyId(personId, familyId);
        }

        public int Save(PersonViewModel person, Person currentPerson)
        {
            if (!currentPerson.HasPermission(Permissions.EditChurchPersonalDetails))
            {
                if (currentPerson.HasPermission(Permissions.EditGroupPersonalDetails))
                {
                    if (!_permissionRepository.CheckSavePermissionGroup(person, currentPerson)) { return person.PersonId; }
                }
                else if (currentPerson.HasPermission(Permissions.EditOwnDetails))
                {
                    if (!_permissionRepository.CheckSavePermissionPersonal(person, currentPerson)) { return person.PersonId; }
                }
                else
                {
                    return person.PersonId;
                }
            }

            bool sendWelcomeEmail;
            Church church;
            var personToSave = _personRepository.UpdatePerson(person, currentPerson, out sendWelcomeEmail, out church);
            var anniversaryHasChanged = _personRepository.SavePersonalDetails(person, currentPerson, personToSave);
            _personRoleRepository.SavePersonChurchRole(person, currentPerson, personToSave);
            var addedToNewGroup = _personGroupRepository.AddPersonToGroup(person, currentPerson, personToSave);
            _personOptionalFieldRepository.SaveOptionalFields(person, personToSave);
            _addressRepository.SaveAddressInformation(person, personToSave.Family.Address, personToSave.Family);
            _relationshipRepository.UpdateRelationships(person, personToSave, anniversaryHasChanged);
            personToSave = _personRepository.FetchPerson(personToSave.PersonId, currentPerson);
            _personRepository.SaveWindowsLiveId(person, personToSave);
            _emailService.SendEmails(person, sendWelcomeEmail, church, personToSave, currentPerson);
            _emailService.EmailGroupLeader(person, currentPerson, church, personToSave, addedToNewGroup);

            return personToSave.PersonId;
        }
    }


}