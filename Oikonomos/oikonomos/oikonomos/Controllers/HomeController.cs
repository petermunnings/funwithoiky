using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using oikonomos.common.DTOs;
using oikonomos.data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using oikonomos.common;
using oikonomos.data.DataAccessors;
using oikonomos.common.Models;
using System.Net.Mail;
using oikonomos.repositories;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.repositories.Messages;
using oikonomos.repositories.interfaces;
using oikonomos.services;
using oikonomos.services.interfaces;
using oikonomos.web.Helpers;
using oikonomos.web.Models.Groups;
using OpenPop.Pop3;
using Message = OpenPop.Mime.Message;


namespace oikonomos.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IPersonGroupRepository _personGroupRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IPersonService _personService;
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IPhotoServices _photoServices;

        private const string Hostname = "sark.aserv.co.za";
        private const int Port = 995;
        private const bool UseSsl = true;
        private const string Username = "reply@oikonomos.co.za";
        private const string Password = "MQaz3xXwzc";

        public HomeController()
        {
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();
            _personRepository = new PersonRepository(permissionRepository, churchRepository); var birthdayRepository = new BirthdayRepository();
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
            _personGroupRepository = new PersonGroupRepository(_personRepository);
            _usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);

            _photoRepository = new PhotoRepository();
            _photoServices = new PhotoServices();
            _personService = new PersonService(
                _personRepository,
                new PersonGroupRepository(_personRepository),
                permissionRepository,
                new PersonRoleRepository(),
                new PersonOptionalFieldRepository(),
                new RelationshipRepository(_personRepository),
                new ChurchMatcherRepository(),
                new GroupRepository(),
                new FamilyRepository(_photoRepository),
                emailService,
                new AddressRepository(),
                _photoRepository
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

        [HttpPost]
        public ContentResult UploadFiles()
        {
            try
            {
                if (Session["AttachmentList"] == null)
                    Session["AttachmentList"] = new List<UploadFilesResult>();
                var r = (List<UploadFilesResult>)Session["AttachmentList"];

                foreach (string file in Request.Files)
                {
                    var hpf = Request.Files[file];
                    if (hpf.ContentLength == 0)
                        continue;

                    var s = hpf.InputStream;
                    var attachmentContent = new byte[hpf.ContentLength + 1];
                    s.Read(attachmentContent, 0, hpf.ContentLength);

                    var newItem = r.FirstOrDefault(i => i.Name == hpf.FileName);
                    if (newItem == null)
                    {
                        newItem = new UploadFilesResult()
                        {
                            Name = hpf.FileName,
                            Length = hpf.ContentLength,
                            AttachmentContent = attachmentContent,
                            AttachmentContentType = hpf.ContentType,
                            Type = hpf.ContentType
                        };
                        r.Add(newItem);
                    }
                    else
                    {
                        newItem.Length = hpf.ContentLength;
                        newItem.Type = hpf.ContentType;
                    }
                }
                Session["AttachmentList"] = r;
                var response = r.Aggregate("{\"list\":[", (current, item) => current + ("{\"name\":\"" + item.Name + "\"},"));
                response = response.Substring(0, response.Length - 1);
                response += "]}";
                return Content(response, "application/json");
            }
            catch (Exception ex)
            {
                return Content("{\"errorMessage\":\"" + ex.Message + "\"}", "application/json");
            }
        }

        [HttpPost]
        public ContentResult UploadPhoto()
        {
            try
            {
                var acceptableImageTypes = new[] {"image/jpeg", "image/png", "image/bmp"};
                Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
                if (currentPerson == null)
                {
                    return Content("{\"errorMessage\":\"You have been logged out due to inactivity.  Please log in again\"}", "application/json");
                }
                
                foreach (string file in Request.Files)
                {
                    var hpf = Request.Files[file];
                    if (hpf.ContentLength == 0)
                        continue;
                    if (!acceptableImageTypes.Contains(hpf.ContentType)) return Content("{\"errorMessage\":\"The image must be a bmp, jpeg or png\"}", "application/json");
                    var s = hpf.InputStream;
                    var attachmentContent = new byte[hpf.ContentLength + 1];
                    s.Read(attachmentContent, 0, hpf.ContentLength);
                    var image = _photoServices.ByteArrayToImage(attachmentContent);
                    var scaleFactor = image.Width/600d;
                    if (scaleFactor > 1d)
                    {
                        var newSize = new Size((int) (image.Width/scaleFactor), (int) (image.Height/scaleFactor));
                        var newImage = _photoServices.ResizeImage(_photoServices.ByteArrayToImage(attachmentContent),newSize);
                        _photoRepository.SavePhoto(currentPerson.PersonId, _photoServices.ImageToByteArray(newImage, hpf.ContentType),hpf.ContentType, hpf.FileName);
                    }
                    else
                    {
                        _photoRepository.SavePhoto(currentPerson.PersonId, attachmentContent, hpf.ContentType, hpf.FileName);
                    }
                    
                    var response = "{\"filename\":\"" + hpf.FileName + "\"}";
                    return Content(response, "application/json");
                }
                return Content("{\"errorMessage\":\"An unkown error occured trying to upload the file\"}", "application/json");
            }
            catch (Exception ex)
            {
                return Content("{\"errorMessage\":\"" + ex.Message + "\"}", "application/json");
            }
        }
        
        [HttpPost]
        public ContentResult RemoveAttachment(string name)
        {
            if (Session["AttachmentList"] == null)
                Session["AttachmentList"] = new List<UploadFilesResult>();
            var r = (List<UploadFilesResult>)Session["AttachmentList"];


            var itemToRemove = r.FirstOrDefault(i => i.Name == name);
            if (itemToRemove != null)
            {
                r.Remove(itemToRemove);
            }
            Session["AttachmentList"] = r;
            var response = r.Aggregate("{\"list\":[", (current, item) => current + ("{\"name\":\"" + item.Name + "\"},"));

            if (r.Count > 0) 
                response = response.Substring(0, response.Length - 1);
             
            response += "]}";
            return Content(response, "application/json");
        }

        [HttpPost]
        public ContentResult RemoveAllAttachments()
        {
            Session["AttachmentList"] = new List<UploadFilesResult>();
            return Content(string.Empty, "application/json");
        }

        [HttpGet]
        public IEnumerable<string> GetEmails()
        {
            var returnMessages = new List<string>();
            using (var pop3Client = new Pop3Client())
            {
                var messages = GetNewMessages(pop3Client);
                var messageCount = messages.Count();
                if (messageCount > 0)
                {
                    IMessageRepository messageRepository = new MessageRepository();
                    IMessageRecepientRepository messageRecepientRepository = new MessageRecepientRepository();
                    IMessageAttachmentRepository messageAttachmentRepository = new MessageAttachmentRepository();
                    IPermissionRepository permissionRepository = new PermissionRepository();
                    IChurchRepository churchRepository = new ChurchRepository();
                    IPersonRepository personRepository = new PersonRepository(permissionRepository, churchRepository);

                    IEmailSender emailSender = new EmailSender(messageRepository, messageRecepientRepository, messageAttachmentRepository, personRepository);

                    for (var count = 0; count < messageCount; count++)
                    {
                        var mm = messages[count].ToMailMessage();
                        var regex = new Regex(@"##([0-9]*)##");
                        var matches = regex.Matches(mm.Body);
                        if (matches.Count > 0 && matches[0].Groups.Count > 1)
                        {
                            try
                            {
                                int messageId;
                                if (int.TryParse(matches[0].Groups[1].Value, out messageId))
                                {
                                    var originalSender = messageRepository.GetSender(messageId);
                                    if (originalSender != null)
                                    {
                                        var originalReceiver = personRepository.FetchPersonIdsFromEmailAddress(mm.From.Address, originalSender.ChurchId);
                                        var fromPersonId = originalSender.PersonId;

                                        if (originalReceiver.Any())
                                        {
                                            fromPersonId = originalReceiver.First();
                                        }
                                        returnMessages.Add(string.Format("Forwarding email on to {0}", originalSender.Email));
                                        emailSender.SendEmail(mm.Subject, mm.Body, Username, originalSender.Email, Username, Password, fromPersonId, originalSender.ChurchId, mm.Attachments);
                                    }
                                    pop3Client.DeleteMessage(count + 1);
                                }
                            }
                            catch (Exception errSending)
                            {
                                returnMessages.Add(errSending.Message);
                                emailSender.SendExceptionEmailAsync(errSending);
                            }
                        }
                    }
                }
            }
            return returnMessages;
        }

        private static List<Message> GetNewMessages(Pop3Client pop3Client)
        {
            var allMessages = new List<Message>();

            pop3Client.Connect(Hostname, Port, UseSsl);
            pop3Client.Authenticate(Username, Password);
            var messageCount = pop3Client.GetMessageCount();

            for (var i = messageCount; i > 0; i--)
            {
                allMessages.Add(pop3Client.GetMessage(i));
            }
            return allMessages;
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

            var optionalFields = SettingsDataAccessor.FetchChurchOptionalFields(currentPerson.ChurchId);
            foreach (var ct in optionalFields)
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
                    case OptionalFields.ShowOverseeingElder:
                    {
                        viewModel.DisplayOverseeingElder = ct.Display ? "tableRow" : "displayNone";
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
                                return RedirectToAction("Facebook", "Account");
                            }
                        }
                    }
                    return View();
                }
                
                if (currentPerson.HasPermission(Permissions.Login))
                {
                    var fullName = currentPerson.Firstname + " " + currentPerson.Family.FamilyName;
                    ViewBag.Message = "Welcome " + fullName + " from " + currentPerson.Church.Name;
                    var eventDisplayModel = _eventService.FetchEventsToDisplay(currentPerson);
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
                    EventDisplayModel eventDisplayModel = _eventService.FetchEventsToDisplay(currentPerson);
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

            EventDisplayModel eventDisplayModel = _eventService.FetchEventsToDisplay(currentPerson);
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
                    case OptionalFields.MaritalStatus:
                        {
                            ViewBag.DisplayMaritalStatus = ct.Display ? "tableRow" : "displayNone";
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

            var viewModel = new AdminReportsViewModel
            {
                RoleId = PermissionDataAccessor.FetchDefaultRoleId(currentPerson),
                SecurityRoles = PermissionDataAccessor.FetchRoles(currentPerson),
                Months = new List<MonthViewModel>
                {
                    new MonthViewModel {MonthId = 1, Name = "Jan"},
                    new MonthViewModel {MonthId = 2, Name = "Feb"},
                    new MonthViewModel {MonthId = 3, Name = "Mar"},
                    new MonthViewModel {MonthId = 4, Name = "Apr"},
                    new MonthViewModel {MonthId = 5, Name = "May"},
                    new MonthViewModel {MonthId = 6, Name = "Jun"},
                    new MonthViewModel {MonthId = 7, Name = "Jul"},
                    new MonthViewModel {MonthId = 8, Name = "Aug"},
                    new MonthViewModel {MonthId = 9, Name = "Sep"},
                    new MonthViewModel {MonthId = 10, Name = "Oct"},
                    new MonthViewModel {MonthId = 11, Name = "Nov"},
                    new MonthViewModel {MonthId = 12, Name = "Dec"}
                },
                MonthId = DateTime.Today.Month
            };


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
                    case OptionalFields.MaritalStatus:
                        {
                            ViewBag.DisplayMaritalStatus = ct.Display ? "tableRow" : "displayNone";
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
