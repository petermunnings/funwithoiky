using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using oikonomos.common;

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
            object returnValue = cache[name];
            if (returnValue == null)
            {
                return default(T);
            }

            if (returnValue is T)
            {
                return (T)returnValue;
            }
            else
            {
                return default(T);
            }
        }

        private static void SetCacheValue(string name, object value)
        {
            cache[name] = value;
        }

        public static List<string> SecurityRoles(oikonomosEntities context)
        {
            List<string> securityRoles = Cache.FetchCacheValue<List<string>>(CacheNames.SecurityRoles);
            if (securityRoles == null)
            {
                //Fetch Security Roles from Database
                securityRoles = (from r in context.Roles
                                 join pr in context.PermissionRoles
                                 on r.RoleId equals pr.RoleId
                                 where pr.PermissionId != (int)common.Permissions.SystemAdministrator
                                 select r.Name).ToList();

                Cache.SetCacheValue(CacheNames.SecurityRoles, securityRoles);
            }

            return securityRoles;
        }

        public static List<string> Permissions(oikonomosEntities context)
        {
            List<string> permissions = Cache.FetchCacheValue<List<string>>(CacheNames.Permissions);
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