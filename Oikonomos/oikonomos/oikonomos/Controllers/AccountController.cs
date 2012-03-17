using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web;
using Facebook;
using System.Web.Security;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.common.Models;
using oikonomos.common;
using System.Configuration;
using oikonomos.web.Helpers;

namespace oikonomos.web.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login(string id, string returnUrl)
        {
            if (id != null)
            {
                Session["PersonTryingToLogin"] = PersonDataAccessor.FetchPersonFromPublicId(id);
            }

            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(ConfigurationManager.AppSettings["RedirectUrl"]);
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
            return Redirect(loginUri.AbsoluteUri + "&scope=user_birthday,email");
        }

        //
        // GET: /Account/OAuth/

        public ActionResult OAuth(string code, string state)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    string accessToken = GetAccessToken(code);

                    dynamic me;
                    long facebookId;
                    DateTime? birthdate;
                    GetFacebookDetails(accessToken, out me, out facebookId, out birthdate);

                    //Check for the id in the database
                    Person currentUser = PersonDataAccessor.FetchPersonFromFacebookId(facebookId);
                    if (currentUser != null)
                    {
                        SecurityHelper.CheckCurrentUser(facebookId, currentUser, Session, Response, ViewBag);
                    }
                    else
                    {
                        if (Session["PersonTryingToLogin"] != null)
                        {
                            currentUser = (Person)Session["PersonTryingToLogin"];
                            PersonDataAccessor.SavePersonFacebookDetails(currentUser, facebookId, birthdate);
                            SecurityHelper.CheckCurrentUser(facebookId, currentUser, Session, Response, ViewBag);
                        }
                        else
                        {
                            List<Person> people = PersonDataAccessor.FetchPersonFromName(me.name, me.first_name, me.last_name, me.email);
                            if (people.Count == 1)
                            {
                                //For now we assume its the right person
                                currentUser = people[0];
                                PersonDataAccessor.SavePersonFacebookDetails(currentUser, facebookId, birthdate);
                                SecurityHelper.CheckCurrentUser(facebookId, currentUser, Session, Response, ViewBag);
                            }
                            else
                            {
                                Session["FacebookId"] = facebookId;
                                return View("ValidateFacebookLogin");
                            }
                        }
                    }

                    if (currentUser != null)
                    {
                        Response.Cookies["AuthenticatedViaFacebook"].Value = "true";
                        Response.Cookies["AuthenticatedViaFacebook"].Expires = DateTime.Now.AddMonths(12);
                    }

                    // prevent open redirection attack by checking if the url is local.
                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        private void GetFacebookDetails(string accessToken, out dynamic me, out long facebookId, out DateTime? birthdate)
        {
            FacebookClient fbClient = new FacebookClient(accessToken);
            Session["FacebookClient"] = fbClient;
            me = fbClient.Get("me?fields=id,name,first_name,last_name,birthday,email");
            facebookId = Convert.ToInt64(me.id);
            string stringDate = me.birthday;
            birthdate = null;
            if (stringDate != string.Empty)
            {
                string[] date = me.birthday.Split('/');
                if (date.Length > 2)
                {
                    birthdate = new DateTime(int.Parse(date[2]), int.Parse(date[0]), int.Parse(date[1]));
                }
            }
        }

        private static string GetAccessToken(string code)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(ConfigurationManager.AppSettings["RedirectUrl"]);
            dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
            string accessToken = tokenResult.access_token;

            DateTime expiresOn = DateTime.MaxValue;

            if (tokenResult.ContainsKey("expires"))
            {
                DateTimeConvertor.FromUnixTime(tokenResult.expires);
            }
            return accessToken;
        }

        //
        // GET: /Account/LogOff/

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            var oAuthClient = new FacebookOAuthClient();
            oAuthClient.RedirectUri = new Uri(ConfigurationManager.AppSettings["LogoffUrl"]);
            var logoutUrl = oAuthClient.GetLogoutUrl();
            return Redirect(logoutUrl.AbsoluteUri);
        }

        public ActionResult ValidateFacebookLogin(string email, string password)
        {
            string message = string.Empty;
            Person currentPerson = PersonDataAccessor.Login(email, password, ref message);
            ViewBag.Message = message;
            if (currentPerson == null)
            {
                ViewBag.Message = "Invalid email or password";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session[SessionVariable.LoggedOnPerson] = currentPerson;
                long facebookId = (long)Session["FacebookId"];
                PersonDataAccessor.SavePersonFacebookDetails(currentPerson, facebookId, null);
                SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
                ChurchViewModel churchViewModel = ChurchDataAccessor.FetchChurch(currentPerson.Church.Name);
                Session[SessionVariable.Church] = churchViewModel;
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
