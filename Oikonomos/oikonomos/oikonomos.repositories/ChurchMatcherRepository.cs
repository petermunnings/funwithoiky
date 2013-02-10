using System;
using System.Linq;
using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ChurchMatcherRepository : RepositoryBase, IChurchMatcherRepository
    {
        public void CheckThatChurchIdsMatch(int personId, Person currentPerson)
        {
            if (currentPerson.HasPermission(Permissions.SystemAdministrator))
                return;
            if (!Context.People.First(p => p.PersonId == personId).PersonChurches.Select(c => c.ChurchId).ToList().Contains(currentPerson.ChurchId))
                throw new ApplicationException("ChurchId does not match currentPerson ChurchId");
        }
    }
}