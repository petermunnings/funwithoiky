using System.Linq;
using oikonomos.common;
using oikonomos.data;
using oikonomos.data.Services;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IChurchRepository _churchRepository;
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;

        public PasswordService(IPersonRepository personRepository, IChurchRepository churchRepository, IUsernamePasswordRepository usernamePasswordRepository)
        {
            _personRepository = personRepository;
            _churchRepository = churchRepository;
            _usernamePasswordRepository = usernamePasswordRepository;
        }

        public string ResetPassword(string emailAddress)
        {
            var people = _personRepository.GetPeopleWithEmailAddress(emailAddress).ToList();

            if (people.Count == 1)
            {
                var newPassword = _usernamePasswordRepository.UpdatePassword(people[0]);
                return Email.SendResetPasswordEmail(people[0], newPassword);
            }
            return people.Count > 1
                       ? "There is more that one person using this email address in the oikonomos system"
                       : "Your email address could not be found in the database.  Please contact your church administrator for further assistance";
        }

        public bool SendEmailAndPassword(Person currentPerson, int personId, out string message)
        {
            if (personId == 0)
            {
                message = "You need to save the person before sending the email";
                return false;
            }

            if (!currentPerson.HasPermission(Permissions.SendEmailAndPassword))
            {
                message = "You don't have permission to perform this action";
                return false;
            }

            var church = _churchRepository.GetChurch(currentPerson.ChurchId);
            if (church == null)
            {
                message = "Error sending Email";
                return false;
            }

            var personToUpdate = _personRepository.FetchPerson(personId, currentPerson);

            if (personToUpdate == null)
            {
                message = "Error sending Email";
                return false;
            }

            if (personToUpdate.HasPermission(Permissions.Login))
            {
                if (personToUpdate.HasValidEmail())
                {
                    SendEmailAndPassword(personToUpdate.Firstname,
                                         personToUpdate.Family.FamilyName,
                                         church,
                                         personToUpdate.Email,
                                         personToUpdate);

                    message = "Email sent succesfully";
                    return true;
                }
                else
                {
                    message = "Invalid Email address";
                    return false;
                }
            }
            else
            {
                message = string.Format("You cannot send login details to a person with a role of {0}", personToUpdate.Role.Name);
                return false;
            }
        }

        public void SendEmailAndPassword(string firstname,
            string surname,
            Church church,
            string email,
            Person personToSave)
        {
            _usernamePasswordRepository.UpdateUsername(personToSave);
            var password = _usernamePasswordRepository.UpdatePassword(personToSave);
            var publicId = _personRepository.UpdatePublicId(personToSave);
            Email.SendWelcomeEmail(firstname,
                  surname,
                  church,
                  email,
                  password,
                  publicId,
                  personToSave.HasPermission(Permissions.SendVisitorWelcomeLetter),
                  false);
        }

    }
}