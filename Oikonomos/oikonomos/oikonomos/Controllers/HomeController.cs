using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using oikonomos.data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using oikonomos.common;
using oikonomos.data.DataAccessors;
using oikonomos.common.Models;
using System.Net.Mail;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.services;
using oikonomos.services.interfaces;
using oikonomos.web.Helpers;
using oikonomos.web.Models.Groups;


namespace oikonomos.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IPersonGroupRepository _personGroupRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IPersonService _personService;
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;

        public HomeController()
        {
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();
            _personRepository = new PersonRepository(permissionRepository, churchRepository);
            _eventService = new EventService(new EventRepository());
            _personGroupRepository = new PersonGroupRepository(_personRepository);
            _usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var groupRepository = new GroupRepository();
            var emailSender = new EmailSender(new EmailLogger(new MessageRepository(), _personRepository));
            var emailContentService = new EmailContentService(new EmailContentRepository());
            var emailService = new EmailService(
                _usernamePasswordRepository,
                _personRepository,
                groupRepository,
                emailSender,
                emailContentService
                );

            _personService = new PersonService(
                _personRepository,
                new PersonGroupRepository(_personRepository),
                permissionRepository,
                new PersonRoleRepository(),
                new PersonOptionalFieldRepository(),
                new RelationshipRepository(_personRepository),
                new ChurchMatcherRepository(),
                new GroupRepository(),
                new FamilyRepository(),
                emailService,
                new AddressRepository()
                );
            
        }

        public ActionResult Settings()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }

            var settings = new SettingsViewModel();
            ViewBag.GroupId = 0;
            if(currentPerson.HasPermission(common.Permissions.EditSettings))
            {
                settings = SettingsDataAccessor.FetchSettings(currentPerson);
                if (settings.GroupSettings != null)
                {
                    ViewBag.GroupId = settings.GroupSettings.GroupId;
                    ViewBag.GroupName = settings.GroupSettings.GroupName;
                }
            }
            return View(settings);
        }

        public ActionResult SysAdmin()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }

            if (currentPerson.HasPermission(Permissions.SystemAdministrator))
            {
                return View(SettingsDataAccessor.FetchSysAdminViewModel(currentPerson)); 
            }
            else
            {
                return View("Index", GetEventDisplayModel(currentPerson));
            }
        }

        public ActionResult Groups(int? groupId)
        {
            var viewModel = new GroupViewModel();
            
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }

            viewModel.GroupClassifications = SettingsDataAccessor.FetchGroupClassifications(currentPerson);
            viewModel.GroupClassifications.Insert(0, new GroupClassificationViewModel() { GroupClassificationId = 0, GroupClassification = "Select..." });
            viewModel.SelectedGroupClassificationId = 0;
            viewModel.Suburbs = SettingsDataAccessor.FetchSuburbs(currentPerson);
            viewModel.StandardComments = SettingsDataAccessor.FetchStandardComments(currentPerson);

            List<OptionalFieldViewModel> optionalFields = SettingsDataAccessor.FetchChurchOptionalFields(currentPerson.ChurchId);
            foreach (OptionalFieldViewModel ct in optionalFields)
            {
                switch ((OptionalFields)ct.OptionalFieldId)
                {
                    case OptionalFields.HomeGroupClassification:
                        {
                            viewModel.DisplayGroupClassification = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.SuburbLookup:
                        {
                            viewModel.DisplaySuburbLookup = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.GroupAdministratorsCanAddMembers:
                        {
                            viewModel.ShowRoles = ct.Display;
                            break;
                        }
                }
            }

            viewModel.SecurityRoles = PermissionDataAccessor.FetchRoles(currentPerson);
            viewModel.RoleId = viewModel.SecurityRoles.First().RoleId;
            viewModel.GroupEvents = _eventService.GetListEventsForGroup(currentPerson.ChurchId);

            if (currentPerson.HasPermission(Permissions.EditAllGroups))
            {
                viewModel.GroupName = "Loading...";
                viewModel.ShowList = true;
                viewModel.ShowRoles = true;
                return View(viewModel);
            }

            if (currentPerson.HasPermission(Permissions.EditOwnGroups))
            {
                //If this person administrates more then one homegroup
                //then a list of all of those will be shown
                bool displayList;
                viewModel.GroupName = GroupDataAccessor.FetchHomeGroupName(currentPerson, out displayList, ref groupId);
                if (viewModel.GroupName == string.Empty)
                {
                    ViewBag.Message = "You are not currently administrating any groups";
                    return View("Login");
                }

                if (displayList)
                {
                    viewModel.ShowList = true;
                }
                else
                {
                    viewModel.ShowList = false;
                }

                viewModel.GroupId = groupId.HasValue ? groupId.Value : 0;

                return View(viewModel);
            }

            

            return View("Index");
        }

        public ActionResult Login(string id, string email, string password)
        { 
            string message = string.Empty;
            if (email == null)
            {
                Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
                if (currentPerson == null)
                {
                    if (id != null)
                    {
                        Person person = _personRepository.FetchPersonFromPublicId(id);
                        if (person != null)
                        {
                            ViewBag.Message = "Welcome " + person.Firstname + " please login";
                            Session[SessionVariable.Church] = ChurchDataAccessor.FetchChurch(person.Church.Name);
                            Response.Cookies["PersonId"].Value = person.PersonId.ToString();
                            Response.Cookies["PersonId"].Expires = DateTime.Now.AddYears(1);
                            ViewBag.PublicId = id;
                        }
                        else
                        {
                            ViewBag.Message = "Please login below";
                        }
                    }
                    else
                    {
                        if (Request.Cookies["AuthenticatedViaFacebook"] != null)
                        {
                            var val = Server.HtmlEncode(Request.Cookies["AuthenticatedViaFacebook"].Value);
                            if (val == "true")
                            {
                                return RedirectToAction("Login", "Account");
                            }
                        }
                    }
                    return View();
                }
                
                if (currentPerson.HasPermission(Permissions.Login))
                {
                    var fullName = currentPerson.Firstname + " " + currentPerson.Family.FamilyName;
                    ViewBag.Message = "Welcome " + fullName + " from " + currentPerson.Church.Name;
                    var eventDisplayModel = EventDataAccessor.FetchEventsToDisplay(currentPerson);
                    return View("Index", eventDisplayModel);
                }
                
                ViewBag.Message = "Invalid email or password";
                return View();
            }
            else
            {
                Person currentPerson = _usernamePasswordRepository.Login(email, password, out message);
                ViewBag.Message = message;
                if (currentPerson == null)
                {
                    ViewBag.Message = "Invalid email or password";
                    return View("Login");
                }
                else
                {
                    Session[SessionVariable.LoggedOnPerson] = currentPerson;
                    SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
                    ChurchViewModel churchViewModel = ChurchDataAccessor.FetchChurch(currentPerson.Church.Name);
                    Session[SessionVariable.Church] = churchViewModel;
                    ViewBag.Message = "Welcome " + currentPerson.Firstname + " " + currentPerson.Family.FamilyName + " from " + churchViewModel.ChurchName;
                    EventDisplayModel eventDisplayModel = EventDataAccessor.FetchEventsToDisplay(currentPerson);
                    return View("Index", eventDisplayModel);
                }
            }
        }

        public ActionResult Index()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }
            EventDisplayModel eventDisplayModel = GetEventDisplayModel(currentPerson);
            return View("Index", eventDisplayModel);
        }

        private EventDisplayModel GetEventDisplayModel(Person currentPerson)
        {
            var churchViewModel = (ChurchViewModel)Session[SessionVariable.Church];
            ViewBag.Message = "Welcome " + currentPerson.Firstname + " " + currentPerson.Family.FamilyName + " from " + churchViewModel.ChurchName;

            EventDisplayModel eventDisplayModel = EventDataAccessor.FetchEventsToDisplay(currentPerson);
            eventDisplayModel.SelectedChurchId = currentPerson.ChurchId;
            return eventDisplayModel;
        }

        public ActionResult Person(int? personId, int? GroupId)
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }

            ViewBag.ChurchAdminDisplayRoles = false;
            ViewBag.GroupAdminDisplayRoles = false;
            if (currentPerson.HasPermission(Permissions.EditChurchPersonalDetails))
            {
                ViewBag.ChurchAdminDisplayRoles = true;
            }

            PersonViewModel personViewModel;
            if (personId.HasValue)
            {
                personViewModel = _personService.FetchPersonViewModel(personId.Value, currentPerson);
                if (personViewModel == null)
                {
                    personViewModel = _personService.FetchPersonViewModel(currentPerson.PersonId, currentPerson);
                }
                else if (GroupId.HasValue)
                {
                    personViewModel.GroupId = GroupId.Value;
                }
            }
            else
            {
                personViewModel = _personService.FetchPersonViewModel(currentPerson.PersonId, currentPerson);
            }

            ViewBag.CanChangeRole = false;
            foreach (var role in personViewModel.SecurityRoles.Where(role => role.RoleId == personViewModel.RoleId))
            {
                ViewBag.CanChangeRole = true;
                break;
            }

            //Fetch Groups
            personViewModel.PersonGroups = _personGroupRepository.GetPersonGroupViewModels(personViewModel.PersonId, currentPerson);
            var primaryGroup = !personViewModel.PersonGroups.Any() ? null : personViewModel.PersonGroups.FirstOrDefault(pg => pg.IsPrimaryGroup);
            personViewModel.GroupId = primaryGroup == null ? 0 : primaryGroup.GroupId;
            personViewModel.GroupName = primaryGroup == null ? "None" : primaryGroup.GroupName;
            
            var optionalFields = SettingsDataAccessor.FetchChurchOptionalFields(currentPerson.ChurchId);
            ViewBag.DisplayFacebook = false;
            foreach (var ct in optionalFields)
            {
                switch ((OptionalFields)ct.OptionalFieldId)
                {
                    case OptionalFields.CellPhone:
                        {
                            ViewBag.DisplayCellPhone = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.WorkPhone:
                        {
                            ViewBag.DisplayWorkPhone = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Skype:
                        {
                            ViewBag.DisplaySkype = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Twitter:
                        {
                            ViewBag.DisplayTwitter = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Facebook:
                        {
                            ViewBag.DisplayFacebook = ct.Display;
                            break;
                        }
                    case OptionalFields.Occupation:
                        {
                            ViewBag.DisplayOccupation = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Gender:
                        {
                            ViewBag.DisplayGender = ct.Display;
                            break;
                        }
                    case OptionalFields.HeardAbout:
                        {
                            ViewBag.DisplayHeardAbout = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                }
            }

            var siteLookups = new List<string> {"Select site..."};
            var sites = ChurchDataAccessor.FetchSites(currentPerson);
            if (sites.Count > 0)
            {
                ViewBag.DisplaySites = "tableRow";
                siteLookups.AddRange(sites.Select(site => site.SiteName));
            }
            else
            {
                ViewBag.DisplaySites = "displayNone";
            }

            ViewBag.Sites = siteLookups;

            return View(personViewModel);
        }

        public ActionResult Children()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            return currentPerson == null ? View("Login") : View();
        }

        public ActionResult Sites()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            return currentPerson == null ? View("Login") : View();
        }

        public ActionResult ReportsMap()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }
            GetReportViewBagValues(currentPerson);
            return View();
        }

        public ActionResult ReportsAdmin()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }

            if (!currentPerson.HasPermission(Permissions.ViewAdminReports))
            {
                return View("ReportGrid");
            }

            AdminReportsViewModel viewModel = new AdminReportsViewModel();
            viewModel.RoleId = PermissionDataAccessor.FetchDefaultRoleId(currentPerson);
            viewModel.SecurityRoles = PermissionDataAccessor.FetchRoles(currentPerson);


            return View(viewModel);
        }

        public ActionResult ReportGrid()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }

            GetReportViewBagValues(currentPerson);

            return View();

        }

        public ActionResult Help()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return View("Login");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session[SessionVariable.LoggedOnPerson] = null;
            ViewBag.Message = "Please login below";
            return View("Login");
        }

        [ValidateInput(false)]
        public ActionResult RunSql(TableGenerationModel tgm)
        {
            tgm.CommandTypeOptions = new List<SelectListItem>();
            SelectListItem select = new SelectListItem();
            select.Value = "Select";
            select.Text = "Select";

            SelectListItem execute = new SelectListItem();
            execute.Value = "Execute";
            execute.Text = "Execute";

            SelectListItem email = new SelectListItem();
            email.Value = "Email";
            email.Text = "Email";

            tgm.CommandTypeOptions.Add(select);
            tgm.CommandTypeOptions.Add(execute);
            tgm.CommandTypeOptions.Add(email);

            tgm.Message = "Welcome";

            if (tgm.Sql == null)
            {
                return View(tgm);
            }

            if (Session["IsAuthenticated"] != null)
            {
                tgm.IsAuthenticated = (bool)Session["IsAuthenticated"];
            }

            try
            {
                if (tgm.IsAuthenticated || (tgm.UserName == "peter" && tgm.Password == "sandton2000"))
                {
                    Session["IsAuthenticated"] = tgm.IsAuthenticated = true;
                    if (tgm.CommandType == "Email")
                    {
                        try
                        {
                            using (MailMessage message = new MailMessage())
                            {
                                message.Subject = "Test Email";
                                message.Body = tgm.Sql;
                                message.To.Add("peter@sandtoncitychurch.org.za");
                                message.From = new MailAddress("support@oikonomos.co.za");
                                

                                using (SmtpClient client = new SmtpClient("mail.oikonomos.co.za"))
                                {
                                    client.Credentials = new System.Net.NetworkCredential("support@oikonomos.co.za", "sandton2000");

                                    client.Send(message);
                                }
                            }
                        }

                        catch (SmtpException mailEx)
                        {
                            // handle exception here
                            tgm.Message = mailEx.Message;
                        }
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                        {
                            con.Open();
                            string sql = tgm.Sql;

                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                if (tgm.CommandType == "Execute")
                                {
                                    cmd.ExecuteNonQuery().ToString();
                                    tgm.Message = "success";
                                }
                                else
                                {
                                    //Populate a table with the select
                                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                    {
                                        tgm.Results = new DataTable();
                                        da.Fill(tgm.Results);
                                    }

                                }
                            }
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tgm.Message = ex.Message;
            }

            return View(tgm);

        }

        #region Private Methods
        private void GetReportViewBagValues(Person currentPerson)
        {
            ViewBag.Title = currentPerson.HasPermission(Permissions.ViewChurchContactDetails) ? "List of People" : "List of People";
            
            var optionalFields = SettingsDataAccessor.FetchChurchOptionalFields(currentPerson.ChurchId);
            ViewBag.DisplayFacebook = false;
            foreach (var ct in optionalFields)
            {
                switch ((OptionalFields)ct.OptionalFieldId)
                {
                    case OptionalFields.CellPhone:
                        {
                            ViewBag.DisplayCellPhone = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.WorkPhone:
                        {
                            ViewBag.DisplayWorkPhone = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Skype:
                        {
                            ViewBag.DisplaySkype = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Twitter:
                        {
                            ViewBag.DisplayTwitter = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Occupation:
                        {
                            ViewBag.DisplayOccupation = ct.Display ? "tableRow" : "displayNone";
                            break;
                        }
                    case OptionalFields.Facebook:
                        {
                            ViewBag.DisplayFacebook = ct.Display;
                            break;
                        }
                    case OptionalFields.ShowWholeChurch:
                        {
                            if (!currentPerson.HasPermission(Permissions.ViewChurchContactDetails))
                            {
                                ViewBag.DislayWholeChurch = ct.Display;
                            }
                            else
                            {
                                ViewBag.DislayWholeChurch = true;
                            }
                            break;
                        }
                }
            }
        }

        #endregion Private Methods


    }
}
