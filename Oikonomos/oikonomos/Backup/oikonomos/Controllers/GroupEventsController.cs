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
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.web.Controllers
{
    public class GroupEventsController : Controller
    {
        private readonly IEventService _eventService;

        public GroupEventsController()
        {
            var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString);
            _eventService = new EventService(new EventRepository(context));
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
                        _eventService.UpdatePersonEvent(personId, eventId, completed, currentPerson);
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
