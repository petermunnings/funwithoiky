namespace oikonomos.services.interfaces
{
    public interface IPasswordService
    {
        string ResetPassword(string emailAddress);
    }
}