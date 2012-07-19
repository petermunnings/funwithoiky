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
            if (Permissions.Contains((int)permission))
                return true;
            return false;
        }

        public bool HasValidEmail()
        {
            if (Email!=null && Email != string.Empty && Utils.ValidEmailAddress(Email))
            {
                return true;
            }
            return false;
        }

        public string Fullname
        {
            get { return Firstname + " " + Family.FamilyName; }
        }

        public int ChurchId { get; set; }

        public Church Church { get; set; }

        public List<int> Permissions { get; set; }

        public bool IsSystemAdministrator { get; set; }

    }
}
