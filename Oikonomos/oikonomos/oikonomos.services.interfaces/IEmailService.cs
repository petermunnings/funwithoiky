using System;
using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IEmailService
    {
        void SendEmails(PersonViewModel person, bool sendWelcomeEmail, Church church, Person personToSave, Person currentPerson);
        void EmailGroupLeader(PersonViewModel person, Person currentPerson, Church church, Person personToSave, bool addedToNewGroup);
        void SendExceptionEmail(Exception ex);
        void SendSystemEmail(string subject, string body);
        string SendGroupEmail(IEnumerable<string> addresses, string subject, string body, Person currentPerson);
        bool SendEmailAndPassword(Person currentPerson, int personId, out string message);
        string SendResetPasswordEmail(Person person, Church church, string password);
    }
}