using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oikonomos.common;
using System.Text.RegularExpressions;

namespace oikonomos.data
{
    public partial class Person
    {
        public bool HasPermission(Permissions permission)
        {
            return Permissions.Contains((int)permission);
        }

        public bool HasValidEmail()
        {
            return !string.IsNullOrEmpty(Email) && Utils.ValidEmailAddress(Email);
        }

        public string Fullname
        {
            get { return Firstname + " " + Family.FamilyName; }
        }

        public int ChurchId { get; set; }

        public Church Church { get; set; }

        public List<Church> Churches { get; set; } 

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public List<int> Permissions { get; set; }

        public bool IsSystemAdministrator { get; set; }

    }
}
