using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IPersonService
    {
        int Save(PersonViewModel person, Person currentPerson);
        void SavePersonToSampleChurch(string firstname, string surname, string liveId, string cellPhone, string email, int roleId);
        PersonViewModel FetchPersonViewModel(int personId, Person currentPerson);
    }
}