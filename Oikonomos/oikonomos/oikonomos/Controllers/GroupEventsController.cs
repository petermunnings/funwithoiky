using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories;
using oikonomos.repositories.Messages;
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.web.Controllers
{
    public class GroupEventsController : Controller
    {
        private readonly IEventService _eventService;

        public GroupEventsController()
        {
            var birthdayRepository = new BirthdayRepository();
            var permissionRepository = new PermissionRepository();
            var personRepository = new PersonRepository(permissionRepository, new ChurchRepository());
            var usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var groupRepository = new GroupRepository();
            var messageRepository = new MessageRepository();
            var messageRecepientRepository = new MessageRecepientRepository();
            var messageAttachmentRepository = new MessageAttachmentRepository();
            var emailSender = new EmailSender(messageRepository, messageRecepientRepository, messageAttachmentRepository, personRepository);
            var churchEmailTemplatesRepository = new ChurchEmailTemplatesRepository();
            var emailContentRepository = new EmailContentRepository();
            var emailContentService = new EmailContentService(emailContentRepository);
            var emailService = new EmailService(usernamePasswordRepository, personRepository, groupRepository, emailSender, emailContentService, churchEmailTemplatesRepository);
            var eventRepository = new EventRepository(birthdayRepository);
            _eventService = new EventService(eventRepository, emailService);
        }
        
        public JsonResult GetGroupEvents(int groupId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person) Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.ViewDiscipleship101))
                {
                    return Json(_eventService.GetPersonEventsForGroup(groupId, currentPerson));
                }
            }
            return Json(new List<PersonEventDto>());
        }

        public JsonResult UpdatePersonEvent(int personId, int eventId, bool completed)
    {
            try                                  
            {
                if (Session[SessionVariable.LoggedOnPerson] != null)
                {
                    var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    if (currentPerson.HasPermission(Permissions.ViewDiscipleship101))
                    {
                        _eventService.UpdatePersonEvent(personId, eventId, completed);
                        return Json("success");
                    }
                }
            }
            catch (Exception)
            {
                return Json("failure");
            }
            
            return Json("failure");
        }
        

    }
}
