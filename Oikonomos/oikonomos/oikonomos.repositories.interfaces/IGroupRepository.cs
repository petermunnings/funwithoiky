using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IGroupRepository
    {
        void PopulateGroupId(int personId, Person currentPerson, PersonViewModel personViewModel);
        Group GetGroup(int groupId);
    }
}