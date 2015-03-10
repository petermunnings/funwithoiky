using System.Linq;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ChurchEmailTemplatesRepository : RepositoryBase, IChurchEmailTemplatesRepository
    {
        public string FetchChurchEmailSignature(int churchId)
        {
            var signature = Context.ChurchEmailTemplates.FirstOrDefault(t => t.ChurchId == churchId && t.EmailTemplateId == 4);
            return signature == null ? null : signature.Template;
        }
    }
}