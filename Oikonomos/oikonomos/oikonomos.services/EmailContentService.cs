using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EmailContentService : IEmailContentService
    {
        private readonly IEmailContentRepository _emailContentRepository;

        public EmailContentService(IEmailContentRepository emailContentRepository)
        {
            _emailContentRepository = emailContentRepository;
        }

        public string GetWelcomeLetterBodyFromDataBase(int churchId, string firstname, string surname, string systemName,
                                                       string churchName, string churchOfficeNo, string churchOfficeEmail,
                                                       string churchWebsite, string email, string password, string guid, bool isVisitor,
                                                       bool includeUsernamePassword)
        {
            string emailBody = isVisitor ? _emailContentRepository.GetVisitorWelcomeLetter(churchId) : _emailContentRepository.GetMemberWelcomeLetter(churchId);

            emailBody = emailBody.Replace("##SystemName##", systemName);
            emailBody = emailBody.Replace("##Firstname##", firstname);
            emailBody = emailBody.Replace("##Surname##", surname);
            emailBody = emailBody.Replace("##PublicId##", guid);
            emailBody = emailBody.Replace("##Email##", email);
            emailBody = emailBody.Replace("##Password##", password);
            emailBody = emailBody.Replace("##ChurchName##", churchName);
            emailBody = emailBody.Replace("##ChurchOfficeNo##", churchOfficeNo);
            emailBody = emailBody.Replace("##ChurchOfficeEmail##", churchOfficeEmail);
            emailBody = emailBody.Replace("##ChurchWebsite##", churchWebsite);

            return emailBody;
        }
    }
}