using System;
using System.Linq;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IChurchRepository _churchRepository;
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;
        private readonly IEmailService _emailService;

        public PasswordService(IPersonRepository personRepository, IChurchRepository churchRepository, IUsernamePasswordRepository usernamePasswordRepository, IEmailService emailService)
        {
            _personRepository = personRepository;
            _churchRepository = churchRepository;
            _usernamePasswordRepository = usernamePasswordRepository;
            _emailService = emailService;
        }

        public string ResetPassword(string emailAddress)
        {
            var people = _personRepository.GetPeopleWithEmailAddress(emailAddress).ToList();

            if (people.Count == 1)
            {
                try
                {
                    var newPassword = _usernamePasswordRepository.UpdatePassword(people[0]);
                    return _emailService.SendResetPasswordEmail(people[0], people[0].PersonChurches.First().Church, newPassword);
                }
                catch (Exception)
                {
                    return "There was a problem reseting your password";
                }
                
            }
            return people.Count > 1
                       ? "There is more that one person using this email address in the oikonomos system"
                       : "Your email address could not be found in the database.  Please contact your church administrator for further assistance";
        }
    }
}