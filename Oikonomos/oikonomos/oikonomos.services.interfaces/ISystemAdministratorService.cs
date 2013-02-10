using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface ISystemAdministratorService
    {
        Church SetNewChurch(Person currentPerson, int churchId);
    }
}