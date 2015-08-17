using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using oikonomos.common.DTOs;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.repositories.Messages;
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.web.Controllers
{
    public class RemindersController : Controller
    {
        private readonly IReminderService _reminderService;
        private readonly IEmailSender _emailSender;
        private readonly IChurchRepository _churchRepository;
        private readonly IBirthdayAndAnniversaryService _birthdayAndAnniversaryService;
        private readonly IDictionary<int, string> _monthNames;

        public RemindersController()
        {
            _reminderService = new ReminderService(new ReminderRepository());
            var permissionRepository = new PermissionRepository();
            _churchRepository = new ChurchRepository();
            var personRepository = new PersonRepository(permissionRepository, _churchRepository);
            _birthdayAndAnniversaryService = new BirthdayAndAnniversaryService(new BirthdayAndAniversaryRepository());
            _emailSender = new EmailSender(new MessageRepository(), new MessageRecepientRepository(), new MessageAttachmentRepository(), personRepository);
            _monthNames = new Dictionary<int, string>
            {
                {1, "January"},
                {2, "February"},
                {3, "March"},
                {4, "April"},
                {5, "May"},
                {6, "June"},
                {7, "July"},
                {8, "August"},
                {9, "September"},
                {10, "October"},
                {11, "November"},
                {12, "December"}
            };
        }

        public JsonResult Index()
        {
            //If it is the end of the month - get a list of reminders for the next month
            if (DateTime.Today.Day == 25)
            {
                var list = _reminderService.GetListOfMonthlyReminders("BirthdayAnniversary");
                var sortedByChurch = list.OrderBy(l => l.ChurchId);
                var churchId = 0;
                var listOfEmails = new List<string>();
                foreach (var person in sortedByChurch)
                {
                    if (churchId != person.ChurchId)
                    {
                        CheckForRecepientsAndSend(listOfEmails, churchId);

                        churchId = person.ChurchId;
                        listOfEmails = new List<string>();
                    }
                    listOfEmails.Add(person.Email);
                }
                CheckForRecepientsAndSend(listOfEmails, churchId);
            }

            //If it is the end of the week - get a list of reminders for the next week


            return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
        }

        private void CheckForRecepientsAndSend(List<string> listOfEmails, int churchId)
        {
            if (listOfEmails.Any())
            {
                var churchAdmins = _churchRepository.GetChurchAdmins(churchId);
                var churchAdminId = churchAdmins.Any() ? churchAdmins.First().PersonId : 1;

                //generate birthday and anniversary report
                var selectedMonth = DateTime.Today.AddMonths(1).Month;
                var birthdayFileStream = _birthdayAndAnniversaryService.GetBirthdayListForAMonth(selectedMonth, churchId);
                var anniversaryFileStream = _birthdayAndAnniversaryService.GetAnniversaryListForAMonth(selectedMonth, churchId);
                var birthdayFileContent = ReadToEnd(birthdayFileStream);
                var anniversaryFileContent = ReadToEnd(anniversaryFileStream);

                var birthdayAttachment = new UploadFilesResult
                {
                    Name = "BirthdayList.csv",
                    AttachmentContent = birthdayFileContent,
                    Length = birthdayFileContent.Length,
                    AttachmentContentType = "text/csv",
                    Type = "text/csv"
                };

                var anniversaryAttachment = new UploadFilesResult
                {
                    Name = "AnniversaryList.csv",
                    AttachmentContent = anniversaryFileContent,
                    Length = anniversaryFileContent.Length,
                    AttachmentContentType = "text/csv",
                    Type = "text/csv"
                };

                _emailSender.QueueEmails(
                    "Birthday and Anniversary reminder for the month of " + _monthNames[selectedMonth],
                    "Hi, attached is a list of birthdays and anniversaries for next month",
                    listOfEmails,
                    churchAdminId,
                    churchId,
                    new List<UploadFilesResult>
                    {
                        birthdayAttachment,
                        anniversaryAttachment
                    });
            }
        }

        private static byte[] ReadToEnd(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
