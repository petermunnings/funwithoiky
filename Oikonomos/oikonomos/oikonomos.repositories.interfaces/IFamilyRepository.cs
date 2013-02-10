using System.Collections.Generic;
using oikonomos.common.Models;

namespace oikonomos.repositories.interfaces
{
    public interface IFamilyRepository
    {
        IEnumerable<FamilyMemberViewModel> FetchFamilyMembers(int personId, int familyId);
        IEnumerable<FamilyMemberViewModel> AddPersonToFamily(int familyId, int personId);
    }
}