﻿using System;
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
        
        public static List<RoleViewModel> SecurityRoles(oikonomosEntities context, Person currentPerson)
        {
            //TODO - not a good idea to cache these - gives everyone the same security role.
            //       rethink this later
            var securityRoles = new List<RoleViewModel>();
            var currentPersonRole = GetCurrentPersonRole(context, currentPerson);
            if (currentPersonRole != null)
                securityRoles = (from r in context.Roles
                                 from canSetRole in r.CanSetRoles
                                 where r.ChurchId == currentPerson.ChurchId
                                 && canSetRole.RoleId == currentPersonRole.RoleId
                                 select new RoleViewModel()
                                 {
                                     RoleId = r.RoleId,
                                     Name = r.Name
                                 }).ToList();

            return securityRoles;
        }

        private static PersonRole GetCurrentPersonRole(oikonomosEntities context, Person currentPerson)
        {
            return (from pr in context.PersonRoles
                    join r in context.Roles
                      on pr.RoleId equals r.RoleId
                    where pr.PersonId == currentPerson.PersonId
                      && r.ChurchId == currentPerson.ChurchId
                    select pr).FirstOrDefault();
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