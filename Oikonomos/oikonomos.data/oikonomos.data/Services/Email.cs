using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using oikonomos.common.Models;
using oikonomos.data.DataAccessors;
using System.Web;

namespace oikonomos.data.Services
{
    public class Email
    {
        public static void SendExceptionEmail(Exception ex)
        {
            Task.Factory.StartNew(() => SendExceptionEmailAsync(ex));
        }

        public static void SendSystemEmail(string subject, string body)
        {
            Task.Factory.StartNew(() => SendSystemEmailAsync(subject, body));
        }
        
        
        public static void SendNewVisitorEmail(PersonViewModel person, Church church, string firstname, string surname, string email)
        {
            Task.Factory.StartNew(() => SendNewVisitorEmailAsync(person, church, firstname, surname, email));
        }

        public static string SendGroupEmail(List<string> addresses, string subject, string body, Person currentPerson)
        {
            string message = string.Empty;

            string disclaimer = "\r\n\r\n\r\n______________________________________________________________\r\n\r\n";
            disclaimer += "Please do not reply to this email.  This email was sent from " + currentPerson.Church.SiteHeader + " by ";

            Task.Factory.StartNew(() => SendEmails(addresses, subject, body, currentPerson, disclaimer));

            return "Emails queued for sending";
        }
      
        public static string SendWelcomeEmail(string firstname,
            string surname,
            Church church,
            string email,
            string password,
            bool isVisitor,
            bool includeUsernamePassword)
        {
            Guid guid = Guid.NewGuid();

            Task.Factory.StartNew(() => SendWelcomeEmailAsync(firstname,
                                                  surname,
                                                  church,
                                                  email,
                                                  password,
                                                  guid.ToString(),
                                                  isVisitor,
                                                  includeUsernamePassword));

            return guid.ToString();

        }

        public static string SendResetPasswordEmail(Person person, string password)
        {
            
            Task.Factory.StartNew(() => SendResetPasswordEmail(person.Email,
                                                                person.Firstname,
                                                                password,
                                                                person.Church.SiteHeader,
                                                                person.Church.OfficePhone,
                                                                person.Church.OfficeEmail,
                                                                person.Church.Name,
                                                                person.Church.EmailLogin,
                                                                person.Church.EmailPassword));
            
            return "Password has been reset.  You should receive an email shortly explaining what to do next";

        }

        private static void SendEmails(List<string> addresses, string subject, string body, Person currentPerson, string disclaimer)
        {
            foreach (string address in addresses)
            {
                try
                {
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.Subject = subject;
                        mailMessage.Body = body + disclaimer + currentPerson.Firstname + " " + currentPerson.Family.FamilyName + " (" + currentPerson.Email + ")";
                        mailMessage.IsBodyHtml = false;
                        mailMessage.To.Add(address);
                        SendEmail(mailMessage, currentPerson.Church.EmailLogin, currentPerson.Church.EmailPassword, currentPerson.Fullname + " on behalf of " + currentPerson.Church.Name);
                    }
                }
                catch (Exception ex)
                {
                    //Need to log this
                }
            }
        }
  
        private static void SendWelcomeEmailAsync(string firstname,
            string surname,
            Church church,
            string email,
            string password,
            string guid,
            bool isVisitor,
            bool includeUserNamePassword)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.Subject = "Welcome to " + church.SiteHeader;
                    message.Body = GetWelcomeLetterBodyFromDataBase(
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
                        guid.ToString(),
                        isVisitor,
                        includeUserNamePassword);

                    message.To.Add(email);
                    message.IsBodyHtml = true;

