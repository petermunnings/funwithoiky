using System.Collections.Generic;
using System.Linq;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ChurchRepository : RepositoryBase, IChurchRepository
    {
        public Church GetChurch(int churchId)
        {
            return Context.Churches.FirstOrDefault(c => c.ChurchId == churchId);
        }

        public IEnumerable<int> GetChurchMemberRoles(int churchId)
        {
            return (from r in Context.Roles
                    where r.ChurchId == churchId
                    && r.DisplayName == "Member"
                    select r.RoleId).ToList();
        }

        public IEnumerable<Person> GetChurchAdmins(int churchId)
        {
            return Context.PersonChurches
                .Where(p =>
                    p.ChurchId == churchId &&
                    p.Role.Name == "Church Administrator")
                .Select(p => p.Person);
        }
    }
}