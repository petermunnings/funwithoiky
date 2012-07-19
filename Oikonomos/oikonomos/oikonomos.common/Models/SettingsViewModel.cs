using System.Collections.Generic;

namespace oikonomos.common.Models
{
    public class SettingsViewModel
    {
        public ChurchSettingsViewModel ChurchSettings { get; set; }        
        public GroupDto GroupSettings { get; set; }
        public List<SiteSettingsViewModel> Sites { get; set; }
        public List<OptionalFieldViewModel> OptionalFields { get; set; }
        public int RoleId { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public int ChurchEmailTemplateId { get; set; }
        public List<EmailTemplateViewModel> EmailTemplates { get; set; }
    }
}
