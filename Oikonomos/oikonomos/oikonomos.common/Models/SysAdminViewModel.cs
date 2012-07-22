using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class SysAdminViewModel
    {
        public int ChurchId { get; set; }
        public List<ChurchViewModel> Churches { get; set; }
        public int EmailTemplateId { get; set; }
        public List<EmailTemplateViewModel> EmailTemplates { get; set; }
    }
}