                    SendEmail(message, church.EmailLogin, church.EmailPassword, church.Name);
                }
            }

            catch
            {
                // handle exception here
                // Need to look at some kind of logging
            }
        }

        private static void SendResetPasswordEmail(string email,
                                                    string firstname,
                                                    string password,
                                                    string systemName,
                                                    string officePhone,
                                                    string officeEmail,
                                                    string churchName,
                                                    string churchLogin,
                                                    string churchPassword)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.Subject = "Your password has been reset on " + systemName;
                    message.Body = GetResetPasswordBody(firstname,
                                                   email,
                                                   password,
                                                   systemName,
                                                   churchName,
                                                   officePhone,
                                                   officeEmail);

                    message.To.Add(email);
                    message.IsBodyHtml = true;

                    SendEmail(message, churchLogin, churchPassword, churchName);
                }
            }

            catch
            {
                // handle exception here
                // Need to look at some kind of logging
            }
        }

        private static void SendEmail(MailMessage message, string username, string password, string displayName)
        {
            message.From = new MailAddress(username, displayName);

            using (SmtpClient client = new SmtpClient("mail.oikonomos.co.za"))
            {
                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.Send(message);
            }
        }

        private static void SendExceptionEmailAsync(Exception ex)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.Subject = "Exception from website";
                    message.Body = ex.ToString();

                    message.To.Add("peter@munnings.co.za");
                    message.IsBodyHtml = false;

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", "Oiky Error");
                }
            }
            catch
            {
                // handle exception here
                // Need to look at some kind of logging
            }

        }

        private static void SendSystemEmailAsync(string subject, string body)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.Subject = subject;
                    message.Body = HttpUtility.HtmlDecode(body);

                    message.To.Add("peter@munnings.co.za");
                    message.IsBodyHtml = true;

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", "Oiky System Message");
                }
            }
            catch
            {
                // handle exception here
                // Need to look at some kind of logging
            }

        }

        private static void SendNewVisitorEmailAsync(PersonViewModel person, 
            Church church,
            string firstname,
            string surname,
            string email)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.Subject = "A new visitor to " + church.Name + " has been added to your homegroup";
                    message.Body = GetNewVisitorEmailBody(firstname,
                                           surname,
                                           church.Name,
                                           person);

                    message.To.Add(email);
                    message.IsBodyHtml = true;

                    SendEmail(message, church.EmailLogin, church.EmailPassword, church.Name);
                }
            }
            catch
            {
                // handle exception here
                // Need to look at some kind of logging
            }

        }

        private static string GetNewVisitorEmailBody(string firstname,
                                           string surname,
                                           string churchName,
                                           PersonViewModel person)
        {
            StringBuilder b = new StringBuilder();

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
            if ((person.Email != null && person.Email != string.Empty) ||
               (person.CellPhone != null && person.CellPhone != string.Empty) ||
               (person.HomePhone != null && person.HomePhone != string.Empty) ||
               (person.WorkPhone != null && person.WorkPhone != string.Empty))
            {
                b.AppendLine(person.Firstname + "'s contact details are:");
                b.AppendLine("                        <ul>");
                if (person.Email != null && person.Email != string.Empty)
                {
                    b.AppendLine("                            <li>Email: " + person.Email);
                }
                if (person.CellPhone != null && person.CellPhone != string.Empty)
                {
                    b.AppendLine("                            <li>Cell Phone: " + person.CellPhone);
                }
                if (person.HomePhone != null && person.HomePhone != string.Empty)
                {
                    b.AppendLine("                            <li>Home Phone: " + person.HomePhone);
                }
                if (person.WorkPhone != null && person.WorkPhone != string.Empty)
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

        private static string GetWelcomeLetterBody(string firstname,
            string surname,
            string systemName,
            string churchName,
            string churchOfficeNo,
            string churchOfficeEmail,
            string churchWebsite,
            string email,
            string password,
            string guid,
            bool isVisitor,
            bool includeUsernamePassword)
        {
            StringBuilder b = new StringBuilder();
            
            b.AppendLine("<html>");
            b.AppendLine("<head>");
            if (isVisitor)
            {
                b.AppendLine("    <title>Welcome to " + churchName + " </title>");
            }
            else
            {
                b.AppendLine("    <title>Welcome to " + systemName + " </title>");
            }
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
            b.AppendLine("                            <span style='font-size:14.0pt'>Hi " + firstname + " " + surname + "</span>&nbsp;");
            b.AppendLine("                        </p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            if (isVisitor)
            {
                b.AppendLine("                        <p>Thank you for visiting us at " + churchName + ".  We'd love to get to know you better, and let you experience different aspects and expressions of church life.  There is more information on our website <a href='" + churchWebsite + "'>" + churchWebsite + "</a>.  We also meet in groups during the week and that's a great place to meet people in the church.  You can find out more about the groups on the website, and we encourage you to visit one close to you.");
                b.AppendLine("                        <p>&nbsp;</p>");
                if (includeUsernamePassword)
                {
                    b.AppendLine("                        <p>This is also to let you know that you now have access to " + systemName + ".  This is a fun new way of keeping in touch with people in our church.  You'll be able to <ul><li>update your details</li><li>lookup other people's details</li><li>see what events/birthdays/anniversaries are about to happen</li></ul>");
                    b.AppendLine("                        </p>");
                    b.AppendLine("                        <p>" + systemName + " is completely private and safe and your details are not shared with anyone outside the church.  Even if you log in using facebook - nothing is sent to facebook from " + systemName + ".  It simply uses facebook to check that you are who you say you are and it fetches your profile picture so people who are good with faces and not names can remember you.");
                    b.AppendLine("                        <p>&nbsp;</p>");
                    b.AppendLine("                        <p>In order to access it, go to <a href=\"http://www.oikonomos.co.za/Home/Login/" + guid + "\">");
                    b.AppendLine("                            www.oikonomos.co.za</a> and do one of the following:</p>");
                    b.AppendLine("                        <ul><li>If you have a facebook account you can click on the <b>Login using facebook button</b>.  This will then use your facebook username and password to connect you to the system.  It saves you remembering another password, and gives you additional functionality on " + systemName + "</li>");
                    b.AppendLine("                        <li>If not, you can login with the following credentials</li></ul>");
                    b.AppendLine("                        <p>Email: " + email + "</p>");
                    b.AppendLine("                        <p>Password: " + password + "</p>");
                    b.AppendLine("                        <p>&nbsp;</p>");
                    b.AppendLine("                        <p>Once you have logged in, you can change your password by clicking on the <b>Settings</b> menu button</p><p>&nbsp;</p>");
                }
            }
            else
            {
                b.AppendLine("                        <p>This is to let you know that you now have access to " + systemName + ".  This is a fun new way of keeping in touch with people in your church.  You'll be able to <ul><li>update your details</li><li>lookup other people's details</li><li>see what events/birthdays/anniversaries are about to happen</li></ul>");
                b.AppendLine("                        </p>");
                b.AppendLine("                        <p>" + systemName + " is completely private and safe and your details are not shared with anyone outside the church.  Even if you log in using facebook - nothing is sent to facebook from " + systemName + ".  It simply uses facebook to check that you are who you say you are and it fetches your profile picture so people who are good with faces and not names can remember you.");
                b.AppendLine("                        <p>&nbsp;</p>");
                b.AppendLine("                        <p>In order to access it, go to <a href=\"http://www.oikonomos.co.za/Home/Login/" + guid + "\">");
                b.AppendLine("                            www.oikonomos.co.za</a> and do one of the following:</p>");
                b.AppendLine("                        <ul><li>If you have a facebook account you can click on the <b>Login using facebook button</b>.  This will then use your facebook username and password to connect you to the system.  It saves you remembering another password, and gives you additional functionality on " + systemName + "</li>");
                b.AppendLine("                        <li>If not, you can login with the following credentials</li></ul>");
                b.AppendLine("                        <p>Email: " + email + "</p>");
                b.AppendLine("                        <p>Password: " + password + "</p>");
                b.AppendLine("                        <p>&nbsp;</p>");
                b.AppendLine("                        <p>Once you have logged in, you can change your password by clicking on the <b>Settings</b> menu button</p><p>&nbsp;</p>");
            }
            b.AppendLine("                        <p>Should you have any queries, do not hesitate to contact us at " + churchName + " on " + churchOfficeNo + " or ");
            b.AppendLine("                            " + churchOfficeEmail + "</p>");
            b.AppendLine("                        <p>&nbsp;</p>");
            if (isVisitor)
            {
                b.AppendLine("                        <p>Looking forward to seeing you again soon</p>");
            }
            else
            {
                b.AppendLine("                        <p>Enjoy</p>");
            }
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

        private static string GetWelcomeLetterBodyFromDataBase(
            int    churchId,
            string firstname,
            string surname,
            string systemName,
            string churchName,
            string churchOfficeNo,
            string churchOfficeEmail,
            string churchWebsite,
            string email,
            string password,
            string guid,
            bool   isVisitor,
            bool   includeUsernamePassword)
        {
            string emailBody = string.Empty;
            if (isVisitor)
            {
                emailBody = EmailDataAccessor.GetVisitorWelcomeLetter(churchId);
            }
            else
            {
                emailBody = EmailDataAccessor.GetMemberWelcomeLetter(churchId);
            }

            emailBody = emailBody.Replace("##SystemName##", systemName);
            emailBody = emailBody.Replace("##Firstname##", firstname);
            emailBody = emailBody.Replace("##Surname##", surname);
            emailBody = emailBody.Replace("##PublicId##", guid);
            emailBody = emailBody.Replace("##Email##", email);
            emailBody = emailBody.Replace("##Password##", password);
            emailBody = emailBody.Replace("##ChurchName##", churchName);
            emailBody = emailBody.Replace("##ChurchOfficeNo##", churchOfficeNo);
            emailBody = emailBody.Replace("##ChurchOfficeEmail##", churchOfficeEmail);
            emailBody = emailBody.Replace("##ChurchWebsite##", churchWebsite);

            return emailBody;
        }

        private static string GetResetPasswordBody(string firstname,
                                                   string email,
                                                   string password,
                                                   string systemName,
                                                   string churchName,
                                                   string officePhone,
                                                   string officeEmail)
        {
            StringBuilder b = new StringBuilder();

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