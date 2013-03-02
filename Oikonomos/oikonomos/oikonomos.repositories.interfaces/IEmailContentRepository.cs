namespace oikonomos.repositories.interfaces
{
    public interface IEmailContentRepository
    {
        string GetVisitorWelcomeLetter(int churchId);
        string GetMemberWelcomeLetter(int churchId);
    }
}