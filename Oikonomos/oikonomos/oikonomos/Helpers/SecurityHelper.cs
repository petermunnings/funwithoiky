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
        public static void CheckCurrentUser(long facebookId, Person currentUser, HttpSessionStateBase session, HttpResponseBase response, dynamic viewBag)
        {
            session[SessionVariable.LoggedOnPerson] = currentUser;
            session[SessionVariable.Church] = ChurchDataAccessor.FetchChurch(currentUser.Church.Name);
            FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);
            if (!CheckRoles(currentUser, response, viewBag))
            {
                viewBag.Message = "You are not registered on oikonomos.  Please speak to your church administrator";
            }
        }

        public static Person CheckCurrentUser(HttpSessionStateBase session, HttpResponseBase response, dynamic viewBag)
        {
            if (session[SessionVariable.LoggedOnPerson] == null)
            {
                viewBag.Message = "Please login below";
                return null;
            }

            var currentUser = (Person)session[SessionVariable.LoggedOnPerson];

            if (!CheckRoles(currentUser, response, viewBag))
            {
                viewBag.Message = "Please login below";
                return null;
            }
            return currentUser;

        }

        private static bool CheckRoles(Person currentUser, HttpResponseBase response, dynamic viewBag)
        {
            viewBag.CurrentUser = currentUser;
            
            response.Cookies["PersonId"].Value = currentUser.PersonId.ToString();
            response.Cookies["PersonId"].Expires = DateTime.Now.AddMonths(12);
            return true;
        }
        

    }
}