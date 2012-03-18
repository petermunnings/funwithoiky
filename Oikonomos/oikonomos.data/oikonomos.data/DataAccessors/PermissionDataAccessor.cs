using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.Web.Mvc.JQuery.JqGrid;
using System.Configuration;

namespace oikonomos.data.DataAccessors
{
    public class PermissionDataAccessor
    {
        public static void AddPermissionsToRole(Person currentPerson, int roleId, List<int> permissionIds)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                foreach (var permissionId in permissionIds)
                {
                    PermissionRole pr = new PermissionRole()
                    {
                        RoleId = roleId,
                        Changed = DateTime.Now,
                        Created = DateTime.Now,
                        PermissionId = permissionId
                    };

                    context.PermissionRoles.AddObject(pr);
                }

                context.SaveChanges();

            }
        }

        public static void RemovePermissionsFromRole(Person currentPerson, int roleId, List<int> permissionIds)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var permissionRoles = (from p in context.PermissionRoles
                                       where p.RoleId == roleId
                                       && permissionIds.Contains(p.PermissionId)
                                       select p).ToList();
                if (permissionRoles != null)
                {
                    foreach (var permissionRole in permissionRoles)
                    {
                        context.DeleteObject(permissionRole);
                    }
                }

                context.SaveChanges();

            }
        }

        public static JqGridData FetchPermissionsForRoleJQGrid(Person currentPerson, JqGridRequest request, int roleId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var permissions = (from p in context.Permissions
                              join pr in context.PermissionRoles
                              on p.PermissionId equals pr.PermissionId
                              join r in context.Roles
                              on pr.RoleId equals r.RoleId
                              where r.ChurchId == currentPerson.ChurchId
                                && (pr.RoleId == roleId)
                              select p);

                int totalRecords = permissions.Count();

                switch (request.sidx)
                {
                    case "Permission":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                permissions = permissions.OrderBy(p => p.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                permissions = permissions.OrderByDescending(p => p.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData peopleGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from p in permissions.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = p.PermissionId.ToString(),
                                cell = new string[] 
                                {
                                    p.PermissionId.ToString(),
                                    p.Name
                                }
                            }).ToArray()
                };

                return peopleGridData;
            }
        }

        public static JqGridData FetchPermissionsNotInRoleJQGrid(Person currentPerson, JqGridRequest request, int roleId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var permissionsInRole = (from p in context.Permissions
                                         join pr in context.PermissionRoles
                                         on p.PermissionId equals pr.PermissionId
                                         join r in context.Roles
                                         on pr.RoleId equals r.RoleId
                                         where r.ChurchId == currentPerson.ChurchId
                                           && (pr.RoleId == roleId)
                                         select p);

                var permissionsNotInRole = context.Permissions.Except(permissionsInRole);

                int totalRecords = permissionsNotInRole.Count();

                switch (request.sidx)
                {
                    case "Permission":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                permissionsNotInRole = permissionsNotInRole.OrderBy(p => p.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            
                            }
                            else
                            {
                                permissionsNotInRole = permissionsNotInRole.OrderByDescending(p => p.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData permissionGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from p in permissionsNotInRole.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = p.PermissionId.ToString(),
                                cell = new string[] 
                                {
                                    p.PermissionId.ToString(),
                                    p.Name
                                }
                            }).ToArray()
                };

                return permissionGridData;
            }
        }

    }
}
