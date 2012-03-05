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
            foreach (PersonRole personRole in this.PersonRoles)
            {
                foreach (PermissionRole permissionRole in personRole.Role.PermissionRoles)
                {
                    if (permissionRole.PermissionId == (int)permission || permissionRole.PermissionId == (int)Permissions.SystemAdministrator)
                    {
                        return true;
                    }
                }
            }
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
    }
}
