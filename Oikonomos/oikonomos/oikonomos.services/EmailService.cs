using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.data.Services;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EmailService : IEmailService
    {
        private readonly IPasswordService _passwordService;
        private readonly IGroupRepository _groupRepository;

        public EmailService(IPasswordService passwordService, IGroupRepository groupRepository)
        {
            _passwordService = passwordService;
            _groupRepository = groupRepository;
        }

        public void SendEmails(PersonViewModel person, bool sendWelcomeEmail, Church church, Person personToSave)
        {
            if (sendWelcomeEmail && personToSave.HasValidEmail() &&
                (personToSave.HasPermission(Permissions.Login) || personToSave.HasPermission(Permissions.SendWelcomeLetter)))
            {
                _passwordService.SendEmailAndPassword(
                    person.Firstname,
                    person.Surname,
                    church,
                    person.Email,
                    personToSave);
            }
        }

        public void EmailGroupLeader(PersonViewModel person, Person currentPerson, Church church, Person personToSave, bool addedToNewGroup)
        {
            if (!personToSave.HasPermission(Permissions.NotifyGroupLeaderOfVisit) || person.GroupId <= 0) return;
            var sendEmailToGroupLeader = person.PersonId == 0;
            var group = _groupRepository.GetGroup(person.GroupId);

            if (group==null)
                return;
            if (addedToNewGroup)
                sendEmailToGroupLeader = true;

            if (group.LeaderId == currentPerson.PersonId || group.AdministratorId == currentPerson.PersonId)
                sendEmailToGroupLeader = false;  //This is the groupleader

            if (!sendEmailToGroupLeader) return;
            if (group.Leader != null && group.Leader.HasValidEmail() && group.LeaderId != currentPerson.PersonId)
            {
                Email.SendNewVisitorEmail(person, church, group.Leader.Firstname, group.Leader.Family.FamilyName, group.Leader.Email);
            }
            else if (group.Administrator != null && group.Administrator.HasValidEmail() && group.LeaderId != currentPerson.PersonId)
            {
                Email.SendNewVisitorEmail(person, church, group.Administrator.Firstname, group.Administrator.Family.FamilyName, group.Administrator.Email);
            }
        }
    }
}