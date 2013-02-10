using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IChurchMatcherRepository
    {
        void CheckThatChurchIdsMatch(int personId, Person currentPerson);
    }
}