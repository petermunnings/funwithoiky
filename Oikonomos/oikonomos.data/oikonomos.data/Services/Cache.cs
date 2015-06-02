using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using oikonomos.common;
using oikonomos.common.Models;

namespace oikonomos.data.Services
{
    public static class Cache
    {
        private static ObjectCache cache;

        static Cache()
        {
            cache = MemoryCache.Default;
        }

        private static T FetchCacheValue<T>(string name)
        {
            var returnValue = cache[name];
            if (returnValue == null)
                return default(T);

            if (returnValue is T)
                return (T) returnValue;
            return default(T);
        }

        private static void SetCacheValue(string name, object value)
        {
            cache[name] = value;
        }
        
        public static List<RoleViewModel> SecurityRoles(oikonomosEntities context, Person currentPerson)
        {
            if (currentPerson.HasPermission(common.Permissions.SystemAdministrator))
                return (from r in context.Roles
                         where r.ChurchId == currentPerson.ChurchId
                         select new RoleViewModel {RoleId = r.RoleId, Name = r.Name, IsSelected = r.DisplayName == "Member"}).ToList();
            
            return (from r in context.Roles
                from canSetRole in r.CanBeSetByRoles
                where r.ChurchId == currentPerson.ChurchId
                      && canSetRole.RoleId == currentPerson.RoleId
                select new RoleViewModel { RoleId = r.RoleId, Name = r.Name, IsSelected = r.DisplayName == "Member" }).ToList();
        }

        public static List<string> Permissions(oikonomosEntities context)
        {
            var permissions = Cache.FetchCacheValue<List<string>>(CacheNames.Permissions);
            if (permissions == null)
            {
                //Fetch Permissions from Database
                permissions = context.Permissions
                             .Where(p => p.IsVisible == true)
                             .Select(p => p.Name)
                             .ToList();

                Cache.SetCacheValue(CacheNames.Permissions, permissions);
            }

            return permissions;
        }
    }
}