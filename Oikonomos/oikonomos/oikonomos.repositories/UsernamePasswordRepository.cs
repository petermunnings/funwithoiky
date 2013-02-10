using System;
using System.Linq;
using System.Web.Security;
using oikonomos.common;
using oikonomos.data;
using oikonomos.data.Services;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class UsernamePasswordRepository : RepositoryBase, IUsernamePasswordRepository
    {
        private readonly IPermissionRepository _permissionRepository;

        public UsernamePasswordRepository(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public string UpdatePassword(Person person)
        {
            var password = RandomPasswordGenerator.Generate(RandomPasswordOptions.AlphaNumeric);
            person.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            Context.SaveChanges();
            return password;
        }

        public void UpdateUsername(Person person)
        {
            person.Username = (person.Firstname + person.Family.FamilyName).Replace(" ", string.Empty);
            Context.SaveChanges();
        }

        public Person CheckEmailPassword(string email, string password)
        {
            var passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            var person = (from p in Context.People
                          join pc in Context.PersonChurches
                              on p.PersonId equals pc.PersonId
                          join permissions in Context.PermissionRoles
                              on pc.RoleId equals permissions.RoleId
                          where p.Email == email
                              && p.PasswordHash == passwordHash
                              && permissions.PermissionId == (int)Permissions.Login
                          select p).FirstOrDefault();
            if (person == null)
                return null;

            _permissionRepository.SetupPermissions(person, false);
            return person;
        }

        public Person Login(string email, string password, out string message)
        {
            var loggedOnPerson = CheckEmailPassword(email, password);
            if (loggedOnPerson == null)
            {
                message = "Invalid Email or Password";
                return null;
            }
            var fullName = loggedOnPerson.Firstname + " " + loggedOnPerson.Family.FamilyName;
            message = "Welcome " + fullName + " from " + loggedOnPerson.Church.Name;
            return loggedOnPerson;
        }

        public string ChangePassword(int personId, string currentPassword, string newPassword)
        {
            var loggedOnPerson = CheckEmailPassword(personId, currentPassword);
            if (loggedOnPerson == null)
            {
                return "Invalid Password";
            }
            //Change password
            var passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");
            loggedOnPerson.PasswordHash = passwordHash;
            loggedOnPerson.Changed = DateTime.Now;
            Context.SaveChanges();
            return "Password succesfully changed";
        }


        private Person CheckEmailPassword(int personId, string password)
        {
            var passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            return (from p in Context.People
                    where p.PersonId == personId
                    && p.PasswordHash == passwordHash
                    select p).FirstOrDefault();
        }
    }
}