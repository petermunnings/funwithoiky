using System.Collections.Generic;

namespace oikonomos.common.Models
{
    public class AdminReportsViewModel
    {
        public int RoleId { get; set; }
        public int MonthId { get; set; }
        public IList<RoleViewModel> SecurityRoles { get; set; }
        public IList<MonthViewModel> Months { get; set; }
        public IList<int> SelectedRoles { get; set; }
    }
}
