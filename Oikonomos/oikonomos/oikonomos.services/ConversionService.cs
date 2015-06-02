using System.Collections.Generic;

namespace oikonomos.services
{
    public class ConversionService
    {
        public static IEnumerable<int> ConvertSelectedRolesToListOfInts(IEnumerable<string> selectedRoles)
        {
            var roles = new List<int>();
            foreach (var r in selectedRoles)
            {
                int roleId;
                if (int.TryParse(r, out roleId))
                    roles.Add(roleId);
            }
            return roles;
        }
    }
}