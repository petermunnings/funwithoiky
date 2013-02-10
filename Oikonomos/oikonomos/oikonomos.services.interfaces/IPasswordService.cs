using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IPasswordService
    {
        string ResetPassword(string emailAddress);
        bool SendEmailAndPassword(Person currentPerson, int personId, out string message);
        void SendEmailAndPassword(string firstname, string surname, Church church, string email, Person personToSave);
    }
}