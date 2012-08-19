using System.Collections.Generic;
using oikonomos.common;

namespace oikonomos.data
{
    public partial class Person
    {
        public bool HasPermission(Permissions permission)
        {
            return Permissions.Contains((int)permission) || Permissions.Contains((int)common.Permissions.SystemAdministrator);
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
