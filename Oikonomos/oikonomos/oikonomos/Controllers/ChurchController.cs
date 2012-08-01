using System.Web.Mvc;
using oikonomos.data.DataAccessors;
using oikonomos.common;

namespace oikonomos.web.Controllers
{
    public class ChurchController : Controller
    {
        public ActionResult Name(string id)
        {
            var churchViewModel             = ChurchDataAccessor.FetchChurch(id);
            Session[SessionVariable.Church] = churchViewModel;
            ViewBag.Message                 = "Please login below";
            return View("../Home/Login");
        }

    }
}
