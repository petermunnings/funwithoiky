using System.Collections.Generic;

namespace oikonomos.common.Models
{
    public class AdminReportsViewModel
    {
        public int RoleId { get; set; }
        public int BirthdayMonthId { get; set; }
        public int AnniversaryMonthId { get; set; }
        public IList<RoleViewModel> SecurityRoles { get; set; }
        public IList<RoleViewModel> BirthdaySecurityRoles { get; set; }
        public IList<RoleViewModel> AnniversarySecurityRoles { get; set; }
        public IList<MonthViewModel> Months { get; set; }
    }
}
