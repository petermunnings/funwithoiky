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
    }
}