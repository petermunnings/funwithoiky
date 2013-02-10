using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class FamilyRepository : RepositoryBase, IFamilyRepository
    {
        public IEnumerable<FamilyMemberViewModel> FetchFamilyMembers(int personId, int familyId)
        {
            var personFirstname = (from p in Context.People
                                   where p.PersonId == personId
                                   select p.Firstname).FirstOrDefault();
            var familyMembers = (
                    from p in Context.People
                    join f in Context.Families
                    on p.FamilyId equals f.FamilyId
                    where f.FamilyId == familyId
                    && p.PersonId != personId
                    select new FamilyMemberViewModel
                    {
                        PersonId = p.PersonId,
                        FamilyMember = p.Firstname,
                        Person = personFirstname,
                        Relationship = (from pr in Context.PersonRelationships
                                        where pr.PersonId == personId
                                        && pr.PersonRelatedToId == p.PersonId
                                        select pr.Relationship.Name).FirstOrDefault(),
                        RelationshipId = (from pr in Context.PersonRelationships
                                          where pr.PersonId == personId
                                          && pr.PersonRelatedToId == p.PersonId
                                          select pr.RelationshipId).FirstOrDefault(),
                        FacebookId = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Facebook) == null ? "" : p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Facebook).Value
                    }).ToList();

            familyMembers.Sort((e1, e2) => e1.RelationshipId.CompareTo(e2.RelationshipId));

            return familyMembers;
        }

        public IEnumerable<FamilyMemberViewModel> AddPersonToFamily(int familyId, int personId)
        {
            var person = (from p in Context.People
                          where p.PersonId == personId
                          select p).First();

            person.FamilyId = familyId;
            person.Changed = DateTime.Now;
            Context.SaveChanges();

            return FetchFamilyMembers(personId, familyId);
        }
    }
}