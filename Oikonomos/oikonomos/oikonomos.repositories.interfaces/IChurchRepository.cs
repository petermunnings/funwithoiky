using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IChurchRepository
    {
        Church GetChurch(int churchId);
    }
}