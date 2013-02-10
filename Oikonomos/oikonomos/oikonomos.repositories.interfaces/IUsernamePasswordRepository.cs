using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IUsernamePasswordRepository
    {
        string UpdatePassword(Person person);
        void UpdateUsername(Person person);
        Person CheckEmailPassword(string email, string password);
        Person Login(string email, string password, out string message);
        string ChangePassword(int personId, string currentPassword, string newPassword);
    }
}