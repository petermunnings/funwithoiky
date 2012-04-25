using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using oikonomos.common;
using oikonomos.data;

namespace oikonomos.web.Controllers
{
    public class EmailController : Controller
    {

        public JsonResult SendSystemEmail(string subject, string body)
        {
            try
            {
                try
                {
                    if (Session[SessionVariable.LoggedOnPerson] != null)
                    {
                        Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                        body = currentPerson.Fullname + body;
                    }
                }
                catch { }
                
                oikonomos.data.Services.Email.SendSystemEmail(subject, body);
                return Json(new { Result = "Success" });
            }
            catch
            {
                return Json(new { Result = "Failure" });
            }
        }

    }
}
