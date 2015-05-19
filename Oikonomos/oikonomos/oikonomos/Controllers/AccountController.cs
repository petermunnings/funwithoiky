using System;
using System.Web.Mvc;
using Facebook;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.common.Models;
using oikonomos.common;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.web.Helpers;

namespace oikonomos.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;

        public AccountController()
        {
            var permissionRepository = new PermissionRepository();
            _personRepository = new PersonRepository(permissionRepository, new ChurchRepository());
            _usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url)
                {
                    Query = null,
                    Fragment = null,
                    Path = Url.Action("FacebookCallback")
                };
                return uriBuilder.Uri;
            }
        }

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            Uri loginUrl;

            if (Request.Url.AbsoluteUri == "http://localhost:53624/Account/Facebook")
            {
                loginUrl = fb.GetLoginUrl(new
                {
                    client_id = "210504125641177",
                    client_secret = "d417ef6d72b9cdb430f938eb19c1b929",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",
                    scope = "user_birthday, email"
                });
            }
            else
            {
                loginUrl = fb.GetLoginUrl(new
                {
                    client_id = "118326544910444",
                    client_secret = "d2f09ce5b35cd32352d295be5df2ba39",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",
                    scope = "user_birthday, email"
                });
            }


            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            if (Request.Url.AbsoluteUri.StartsWith("http://localhost:53624/Account/Facebook"))
            {
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "210504125641177",
                    client_secret = "d417ef6d72b9cdb430f938eb19c1b929",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });

                var accessToken = result.access_token;

                fb.AccessToken = accessToken;
            }
            else
            {
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "118326544910444",
                    client_secret = "d2f09ce5b35cd32352d295be5df2ba39",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });

                var accessToken = result.access_token;

                fb.AccessToken = accessToken;
            }
            
            dynamic me;
            long facebookId;
            DateTime? birthdate;
            GetFacebookDetails(fb, out me, out facebookId, out birthdate);

            var currentUser = _personRepository.FetchPersonFromFacebookId(facebookId);
            if (currentUser != null)
            {
                SecurityHelper.CheckCurrentUser(facebookId, currentUser, Session, Response, ViewBag);
            }
            else
            {
                if (Session["PersonTryingToLogin"] != null)
                {
                    currentUser = (Person) Session["PersonTryingToLogin"];
                    PersonDataAccessor.SavePersonFacebookDetails(currentUser, facebookId, birthdate);
                    SecurityHelper.CheckCurrentUser(facebookId, currentUser, Session, Response, ViewBag);
                }
                else
                {
                    var people = _personRepository.FetchPersonFromName(me.name, me.first_name, me.last_name, me.email);
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

            return RedirectToAction("Index", "Home");
        }

        private void GetFacebookDetails(FacebookClient fbClient, out dynamic me, out long facebookId, out DateTime? birthdate)
        {
            Session["FacebookClient"] = fbClient;
            me = fbClient.Get("me?fields=id,name,first_name,last_name,birthday,email");
            facebookId = Convert.ToInt64(me.id);
            string stringDate = me.birthday;
            birthdate = null;
            if (!string.IsNullOrEmpty(stringDate))
            {
                string[] date = me.birthday.Split('/');
                if (date.Length > 2)
                {
                    birthdate = new DateTime(int.Parse(date[2]), int.Parse(date[0]), int.Parse(date[1]));
                }
            }
        }

        public ActionResult ValidateFacebookLogin(string email, string password)
        {
            string message = string.Empty;
            Person currentPerson = _usernamePasswordRepository.Login(email, password, out message);
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
