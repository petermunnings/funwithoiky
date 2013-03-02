using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using oikonomos.common.DTOs;
using oikonomos.data.DataAccessors;
using oikonomos.data;
using oikonomos.common.Models;
using Lib.Web.Mvc.JQuery.JqGrid;
using System.Threading.Tasks;
using Facebook;
using oikonomos.common;
using System;
using oikonomos.data.Services;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.web.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IGroupEventRepository _groupEventRepository;
        private readonly IPasswordService _passwordService;
        private readonly IFamilyRepository _familyRepository;
        private readonly ISystemAdministratorService _systemAdministratorService;
        private readonly IUsernamePasswordRepository _usernamePasswordRepository;
        private readonly IPersonGroupService _personGroupService;
        private readonly IPersonGroupRepository _personGroupRepository;
        private readonly IEmailService _emailService;

        public AjaxController()
        {
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();            
            var personRepository = new PersonRepository(permissionRepository, churchRepository);
            _familyRepository = new FamilyRepository();
            _usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var personGroupRepository = new PersonGroupRepository(personRepository);
            _personGroupRepository = personGroupRepository;
            var groupRepository = new GroupRepository();
            var emailSender = new EmailSender(new EmailLogger(new MessageRepository(), personRepository));
            var emailContentService = new EmailContentService(new EmailContentRepository());
            _emailService = new EmailService(
                _usernamePasswordRepository,
                personRepository,
                groupRepository,
                emailSender,
                emailContentService
                );

            _passwordService = new PasswordService(personRepository, churchRepository, _usernamePasswordRepository, _emailService);

            _personService = new PersonService(
                personRepository,
                _personGroupRepository,
                permissionRepository,
                new PersonRoleRepository(),
                new PersonOptionalFieldRepository(),
                new RelationshipRepository(personRepository),
                new ChurchMatcherRepository(), 
                groupRepository,
                _familyRepository,
                _emailService,
                new AddressRepository()
                );

            _groupEventRepository = new GroupEventRepository(personRepository);
            _systemAdministratorService  = new SystemAdministratorService(churchRepository, permissionRepository);
            _personGroupService = new PersonGroupService(_personGroupRepository);
            
        }

        public JsonResult InitializeChurchSettingsViewModel()
        {
            return Json(new ChurchSettingsViewModel { UITheme = "start", SystemName = "Oiky", AddressType = "street_address", BulkSmsUsername = string.Empty, BulkSmsPassword = string.Empty }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult PersonAutoComplete(string term)
        {
            var data = new AutoCompleteViewModel[0];
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                data = PersonDataAccessor.FetchPersonAutoComplete(term, currentPerson, Request.UrlReferrer.PathAndQuery == "/Home/Groups");
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FamilyAutoComplete(string term)
        {
            var data = new AutoCompleteViewModel[0];
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                data = PersonDataAccessor.FetchFamilyAutoComplete(term, currentPerson.ChurchId);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPassword(string emailAddress)
        {
            string message;

            try
            {
                message = Utils.ValidEmailAddress(emailAddress) ? _passwordService.ResetPassword(emailAddress) : "Invalid email address";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var response = new { Message = message };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveComments(int personId, IEnumerable<string> comments)
        {
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                ICommentService commentService = new CommentService(new CommentRepository());
                var newComments = comments.Select(comment => new CommentDto {AboutPersonId = personId, Comment = comment, CommentDate = DateTime.Now}).ToList();
                commentService.SaveComments((Person)Session[SessionVariable.LoggedOnPerson], newComments);
            }

            var response = new { SessionTimeOut = sessionTimedOut };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SavePersonEvents(PersonEventViewModel personEvents)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                EventDataAccessor.SavePersonEvents(personEvents, (Person)Session[SessionVariable.LoggedOnPerson]);
            }

            var response = new { SessionTimeOut = sessionTimedOut };
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        
        public JsonResult SavePerson(PersonViewModel person)
        {
            bool sessionTimedOut = false;
            int personId = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                personId = _personService.Save(person, currentPerson);
            }

            var response = new { PersonId = personId, SessionTimeOut = sessionTimedOut };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult DeletePerson(int personId)
        {
            bool sessionTimedOut = false;
            string message=string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.PersonId == personId)
                {
                    message = "You cannot delete yourself";
                }
                else
                {
                    //TODO Check for User Roles
                    PersonDataAccessor.DeletePerson(personId, currentPerson);
                }
            }

            var response = new { SessionTimeOut = sessionTimedOut,
                                    Message = message};
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SendEmailAndPassword(int personId)
        {
            var sessionTimedOut = false;
            var emailSent = false;
            var message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                emailSent=_emailService.SendEmailAndPassword(currentPerson, personId, out message);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                EmailSent = emailSent
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchChurchList(JqGridRequest request)
        {
            var jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PersonDataAccessor.FetchChurchListJQGrid(currentPerson, request);
            }

            return Json(jqGridData); 
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchEventListForPerson(JqGridRequest request, int personId)
        {
            var jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = EventDataAccessor.FetchEventListJQGrid(currentPerson, personId, request);
            }

            return Json(jqGridData);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchCommentListForPerson(JqGridRequest request, int personId)
        {
            var jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                ICommentService commentService = new CommentService(new CommentRepository());
                IGridFormatter gridFormatter = new GridFormatter();
                var comments = commentService.GetListOfComments(currentPerson, personId);
                jqGridData = gridFormatter.FormatCommentsForGrid(comments, request);
            }

            return Json(jqGridData);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupsPersonIsIn(JqGridRequest request, int personId)
        {
            var jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = _personGroupService.FetchGroupsPersonIsInJQGrid(currentPerson, personId, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupsPersonIsNotIn(JqGridRequest request, int personId)
        {
            var jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = _personGroupService.FetchGroupsPersonIsNotInJQGrid(currentPerson, personId, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupsForPerson(JqGridRequest request, int personId)
        {
            var jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PersonDataAccessor.FetchGroupsForPersonJQGrid(currentPerson, personId, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchEventList(JqGridRequest request, DateTime fromDate, DateTime toDate)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = EventDataAccessor.FetchEventListJQGrid(currentPerson, fromDate, toDate, request);
            }

            return Json(jqGridData);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeople(JqGridRequest request, int roleId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PersonDataAccessor.FetchPeopleJQGrid(currentPerson, request, roleId);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeopleInGroupForAttendance(int groupId)
        {
            IEnumerable<PersonViewModel> people = new List<PersonViewModel>();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                people = GroupDataAccessor.FetchPeopleInGroup(currentPerson, groupId);
            }

            return Json(new { People = people });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeopleInGroup(JqGridRequest request, int groupId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = GroupDataAccessor.FetchPeopleInGroupJQGrid(currentPerson, request, groupId);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchSites(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = ChurchDataAccessor.FetchSitesJQGrid(currentPerson, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPermissionsLinked(JqGridRequest request, int roleId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PermissionDataAccessor.FetchPermissionsForRoleJQGrid(currentPerson, request, roleId);
            }

            return Json(jqGridData);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPermissionsUnLinked(JqGridRequest request, int roleId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PermissionDataAccessor.FetchPermissionsNotInRoleJQGrid(currentPerson, request, roleId);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ChangeChurchTo(int churchId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                SelectNewChurch(churchId, currentPerson);
            }

            return Json(new { message = "Success" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SelectNewChurch(int churchId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.Login))
                {
                    SelectNewChurch(churchId, currentPerson);
                }
            }

            return Json(new { message = "Success" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupList(JqGridRequest request)
        {
            try
            {
                var jqGridData = new JqGridData();
                if (Session[SessionVariable.LoggedOnPerson] != null)
                {
                    var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    if (currentPerson.HasPermission(Permissions.EditAllGroups) || currentPerson.HasPermission(Permissions.EditOwnGroups))
                    {
                        jqGridData = GroupDataAccessor.FetchHomeGroupsJQGrid(currentPerson, request);
                    }
                }

                return Json(jqGridData);
            }
            catch (Exception ex)
            {
                try
                {
                    _emailService.SendExceptionEmail(ex);
                }
                catch { }
                return Json(null);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchHomeGroupList(JqGridRequest request)
        {
            try
            {
                JqGridData jqGridData = new JqGridData();
                var message = string.Empty;
                if (Session[SessionVariable.LoggedOnPerson] != null)
                {
                    var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    message = String.Format("Current Person:{0}", currentPerson.Fullname);
                    
                }

                message += " is trying to access FetchHomeGroupList";

                _emailService.SendSystemEmail("Javascript file not updated", message);

                return Json(jqGridData);
            }
            catch (Exception ex)
            {
                try
                {
                    _emailService.SendExceptionEmail(ex);
                }
                catch { }
                return Json(null);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeopleNotInHomeGroup(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(common.Permissions.ViewPeopleNotInAnyGroup))
                {
                    jqGridData = GroupDataAccessor.FetchPeopleNotInAHomeGroupJQGrid(currentPerson, request);
                }
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddPermissionsToRole(int roleId, List<int> permissionIds)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                PermissionDataAccessor.AddPermissionsToRole(currentPerson, roleId, permissionIds);
            }

            return Json(new { Message = "success" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RemovePermissionsFromRole(int roleId, List<int> permissionIds)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                PermissionDataAccessor.RemovePermissionsFromRole(currentPerson, roleId, permissionIds);
            }

            return Json(new { Message = "success" });
        }

        public JsonResult FetchSite(int siteId)
        {
            SiteSettingsViewModel site = new SiteSettingsViewModel();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                site = ChurchDataAccessor.FetchSite(currentPerson, siteId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Site = site
            };
            return Json(response); 
        }

        public JsonResult DeleteSite(int siteId)
        {
            bool sessionTimedOut = false;
            string message=string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                message = ChurchDataAccessor.DeleteSite(currentPerson, siteId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response);
        }

        public JsonResult FetchOptionalFields()
        {
            List<OptionalFieldViewModel> optionalFields = new List<OptionalFieldViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                optionalFields = SettingsDataAccessor.FetchChurchOptionalFields(currentPerson.ChurchId);
            }

            var response = new { SessionTimeOut = sessionTimedOut,
                                 OptionalFields = optionalFields};
            return Json(response, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult FetchGroupClassifications()
        {
            List<GroupClassificationViewModel> groupClassifications = new List<GroupClassificationViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupClassifications = SettingsDataAccessor.FetchGroupClassifications(currentPerson);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                GroupClassifications = groupClassifications
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddGroupClassification(string groupClassification)
        {
            List<GroupClassificationViewModel> groupClassifications = new List<GroupClassificationViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupClassifications = SettingsDataAccessor.AddGroupClassification(currentPerson, groupClassification);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                GroupClassifications = groupClassifications
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteGroupClassification(int groupClassificationId)
        {
            List<GroupClassificationViewModel> groupClassifications = new List<GroupClassificationViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupClassifications = SettingsDataAccessor.DeleteGroupClassification(currentPerson, groupClassificationId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                GroupClassifications = groupClassifications
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchSuburbs()
        {
            List<SuburbViewModel> suburbs = new List<SuburbViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                suburbs = SettingsDataAccessor.FetchSuburbs(currentPerson);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Suburbs = suburbs
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSuburb(string suburb)
        {
            List<SuburbViewModel> suburbs = new List<SuburbViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                suburbs = SettingsDataAccessor.AddSuburb(currentPerson, suburb);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Suburbs = suburbs
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteSuburb(int suburbId)
        {
            List<SuburbViewModel> suburbs = new List<SuburbViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                suburbs = SettingsDataAccessor.DeleteSuburb(currentPerson, suburbId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Suburbs = suburbs
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchStandardComments()
        {
            var standardComments = new List<StandardCommentViewModel>();
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                standardComments = SettingsDataAccessor.FetchStandardComments(currentPerson);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                StandardComments = standardComments
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddStandardComment(string standardComment)
        {
            try
            {
                var standardComments = new List<StandardCommentViewModel>();
                var sessionTimedOut = false;
                if (Session[SessionVariable.LoggedOnPerson] == null)
                {
                    sessionTimedOut = true;
                }
                else
                {
                    var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    standardComments = SettingsDataAccessor.AddStandardComment(currentPerson, standardComment);
                }

                var response = new
                {
                    SessionTimeOut = sessionTimedOut,
                    StandardComments = standardComments
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _emailService.SendExceptionEmail(ex);
                var comments = new List<StandardCommentViewModel>
                    {
                        new StandardCommentViewModel {StandardCommentId = 0, StandardComment = "Error saving comment"}
                    };
                var response = new
                {
                    SessionTimeOut = false,
                    StandardComments = comments
                };
                return Json(response); 

            }
        }

        public JsonResult DeleteStandardComment(int standardCommentId)
        {
            var standardComments = new List<StandardCommentViewModel>();
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                standardComments = SettingsDataAccessor.DeleteStandardComment(currentPerson, standardCommentId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                StandardComments = standardComments
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPersonToFamily(int familyId, int personId)
        {
            var familyMembers = new List<FamilyMemberViewModel>();
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (personId > 0 && familyId > 0)
                {
                    familyMembers = _familyRepository.AddPersonToFamily(familyId, personId).ToList();
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                FamilyMembers = familyMembers
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult AddPersonToGroup(int groupId, int personId)
        {
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if(personId>0 && groupId>0)
                {
                    _personGroupRepository.AddPersonToGroup(currentPerson, personId, groupId);
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult RemovePersonFromGroup(int groupId, int personId)
        {
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (personId > 0 && groupId > 0)
                {
                    _personGroupRepository.RemovePersonFromGroup(currentPerson, personId, groupId);
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SetPrimaryGroup(int groupId, int personId)
        {
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (personId > 0 && groupId > 0)
                {
                    _personGroupRepository.SavePrimaryGroup(personId, groupId, currentPerson);
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public void SetHomeGroupLeader(int groupId, int leaderId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (leaderId > 0 && groupId > 0)
                {
                    GroupDataAccessor.SetHomeGroupLeader(currentPerson, groupId, leaderId);
                }
            }
        }

        public void SetHomeGroupAdministrator(int groupId, int administratorId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (administratorId > 0 && groupId > 0)
                {
                    GroupDataAccessor.SetHomeGroupAdministrator(currentPerson, groupId, administratorId);
                }
            }
        }

        public JsonResult DeleteHomeGroup(int groupId)
        {
            string message=string.Empty;
            bool success = false;
            List<HomeGroupsViewModel> homeGroups = new List<HomeGroupsViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(common.Permissions.DeleteGroups))
                {
                    success = GroupDataAccessor.DeleteHomeGroup(groupId, ref message);
                }
                else
                {
                    message = "You do not have permission to delete a homegroup";
                }
                
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                Success = success
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }


        public JsonResult FetchPeopleInChurch()
        {
            MapDataViewModel mapData = new MapDataViewModel();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                mapData = GroupDataAccessor.FetchPeopleInChurch(currentPerson.ChurchId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                MapData = mapData
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupAttendanceGridSetup()
        {
            GridSetupViewModel gridSetup = new GridSetupViewModel();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.ViewGroupAttendance))
                {
                    gridSetup = GroupDataAccessor.FetchGroupAttendanceGridSetup();
                }
            }

            return Json(gridSetup, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupAttendance(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(common.Permissions.ViewGroupAttendance))
                {
                    jqGridData = GroupDataAccessor.FetchGroupAttendanceJQGrid(currentPerson, request);
                }
            }

            return Json(jqGridData);
        }

        public JsonResult FetchAttendance(int groupId, DateTime date)
        {
            List<AttendanceEventViewModel> attendance = new List<AttendanceEventViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                attendance = EventDataAccessor.FetchGroupAttendance(currentPerson, groupId, date);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Attendance = attendance
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveHomeGroupEvent(HomeGroupEventViewModel hgEvent)
        {
            var message = string.Empty;
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];

                _groupEventRepository.Save(currentPerson, hgEvent);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet); 
        }


        public JsonResult FetchFamilyMembers(int personId, int familyId)
        {
            var response = new
            {
                FamilyMembers = _familyRepository.FetchFamilyMembers(personId, familyId)
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchPerson(int personId)
        {
            var personViewModel = new PersonViewModel();
            var sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (personId > 0)
                {
                    personViewModel = _personService.FetchPersonViewModel(personId, currentPerson);

                    if (personViewModel != null && personViewModel.FacebookId == null && personId != currentPerson.PersonId)
                    {
                        if (Session["FacebookClient"] != null)
                        {
                            var client = (FacebookClient)Session["FacebookClient"];
                            //Search for facebook Id
                            Task.Factory.StartNew(() => SearchForFacebookId(personId, personViewModel.Firstname, personViewModel.Surname, client));
                        }
                    }
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Person = personViewModel
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupInfo(int groupId)
        {
            bool sessionTimedOut = false;
            var groupInfo = new HomeGroupsViewModel();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupInfo = GroupDataAccessor.FetchGroupInfo(currentPerson, groupId);
            }

            var response = new
            {
                GroupInfo = groupInfo,
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveHomeGroup(HomeGroupsViewModel hgvm)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                GroupDataAccessor.SaveHomeGroup(currentPerson, hgvm);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveOptionalFields(List<OptionalFieldViewModel> optionalFields)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                ChurchDataAccessor.SaveChurchOptionalFields(currentPerson.ChurchId, optionalFields);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                OptionalFields = optionalFields
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveGroupAddress(GroupDto groupSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                GroupDataAccessor.SaveGroupSettings(currentPerson, groupSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Address Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBulkSmsDetails(string BulkSmsUsername, string BulkSmsPassword)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                SettingsDataAccessor.SaveBulkSmsDetails(currentPerson, BulkSmsUsername, BulkSmsPassword);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Bulk Sms Details Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveChurchContactDetails(ChurchSettingsViewModel churchSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                SettingsDataAccessor.SaveChurchContactDetails(currentPerson, churchSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Church Contact Details Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewChurch(ChurchSettingsViewModel churchSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                SettingsDataAccessor.CreateNewChurch(currentPerson, churchSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Church Created"
            };

            return Json(response);
        }

        public JsonResult SaveSite(SiteSettingsViewModel siteSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                ChurchDataAccessor.SaveSite(currentPerson, siteSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Site Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult ChangePassword(string currentPassword, string newPassword)
        {
            var sessionTimedOut = false;
            var message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                message=_usernamePasswordRepository.ChangePassword(currentPerson.PersonId, currentPassword, newPassword);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }


        public JsonResult FetchGroupLeaderEmails(bool search, JqGridFilters filters, bool includeMembers)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            List<string> addresses = new List<string>();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailGroupLeaders))
                {
                    Session[SessionVariable.EmailAddresses] = GroupDataAccessor.FetchGroupLeaderAddresses(currentPerson, search, filters, includeMembers);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupLeaderCellPhoneNos(bool search, JqGridFilters filters, bool includeMembers)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            int noNos = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsGroupLeaders))
                {
                    Session[SessionVariable.CellPhoneNos] = GroupDataAccessor.FetchGroupLeaderCellPhoneNos(currentPerson, search, filters, includeMembers);
                    noNos = ((List<string>)Session[SessionVariable.CellPhoneNos]).Count;
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                NoNos = noNos
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupCellPhoneNos(int groupId, List<int> selectedIds, bool selectedOnly)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            int noNos = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsGroupMembers) || currentPerson.HasPermission(Permissions.SmsChurch))
                {
                    Session[SessionVariable.CellPhoneNos] = GroupDataAccessor.FetchGroupCellPhoneNos(currentPerson, groupId, selectedIds, selectedOnly);
                    noNos = ((List<string>)Session[SessionVariable.CellPhoneNos]).Count;
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                NoNos = noNos
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupEmails(int groupId, List<int> selectedIds, bool selectedOnly)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            List<string> addresses = new List<string>();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailGroupMembers))
                {
                    Session[SessionVariable.EmailAddresses] = GroupDataAccessor.FetchGroupAddresses(currentPerson, groupId, selectedIds, selectedOnly);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [ValidateInput(false)]
        public JsonResult SendGroupEmail(string subject, string body)
        {
            if (Session[SessionVariable.EmailAddresses] == null)
            {
                var errorResponse = new
                {
                    SessionTimeOut = false,
                    Message = "No Email addresses selected"
                };

                return Json(errorResponse, JsonRequestBehavior.DenyGet);
            }

            var addressList = (List<string>)Session[SessionVariable.EmailAddresses];
            var sessionTimedOut = false;
            var message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                message = currentPerson.HasPermission(Permissions.EmailGroupMembers) ? _emailService.SendGroupEmail(addressList, subject, body, currentPerson) : ExceptionMessage.InvalidCredentials;
            }

            Session[SessionVariable.EmailAddresses] = null;
            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SendGroupSms(string message)
        {
            if (Session[SessionVariable.CellPhoneNos] == null)
            {
                var errorResponse = new
                {
                    SessionTimeOut = false,
                    Message = "No CellPhoneNos selected"
                };

                return Json(errorResponse, JsonRequestBehavior.DenyGet);
            }

            var chellPhoneNoList = (List<string>)Session[SessionVariable.CellPhoneNos];
            var sessionTimedOut = false;
            var responseMessage = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                responseMessage = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsGroupMembers))
                {
                    string username = string.Empty;
                    string password = string.Empty;
                    ChurchDataAccessor.FetchBulkSmsUsernameAndPassword(currentPerson, out username, out password);
                    if (username == null || password == null)
                    {
                        responseMessage = "Could not find Sms Provider credentials.  Please set them first in the settings tab";
                    }
                    else
                    {
                        responseMessage = Sms.SendSmses(chellPhoneNoList, message, username, password);
                    }
                }
                else
                {
                    responseMessage = ExceptionMessage.InvalidCredentials;
                }
            }

            Session[SessionVariable.CellPhoneNos] = null;
            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = responseMessage
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchChurchEmailAddresses(bool search, string searchField, string searchString)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailChurch))
                {
                    Session[SessionVariable.EmailAddresses] = PersonDataAccessor.FetchChurchEmailAddresses(currentPerson, search, searchField, searchString);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };

            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchChurchCellPhoneNos(bool search, string searchField, string searchString)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            int noNos = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsChurch))
                {
                    Session[SessionVariable.CellPhoneNos] = PersonDataAccessor.FetchChurchCellPhoneNos(currentPerson, search, searchField, searchString);
                    noNos = ((List<string>)Session[SessionVariable.CellPhoneNos]).Count;
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                NoNos = noNos
            };

            return Json(response, JsonRequestBehavior.DenyGet);
        }
        

        public JsonResult SavePersonComment(int personId, string comment)
        {
            var sessionTimedOut = false;
            var message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.AddComment))
                {
                    ICommentService commentService = new CommentService(new CommentRepository());
                    try
                    {
                        commentService.SaveComment(currentPerson,
                                                   new CommentDto
                                                       {
                                                           AboutPersonId = personId,
                                                           Comment       = comment,
                                                           CommentDate   = DateTime.Now
                                                       });
                        message = "Comment Saved";
                    }
                    catch (Exception ex)
                    {
                        _emailService.SendExceptionEmail(ex);
                        message = "There was a problem saving your comment.  Our developers have been notified and will let you know when the problem has been fixed";
                    }
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchPersonCommentHistory(int personId)
        {
            var sessionTimedOut = false;
            var message = string.Empty;
            var comments = new List<CommentDto>();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.ViewComments))
                {
                    ICommentService commentService = new CommentService(new CommentRepository());
                    comments = commentService.GetListOfComments(currentPerson, personId, 5).ToList();
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                Comments = comments
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchChurchEmailTemplate(int churchId, int emailTemplateId)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            string emailTemplate = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EditEmailTemplates))
                {
                    emailTemplate = SettingsDataAccessor.FetchChurchEmailTemplate(currentPerson, churchId, emailTemplateId);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                EmailTemplate = emailTemplate
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [ValidateInput(false)]
        public JsonResult SaveChurchEmailTemplate(int churchId, int emailTemplateId, string template)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EditEmailTemplates))
                {
                    SettingsDataAccessor.SaveChurchEmailTemplate(currentPerson, churchId, emailTemplateId, template);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        
        

        #region Private Methods

        private void SelectNewChurch(int churchId, Person currentPerson)
        {
            var newChurch = _systemAdministratorService.SetNewChurch(currentPerson, churchId);
            if (newChurch != null)
            {
                currentPerson.ChurchId = newChurch.ChurchId;
                currentPerson.Church = newChurch;
                Session[SessionVariable.LoggedOnPerson] = currentPerson;
                Session[SessionVariable.Church] = ChurchDataAccessor.FetchChurch(churchId);
            }
        }

        private void SearchForFacebookId(int personId, string firstname, string surname, FacebookClient client)
        {
            try
            {
                string fullname = firstname + " " + surname;
                dynamic friends = client.Query("SELECT uid, first_name, last_name FROM user WHERE uid IN (    SELECT uid2    FROM friend    WHERE uid1=me()) AND name='" + fullname + "'");
                if (friends.Count == 1)
                {
                    PersonDataAccessor.SavePersonFacebookId(personId, friends[0].uid);
                }
            }
            catch { }

        }
        #endregion Private Methods
    }
}
