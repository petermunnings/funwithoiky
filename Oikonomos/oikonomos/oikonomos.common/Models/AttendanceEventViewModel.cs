using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class AttendanceEventViewModel
    {
        public int PersonId { get; set; }
        public int FamilyId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public bool Attended { get; set; }
        public DateTime Date { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}
