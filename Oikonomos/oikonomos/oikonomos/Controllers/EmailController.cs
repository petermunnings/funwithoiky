using System;
using System.Diagnostics;
using System.Web.Mvc;
using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.repositories.Messages;
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.web.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IMessageRecepientRepository _messageRecepientRepository;

        public EmailController()
        {
            var permissionRepository = new PermissionRepository();
            var personRepository = new PersonRepository(permissionRepository, new ChurchRepository());
            var usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var groupRepository = new GroupRepository();
            _messageRecepientRepository = new MessageRecepientRepository();
            var emailSender = new EmailSender(new MessageRepository(), _messageRecepientRepository, new MessageAttachmentRepository(), personRepository);
            var emailContentService = new EmailContentService(new EmailContentRepository());
            var churchEmailTemplateRepository = new ChurchEmailTemplatesRepository();
            _emailService = new EmailService(
                usernamePasswordRepository,
                personRepository,
                groupRepository,
                emailSender,
                emailContentService,
                churchEmailTemplateRepository
                );
        }

        public JsonResult SendSystemEmail(string subject, string body)
        {
            try
            {
                try
                {
                    if (Session[SessionVariable.LoggedOnPerson] != null)
                    {
                        var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                        body = currentPerson.Fullname + body;
                    }
                }
                catch { }

                _emailService.SendSystemEmail(subject, body);
                return Json(new { Result = "Success" });
            }
            catch
            {
                return Json(new { Result = "Failure" });
            }
        }

        public JsonResult SendQueuedEmail()
        {
            try
            {
                var nextQueuedMessage = _messageRecepientRepository.GetNextQueuedEmail();
                var noOutstandingMessages = _messageRecepientRepository.GetNoOfOutstandingMessages();
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (nextQueuedMessage != null && stopWatch.ElapsedMilliseconds < 60000)
                {
                    _emailService.SendQueuedEmail(nextQueuedMessage);
                    nextQueuedMessage = _messageRecepientRepository.GetNextQueuedEmail();
                    noOutstandingMessages = _messageRecepientRepository.GetNoOfOutstandingMessages();
                }
                var message = noOutstandingMessages == 0
                    ? "All messages sent"
                    : string.Format("{0} messages still queued", noOutstandingMessages);

                return Json(new { Result = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Failure: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
