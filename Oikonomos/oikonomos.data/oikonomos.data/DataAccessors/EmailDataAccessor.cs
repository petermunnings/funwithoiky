using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using oikonomos.common;

namespace oikonomos.data.DataAccessors
{
    public class EmailDataAccessor
    {
        public static string GetVisitorWelcomeLetter(int churchId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return context.ChurchEmailTemplates.FirstOrDefault(x => x.ChurchId == churchId && x.EmailTemplateId == (int)EmailTemplates.WelcomeVisitors).Template;
            }
        }

        public static string GetMemberWelcomeLetter(int churchId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return context.ChurchEmailTemplates.FirstOrDefault(x => x.ChurchId == churchId && x.EmailTemplateId == (int)EmailTemplates.WelcomeMembers).Template;
            }
        }
    }
}
