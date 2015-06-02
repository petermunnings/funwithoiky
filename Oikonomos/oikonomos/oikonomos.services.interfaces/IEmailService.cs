using System;
using System.Collections.Generic;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IEmailService
    {
        void SendWelcomeEmail(PersonViewModel person, bool sendWelcomeEmail, Church church, Person personToSave, Person currentPerson);
        void EmailGroupLeader(PersonViewModel person, Person currentPerson, Church church, Person personToSave, bool addedToNewGroup);
        void SendExceptionEmail(Exception ex);
        void SendSystemEmail(string subject, string body);
        string SendGroupEmail(IEnumerable<string> addresses, string subject, string body, Person currentPerson, IEnumerable<UploadFilesResult> attachmentList);
        bool SendEmailAndPassword(Person currentPerson, int personId, out string message);
        string SendResetPasswordEmail(Person person, Church church, string password);
        string SendQueuedEmail(MessageQueueViewModel queuedMessage);
        void SendUpdateNotification(int churchId, Person currentPerson, Person personBeingUpdated);
    }
}