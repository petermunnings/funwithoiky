using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class AdminReportsViewModel
    {
        public int RoleId { get; set; }

        public List<RoleViewModel> SecurityRoles { get; set; }
    }
}
