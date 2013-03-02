using System.Linq;
using oikonomos.common;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class EmailContentRepository : RepositoryBase, IEmailContentRepository
    {
        public string GetVisitorWelcomeLetter(int churchId)
        {
            var churchEmailTemplate = Context.ChurchEmailTemplates.FirstOrDefault(x => x.ChurchId == churchId && x.EmailTemplateId == (int) EmailTemplates.WelcomeVisitors);
            return churchEmailTemplate != null ? churchEmailTemplate.Template : string.Empty;
        }

        public string GetMemberWelcomeLetter(int churchId)
        {
            var churchEmailTemplate = Context.ChurchEmailTemplates.FirstOrDefault(x => x.ChurchId == churchId && x.EmailTemplateId == (int) EmailTemplates.WelcomeMembers);
            return churchEmailTemplate != null ? churchEmailTemplate.Template : string.Empty;
        }
    }
}