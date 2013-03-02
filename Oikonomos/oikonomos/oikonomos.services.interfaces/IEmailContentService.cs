namespace oikonomos.services.interfaces
{
    public interface IEmailContentService
    {
        string GetWelcomeLetterBodyFromDataBase(
            int churchId,
            string firstname,
            string surname,
            string systemName,
            string churchName,
            string churchOfficeNo,
            string churchOfficeEmail,
            string churchWebsite,
            string email,
            string password,
            string guid,
            bool isVisitor,
            bool includeUsernamePassword); 
    }
}