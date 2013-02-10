using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IRelationshipRepository
    {
        void UpdateRelationships(PersonViewModel person, Person personToSave, bool anniversaryHasChanged); 
    }
}