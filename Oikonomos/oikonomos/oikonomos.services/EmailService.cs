using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EmailService : IEmailService
    {
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailContentService _emailContentService;
        private readonly IChurchEmailTemplatesRepository _churchEmailTemplatesRepository;

        public EmailService(IUsernamePasswordRepository usernamePasswordRepository, IPersonRepository personRepository, IGroupRepository groupRepository, IEmailSender emailSender, IEmailContentService emailContentService, IChurchEmailTemplatesRepository churchEmailTemplatesRepository)
        {
            _usernamePasswordRepository = usernamePasswordRepository;
            _personRepository = personRepository;
            _groupRepository = groupRepository;
            _emailSender = emailSender;
            _emailContentService = emailContentService;
            _churchEmailTemplatesRepository = churchEmailTemplatesRepository;
        }

        public void SendEmails(PersonViewModel person, bool sendWelcomeEmail, Church church, Person personToSave, Person currentPerson)
        {
            if (sendWelcomeEmail && personToSave.HasValidEmail() &&
                (personToSave.HasPermission(Permissions.Login) || personToSave.HasPermission(Permissions.SendWelcomeLetter)))
            {
                SendEmailAndPassword(
                    person.Firstname,
                    person.Surname,
                    person.Email,
                    personToSave,
                    currentPerson);
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
                SendNewVisitorEmail(person, church, group.Leader.Firstname, group.Leader.Family.FamilyName, group.Leader.Email, currentPerson);
            }
            else if (group.Administrator != null && group.Administrator.HasValidEmail() && group.LeaderId != currentPerson.PersonId)
            {
                SendNewVisitorEmail(person, church, group.Administrator.Firstname, group.Administrator.Family.FamilyName, group.Administrator.Email, currentPerson);
            }
        }

        public void SendExceptionEmail(Exception ex)
        {
            Task.Factory.StartNew(() => _emailSender.SendExceptionEmailAsync(ex));
        }

        public void SendSystemEmail(string subject, string body)
        {
            Task.Factory.StartNew(() => _emailSender.SendSystemEmailAsync(subject, body));
        }

        public string SendGroupEmail(IEnumerable<string> addresses, string subject, string body, Person currentPerson, IEnumerable<UploadFilesResult> attachmentList)
        {
            body = AddChurchSignature(body, currentPerson);
            Task.Factory.StartNew(() => _emailSender.SendEmail(subject, body, currentPerson.Church.Name, addresses, currentPerson.Church.EmailLogin, currentPerson.Church.EmailPassword, currentPerson.PersonId, currentPerson.Church.ChurchId, attachmentList));
            return "Emails queued for sending";
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
                                         personToUpdate.Email,
                                         personToUpdate,
                                         currentPerson);

                    message = "Email sent succesfully";
                    return true;
                }
                message = "Invalid Email address";
                return false;
            }
            message = string.Format("You cannot send login details to a person with a role of {0}", personToUpdate.Role.Name);
            return false;
        }

        public string SendResetPasswordEmail(Person person, Church church, string password)
        {
            var subject = "Your password has been reset on " + church.SiteHeader;
            var body = GetResetPasswordBody(person.Firstname,
                                           person.Email,
                                           password,
                                           church.SiteHeader,
                                           church.Name,
                                           church.OfficePhone,
                                           church.OfficeEmail);

            _emailSender.SendEmail(subject, body, church.Name, new[] { person.Email }, church.EmailLogin, church.EmailPassword, person.PersonId, church.ChurchId, new List<UploadFilesResult>());
            return "Password has been reset.  You should receive an email shortly explaining what to do next";
        }

        private void SendEmailAndPassword(string firstname, string surname, string email, Person personToSave, Person currentPerson)
        {
            _usernamePasswordRepository.UpdateUsername(personToSave);
            var password = _usernamePasswordRepository.UpdatePassword(personToSave);
            var publicId = _personRepository.UpdatePublicId(personToSave);
            SendWelcomeEmail(firstname,
                  surname,
                  email,
                  password,
                  publicId,
                  personToSave.HasPermission(Permissions.SendVisitorWelcomeLetter),
                  false,
                  currentPerson);
        }

        private void SendWelcomeEmail(string firstname, string surname, string email, string password, Guid publicId, bool isVisitor, bool includeUserNamePassword, Person currentPerson)
        {
            var church = currentPerson.Church;
            var subject = "Welcome to " + church.SiteHeader;
            var body = _emailContentService.GetWelcomeLetterBodyFromDataBase(
                        church.ChurchId,
                        firstname,
                        surname,
                        church.SiteHeader,
                        church.Name,
                        church.OfficePhone,
                        church.OfficeEmail,
                        church.Url,
                        email,
                        password,
                        publicId.ToString(),
                        isVisitor,
                        includeUserNamePassword);

            Task.Factory.StartNew(() => _emailSender.SendEmail(subject, body, church.Name, new[] { email }, church.EmailLogin, church.EmailPassword, currentPerson.PersonId, church.ChurchId, new List<UploadFilesResult>()));

        }

        private string AddChurchSignature(string body, Person currentPerson)
        {
            var signature = _churchEmailTemplatesRepository.FetchChurchEmailSignature(currentPerson.ChurchId);
            if (signature != null)
                body += signature;
            return body;
        }

        private void SendNewVisitorEmail(PersonViewModel person, Church church, string firstname, string surname, string email, Person currentPerson)
        {
           var subject = "A new visitor to " + church.Name + " has been added to your homegroup";
           var body = GetNewVisitorEmailBody(firstname, surname, church.Name, person);

           Task.Factory.StartNew(() => _emailSender.SendEmail(subject, body, church.Name, new[] { email }, church.EmailLogin, church.EmailPassword, currentPerson.PersonId, church.ChurchId, new List<UploadFilesResult>()));
        }


        private static string GetResetPasswordBody(string firstname,
                                           string email,
                                           string password,
                                           string systemName,
                                           string churchName,
                                           string officePhone,
                                           string officeEmail)
        {
            var b = new StringBuilder();

            b.AppendLine("<html>");
            b.AppendLine("<head>");
            AddStyle(b);
            b.AppendLine("</head>");
            b.AppendLine("<body style=\"background-color: #FFF7CC\">");
            b.AppendLine("    <div>");
            b.AppendLine("        <div align=\"left\">");
            b.AppendLine("            <div style=\"display:table\">");
            b.AppendLine("                <div style=\"display:table-row\">");
            b.AppendLine("                    <div style=\"display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm\">");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div style=\"display:table-row\">");
            b.AppendLine("                    <div style=\"display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm\">");
            b.AppendLine("                    </div>                ");
            b.AppendLine("                    <div style=\"display:table-cell; width:537.75pt;padding:0cm 0cm 0cm 0cm\">");
            b.AppendLine("                        <p>");
            b.AppendLine("                            <span style='font-size:14.0pt'>Hi " + firstname + "</span>&nbsp;");
            b.AppendLine("                        </p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            b.AppendLine("                        <p>Your password on " + systemName + " has been reset.  ");

            b.AppendLine("                        <p>In order to access it, go to <a href=\"http://www.oikonomos.co.za/Home/Login/\">");
            b.AppendLine("                            www.oikonomos.co.za</a> and do one of the following:</p>");
            b.AppendLine("                        <ul><li>If you have a facebook account you can click on the <b>Login using facebook button</b>.  This will then use your facebook username and password to connect you to the system.  It saves you remembering another password, and gives you additional functionality on " + systemName + "</li>");
            b.AppendLine("                        <li>If not, you can login with the following credentials</li></ul>");
            b.AppendLine("                        <p>Email: " + email + "</p>");
            b.AppendLine("                        <p>Password: " + password + "</p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            b.AppendLine("                        <p>Once you have logged in, you can change your password by clicking on the <b>Settings</b> menu button</p><p>&nbsp;</p>");
            b.AppendLine("                        <p>Should you have any queries, do not hesitate to contact us at " + churchName + " on " + officePhone + " or ");
            b.AppendLine("                            " + officeEmail + "</p>");
            b.AppendLine("                        <p>&nbsp;</p>");

            b.AppendLine("                        <p>Enjoy</p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            b.AppendLine("                     </div>");
            b.AppendLine("                 </div>");
            b.AppendLine("             </div>");
            b.AppendLine("         </div>");
            b.AppendLine("     <p >&nbsp;    </p>");
            b.AppendLine(" </div>");
            b.AppendLine("</body>");
            b.AppendLine("</html>");

            return b.ToString();
        }

        private static string GetNewVisitorEmailBody(string firstname,
                                           string surname,
                                           string churchName,
                                           PersonViewModel person)
        {
            var b = new StringBuilder();

            b.AppendLine("<html>");
            b.AppendLine("<head>");
            b.AppendLine("    <title>A new visitor would like to be integrated into the church</title>");
            AddStyle(b);
            b.AppendLine("</head>");
            b.AppendLine("<body style=\"background-color: #FFF7CC\">");
            b.AppendLine("    <div>");
            b.AppendLine("        <div align=\"center\">");
            b.AppendLine("            <div style=\"display:table\">");
            b.AppendLine("                <div style=\"display:table-row\">");
            b.AppendLine("                    <div style=\"display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm\">");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div style=\"display:table-row\">");
            b.AppendLine("                    <div style=\"display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm\">");
            b.AppendLine("                    </div>                ");
            b.AppendLine("                    <div style=\"display:table-cell; width:537.75pt;padding:0cm 0cm 0cm 0cm\">");
            b.AppendLine("                        <p>");
            b.AppendLine("                            <span style='font-size:14.0pt'>Hi " + firstname + " " + surname + "</span>&nbsp;");
            b.AppendLine("                        </p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            b.AppendLine("                        <p>This is to let you know that " + person.Firstname + " " + person.Surname + " who has been visiting " + churchName + " has been allocated to your homegroup.");
            b.AppendLine("                        </p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            b.AppendLine("                        <p>Please can you contact " + person.Firstname + " with an invite to your homegroup.  ");
            if (!string.IsNullOrEmpty(person.Email) ||
               !string.IsNullOrEmpty(person.CellPhone) ||
               !string.IsNullOrEmpty(person.HomePhone) ||
               !string.IsNullOrEmpty(person.WorkPhone))
            {
                b.AppendLine(person.Firstname + "'s contact details are:");
                b.AppendLine("                        <ul>");
                if (!string.IsNullOrEmpty(person.Email))
                {
                    b.AppendLine("                            <li>Email: " + person.Email);
                }
                if (!string.IsNullOrEmpty(person.CellPhone))
                {
                    b.AppendLine("                            <li>Cell Phone: " + person.CellPhone);
                }
                if (!string.IsNullOrEmpty(person.HomePhone))
                {
                    b.AppendLine("                            <li>Home Phone: " + person.HomePhone);
                }
                if (!string.IsNullOrEmpty(person.WorkPhone))
                {
                    b.AppendLine("                            <li>Work Phone: " + person.WorkPhone);
                }
                b.AppendLine("                        </ul>");
            }
            b.AppendLine("                        <p>&nbsp;</p>");
            b.AppendLine("                        <p>This is a wonderful opportunity to see God's kingdom advance.  Thank you.</p>");
            b.AppendLine("                     </div>");
            b.AppendLine("                 </div>");
            b.AppendLine("             </div>");
            b.AppendLine("         </div>");
            b.AppendLine("     <p >&nbsp;    </p>");
            b.AppendLine(" </div>");
            b.AppendLine("</body>");
            b.AppendLine("</html>");

            return b.ToString();

        }

        private static void AddStyle(StringBuilder b)
        {
            b.AppendLine("    <style type=\"text/css\">");
            b.AppendLine("        @font-face	");
            b.AppendLine("        {");
            b.AppendLine("            font-family:Calibri");
            b.AppendLine("        }");
            b.AppendLine("        ");
            b.AppendLine("        a:link, span.MsoHyperlink	");
            b.AppendLine("        {");
            b.AppendLine("            color:#773509;	");
            b.AppendLine("            text-decoration:none none;");
            b.AppendLine("        }");
            b.AppendLine("        ");
            b.AppendLine("        p	");
            b.AppendLine("        {");
            b.AppendLine("            margin-top:0cm;	");
            b.AppendLine("            margin-right:0cm;	");
            b.AppendLine("            margin-bottom:0cm;	");
            b.AppendLine("            margin-left:0pt;	");
            b.AppendLine("            margin-bottom:.0001pt;	");
            b.AppendLine("            text-align:justify;	");
            b.AppendLine("            font-size:14.0pt;	");
            b.AppendLine("            font-family:\"Calibri\";	");
            b.AppendLine("            color:#212A33;");
            b.AppendLine("            }");
            b.AppendLine("        li	");
            b.AppendLine("        {");
            b.AppendLine("            font-size:14.0pt;	");
            b.AppendLine("            font-family:\"Calibri\";	");
            b.AppendLine("            color:#212A33;");
            b.AppendLine("            text-align:left;");
            b.AppendLine("            }");
            b.AppendLine("    </style>");
        }
    }
}