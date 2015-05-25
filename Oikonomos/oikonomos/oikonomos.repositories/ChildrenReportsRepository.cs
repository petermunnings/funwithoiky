using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ChildrenReportsRepository : RepositoryBase, IChildrenReportsRepository
    {
        public IEnumerable<ChildReportDto> GetListOfChildrenForAChurch(Person personRunningReport)
        {
            if (!personRunningReport.HasPermission(Permissions.ViewAdminReports))
                return new List<ChildReportDto>();

            var list = Context.PersonChurches
                .Where(pc => pc.ChurchId == personRunningReport.ChurchId)
                .Select(pc => new ChildReportDto
                {
                    PersonId = pc.PersonId,
                    Firstname = pc.Person.Firstname,
                    Surname = pc.Person.Family.FamilyName,
                    DateOfBirth = pc.Person.DateOfBirth,
                    CellNo = pc.Person.PersonOptionalFields.FirstOrDefault(o => o.OptionalFieldId == (int) OptionalFields.CellPhone).Value,
                    AddressLine1 = pc.Person.Family.Address.Line1,
                    AddressLine2 = pc.Person.Family.Address.Line2,
                    AddressLine3 = pc.Person.Family.Address.Line3,
                    AddressLine4 = pc.Person.Family.Address.Line4,
                    Father = pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int) Relationships.Father).Person1.Firstname,
                    FatherEmail = pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int)Relationships.Father).Person1.Email,
                    FatherCell = pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int)Relationships.Father).Person1==null ? null :
                                 pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int)Relationships.Father).Person1.PersonOptionalFields.FirstOrDefault(o => o.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                    Mother = pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int) Relationships.Mother).Person1.Firstname,
                    MotherEmail = pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int)Relationships.Mother).Person1.Email,
                    MotherCell = pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int)Relationships.Mother).Person1 == null ? null :
                                 pc.Person.PersonRelationships.FirstOrDefault(r => r.RelationshipId == (int)Relationships.Mother).Person1.PersonOptionalFields.FirstOrDefault(o => o.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                    GroupName = pc.Person.PersonGroups.FirstOrDefault(g=>g.PrimaryGroup).Group.Name

                })
                .Where(c=>c.Father != null || c.Mother !=null);

            return list.ToList().Where(c=>c.Age <= 21);

        }
    }
}