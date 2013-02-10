using System;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class RelationshipRepository : RepositoryBase, IRelationshipRepository
    {
        private readonly IPersonRepository _personRepository;

        public RelationshipRepository(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public void UpdateRelationships(PersonViewModel person, Person personToSave, bool anniversaryHasChanged)
        {
            if (person.FamilyMembers == null) return;
            foreach (var familyMember in person.FamilyMembers)
            {
                if (familyMember.Relationship == null) continue;
                var relationship = (Relationships)Enum.Parse(typeof(Relationships), familyMember.Relationship);
                if (anniversaryHasChanged && (relationship == Relationships.Husband || relationship == Relationships.Wife))
                {
                    var spouse = (from p in Context.People
                                  where p.PersonId == familyMember.PersonId
                                  select p).FirstOrDefault();

                    if (spouse != null)
                    {
                        spouse.Anniversary = personToSave.Anniversary;
                    }
                }

                AddPersonRelationship(personToSave.PersonId, familyMember.PersonId, (int)relationship, personToSave);

                //Check the opposite relationship
                UpdateOtherRelationships(familyMember, person);
            }
        }

        private void UpdateOtherRelationships(FamilyMemberViewModel familyMember, PersonViewModel person)
        {
            try
            {
                var relationship = (Relationships)Enum.Parse(typeof(Relationships), familyMember.Relationship);
                var oppositeRelationship = Relationships.Unknown;
                var familyMemberToUpdate = (from p in Context.People.Include("PersonRelationships")
                                               where p.PersonId == familyMember.PersonId
                                               select p).FirstOrDefault();

                var isMale = (from pr in Context.PersonRelationships
                               where pr.PersonRelatedToId == person.PersonId
                                     && (pr.RelationshipId == (int)Relationships.Husband ||
                                         pr.RelationshipId == (int)Relationships.Brother ||
                                         pr.RelationshipId == (int)Relationships.Father ||
                                         pr.RelationshipId == (int)Relationships.Grandfather ||
                                         pr.RelationshipId == (int)Relationships.Grandson ||
                                         pr.RelationshipId == (int)Relationships.Son)
                               select pr).Any();

                var isFemale = (from pr in Context.PersonRelationships
                                 where pr.PersonRelatedToId == person.PersonId
                                       && (pr.RelationshipId == (int)Relationships.Wife ||
                                           pr.RelationshipId == (int)Relationships.Sister ||
                                           pr.RelationshipId == (int)Relationships.Mother ||
                                           pr.RelationshipId == (int)Relationships.Grandmother ||
                                           pr.RelationshipId == (int)Relationships.Granddaughter ||
                                           pr.RelationshipId == (int)Relationships.Daughter)
                                 select pr).Any();

                switch (relationship)
                {
                    case Relationships.Husband:
                        {
                            oppositeRelationship = Relationships.Wife;
                            break;
                        }
                    case Relationships.Wife:
                        {
                            oppositeRelationship = Relationships.Husband;
                            break;
                        }
                    case Relationships.Son:
                    case Relationships.Daughter:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Father;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Mother;
                            }
                            break;
                        }
                    case Relationships.Brother:
                    case Relationships.Sister:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Brother;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Sister;
                            }
                            break;
                        }
                    case Relationships.Father:
                    case Relationships.Mother:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Son;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Daughter;
                            }
                            break;
                        }
                    case Relationships.Grandfather:
                    case Relationships.Grandmother:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Grandson;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Granddaughter;
                            }
                            break;
                        }
                    case Relationships.Grandson:
                    case Relationships.Granddaughter:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Grandfather;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Grandmother;
                            }
                            break;
                        }
                }

                if (oppositeRelationship == Relationships.Unknown) return;
                AddPersonRelationship(familyMember.PersonId, person.PersonId, (int)oppositeRelationship, familyMemberToUpdate);

                var personToUpdate = _personRepository.FetchPerson(person.PersonId, null);

                //What about the rest of the family
                if (relationship == Relationships.Husband || relationship == Relationships.Wife)
                {
                    var spouseRelationships = (from pr in Context.PersonRelationships
                                               where pr.PersonId == familyMember.PersonId
                                                     && (pr.RelationshipId == (int)Relationships.Son ||
                                                         pr.RelationshipId == (int)Relationships.Daughter ||
                                                         pr.RelationshipId == (int)Relationships.Grandson ||
                                                         pr.RelationshipId == (int)Relationships.Granddaughter)
                                               select pr);

                    foreach (var pr in spouseRelationships.Where(pr => person.PersonId != pr.PersonRelatedToId))
                    {
                        AddPersonRelationship(person.PersonId, pr.PersonRelatedToId, pr.RelationshipId, personToUpdate);
                    }
                }

                if (relationship != Relationships.Brother && relationship != Relationships.Sister) return;
                //He has a brother - does the brother have a father, grandfather, mother etc
                var siblingRelationships = (from pr in Context.PersonRelationships
                                            where pr.PersonId == familyMember.PersonId
                                                  && (pr.RelationshipId == (int)Relationships.Father ||
                                                      pr.RelationshipId == (int)Relationships.Mother ||
                                                      pr.RelationshipId == (int)Relationships.Grandfather ||
                                                      pr.RelationshipId == (int)Relationships.Grandmother ||
                                                      pr.RelationshipId == (int)Relationships.Sister ||
                                                      pr.RelationshipId == (int)Relationships.Brother)
                                            select pr);

                foreach (var pr in siblingRelationships.Where(pr => person.PersonId != pr.PersonRelatedToId))
                {
                    AddPersonRelationship(personToUpdate.PersonId, pr.PersonRelatedToId, pr.RelationshipId, personToUpdate);
                }
            }
            catch { }
        }


        private void AddPersonRelationship(int personId, int personRelatedToId, int relationshipId, Person familyMemberToUpdate)
        {
            var found = false;
            foreach (var rel in familyMemberToUpdate.PersonRelationships.Where(rel => rel.PersonRelatedToId == personRelatedToId))
            {
                if (rel.RelationshipId != relationshipId)
                {
                    rel.RelationshipId = relationshipId;
                    rel.Changed = DateTime.Now;
                }
                found = true;
                break;
            }

            if (found) return;
            //Create a new one
            var newRelationship = new PersonRelationship();
            newRelationship.PersonRelatedToId = personRelatedToId;
            newRelationship.RelationshipId = relationshipId;
            newRelationship.PersonId = personId;
            newRelationship.Created = DateTime.Now;
            newRelationship.Changed = DateTime.Now;
            Context.AddToPersonRelationships(newRelationship);
        }

    }
}