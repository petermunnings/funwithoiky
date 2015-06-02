using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PermissionRepository : RepositoryBase, IPermissionRepository
    {
        public bool CheckSavePermissionPersonal(PersonViewModel person, Person currentPerson)
        {
            var canSave = false;
            if (person.PersonId > 0)
            {
                var familyPerson = (from p in Context.People
                                    where p.PersonId == person.PersonId
                                    && p.FamilyId == currentPerson.FamilyId
                                    select p).FirstOrDefault();
                if (familyPerson != null)
                {
                    canSave = true;
                }
            }
            else
            {
                canSave = true;
            }
            return canSave;
        }

        public bool CheckSavePermissionGroup(PersonViewModel person, Person currentPerson)
        {
            var canSave = false;
            if (person.PersonId > 0)
            {
                var groupPerson = (from pg in Context.PersonGroups
                                   join g in Context.Groups
                                       on pg.GroupId equals g.GroupId
                                   where pg.PersonId == person.PersonId
                                         && g.ChurchId == currentPerson.ChurchId
                                         && (g.LeaderId == currentPerson.PersonId || g.AdministratorId == currentPerson.PersonId)
                                   select pg).FirstOrDefault();
                if (groupPerson != null)
                {
                    canSave = true;
                }
            }
            else
            {
                canSave = currentPerson.HasPermission(Permissions.AddNewPerson);
            }
            return canSave;
        }

        public void SetupPermissions(Person currentPerson, bool sysAdmin)
        {
            SetupPermissions(currentPerson, currentPerson.PersonChurches.First().Church, sysAdmin);
        }

        public void SetupPermissions(Person currentPerson, Church church, bool sysAdmin)
        {
            currentPerson.Permissions = (from pr in Context.PersonChurches
                                         join r in Context.Roles
                                             on pr.RoleId equals r.RoleId
                                         join permissions in Context.PermissionRoles
                                             on r.RoleId equals permissions.RoleId
                                         where pr.PersonId == currentPerson.PersonId
                                               && r.ChurchId == church.ChurchId
                                         select permissions.PermissionId)
                .ToList();

            if (sysAdmin) currentPerson.Permissions.Add((int)Permissions.SystemAdministrator);
            var surname = currentPerson.Family.FamilyName;
            currentPerson.Church = church;
            currentPerson.ChurchId = church.ChurchId;
            var personChurch = currentPerson.PersonChurches.FirstOrDefault(pc => pc.ChurchId == currentPerson.ChurchId);
            Role role = null;
            if (personChurch != null)
            {
                role = Context.Roles.First(r => r.RoleId == personChurch.RoleId);
            }
            else if (currentPerson.HasPermission(Permissions.SystemAdministrator))
            {
                role = Context.Roles.FirstOrDefault(r => r.ChurchId == church.ChurchId && r.Name.Contains("Administrator"));
                if (role == null)
                    throw new ApplicationException("Cannot set role for new church");
            }
            else
            {
                throw new ApplicationException("Cannot set role for new church");
            }
            currentPerson.RoleId = role.RoleId;
            currentPerson.Role = role;
            var churchIds = (from p in currentPerson.PersonChurches select p.ChurchId).ToList();
            currentPerson.Churches = Context.Churches.Where(c => churchIds.Contains(c.ChurchId)).ToList();
        }

        public IEnumerable<Person> FetchPeopleWithASpecificPermission(Permissions permission, int churchId)
        {
            var permissionInt = (int) permission;

            return from pc in Context.PersonChurches
                join pr in Context.PermissionRoles 
                    on pc.RoleId equals pr.RoleId
                where pr.PermissionId == permissionInt
                    && pc.ChurchId == churchId
                select pc.Person;

        }
    }
}