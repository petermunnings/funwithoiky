using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IEmailService
    {
        void SendEmails(PersonViewModel person, bool sendWelcomeEmail, Church church, Person personToSave);
        void EmailGroupLeader(PersonViewModel person, Person currentPerson, Church church, Person personToSave, bool addedToNewGroup);
    }
}