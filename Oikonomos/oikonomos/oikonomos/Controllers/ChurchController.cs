using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using oikonomos.common.Models;
using oikonomos.data.DataAccessors;
using oikonomos.common;

namespace oikonomos.web.Controllers
{
    public class ChurchController : Controller
    {
        //
        // GET: /Church/

        public ActionResult Name(string id)
        {
            ChurchViewModel churchViewModel = ChurchDataAccessor.FetchChurch(id);
            Session[SessionVariable.Church] = churchViewModel;
            ViewBag.Message = "Please login below";
            return View("../Home/Login");
        }

    }
}
