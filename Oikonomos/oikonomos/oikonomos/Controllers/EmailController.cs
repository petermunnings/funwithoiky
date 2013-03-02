using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories;
using oikonomos.services;

namespace oikonomos.web.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailService _emailService;

        public EmailController()
        {
            var permissionRepository = new PermissionRepository();
            var personRepository = new PersonRepository(permissionRepository, new ChurchRepository());
            var usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var groupRepository = new GroupRepository();
            var emailSender = new EmailSender(new EmailLogger(new MessageRepository(), personRepository));
            var emailContentService = new EmailContentService(new EmailContentRepository());
            _emailService = new EmailService(
                usernamePasswordRepository,
                personRepository,
                groupRepository,
                emailSender,
                emailContentService
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

    }
}
