using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using oikonomos.data;
using oikonomos.common;
using oikonomos.data.DataAccessors;
using System.Web.Security;

namespace oikonomos.web.Helpers
{
    public static class SecurityHelper
    {
        public static void CheckCurrentUser(long facebookId, Person currentUser, HttpSessionStateBase Session, HttpResponseBase Response, dynamic ViewBag)
        {
            Session[SessionVariable.LoggedOnPerson] = currentUser;
            Session[SessionVariable.Church] = ChurchDataAccessor.FetchChurch(currentUser.Church.Name);
            FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);
            if (!CheckRoles(currentUser, Response, ViewBag))
            {
                ViewBag.Message = "You are not registered on oikonomos.  Please speak to your church administrator";
            }
        }

        public static Person CheckCurrentUser(HttpSessionStateBase Session, HttpResponseBase Response, dynamic ViewBag)
        {
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                ViewBag.Message = "Please login below";
                return null;
            }

            Person currentUser = (Person)Session[SessionVariable.LoggedOnPerson];

            if (!CheckRoles(currentUser, Response, ViewBag))
            {
                ViewBag.Message = "Please login below";
                return null;
            }
            return currentUser;

        }

        private static bool CheckRoles(Person currentUser, HttpResponseBase Response, dynamic ViewBag)
        {
            ViewBag.CurrentUser = currentUser;
            
            Response.Cookies["PersonId"].Value = currentUser.PersonId.ToString();
            Response.Cookies["PersonId"].Expires = DateTime.Now.AddMonths(12);
            return true;
        }
        

    }
}