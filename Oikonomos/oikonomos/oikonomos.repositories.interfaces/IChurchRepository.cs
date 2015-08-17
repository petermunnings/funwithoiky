using System.Collections.Generic;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IChurchRepository
    {
        Church GetChurch(int churchId);
        IEnumerable<int> GetChurchMemberRoles(int churchId);
        IEnumerable<Person> GetChurchAdmins(int churchId);
    }
}