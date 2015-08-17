using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common.Models;
using System.Configuration;
using oikonomos.common;

namespace oikonomos.data.DataAccessors
{
    public class SettingsDataAccessor
    {
        public static void SaveChurchEmailTemplate(Person currentPerson, int churchId, int emailTemplateId, string template)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var churchEmailTemplate = context.ChurchEmailTemplates.FirstOrDefault(x => x.ChurchId == churchId && x.EmailTemplateId == emailTemplateId);
                if (churchEmailTemplate == null)
                {
                    churchEmailTemplate = new ChurchEmailTemplate {ChurchId = churchId, EmailTemplateId = emailTemplateId};
                    context.ChurchEmailTemplates.AddObject(churchEmailTemplate);
                }
                churchEmailTemplate.Template = template;
                context.SaveChanges();
            }
        }

        public static string FetchChurchEmailTemplate(Person currentPerson, int churchId, int emailTemplateId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var churchEmailTemplate = context.ChurchEmailTemplates.FirstOrDefault(x => x.ChurchId == churchId && x.EmailTemplateId == emailTemplateId);
                return churchEmailTemplate==null ? string.Empty : churchEmailTemplate.Template;
            }
        }
        
        public static List<StandardCommentViewModel> FetchStandardComments(Person currentPerson)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var shouldReturnAnyStandardComments =
                    context.ChurchOptionalFields.Where(
                        c =>
                        c.ChurchId == currentPerson.ChurchId &&
                        c.OptionalFieldId == (int) OptionalFields.HomeGroupEvents)
                        .Select(c=>c.Visible)
                        .FirstOrDefault();
                if(shouldReturnAnyStandardComments==false)
                    return new List<StandardCommentViewModel>();
                
                return (from e in context.StandardComments
                        where e.ChurchId == currentPerson.ChurchId
                        select new StandardCommentViewModel
                        {
                            StandardCommentId = e.StandardCommentId,
                            StandardComment = e.StandardComment1
                        }).ToList();
            }
        }

        public static List<StandardCommentViewModel> AddStandardComment(Person currentPerson, string standardComment)
        {

            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(common.Permissions.AddEvent))
                {
                    var check = CheckToSeeIfTheCommentIsAlreadyThere(currentPerson, standardComment, context);

                    if (check == 0)
                    {
                        var newStandardComment = new StandardComment
                                               {
                                                   StandardComment1= standardComment,
                                                   ChurchId        = currentPerson.ChurchId
                                               };

                        context.StandardComments.AddObject(newStandardComment);
                        context.SaveChanges();
                    }
                }

                return (from e in context.StandardComments
                        where e.ChurchId == currentPerson.ChurchId
                        select new StandardCommentViewModel
                        {
                            StandardCommentId = e.StandardCommentId,
                            StandardComment = e.StandardComment1
                        }).ToList();
            }
        }

        private static int CheckToSeeIfTheCommentIsAlreadyThere(Person currentPerson, string comment, oikonomosEntities context)
        {
            var check = (from e in context.StandardComments
                         where e.ChurchId == currentPerson.ChurchId
                               && e.StandardComment1 == comment
                         select e).Count();
            return check;
        }

        public static List<StandardCommentViewModel> DeleteStandardComment(Person currentPerson, int standardCommentId)
        {

            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(Permissions.DeleteEvent))
                {
                    var eventTypeToDelete = (from e in context.StandardComments
                                             where e.ChurchId == currentPerson.ChurchId
                                                   && e.StandardCommentId == standardCommentId
                                             select e).FirstOrDefault();

                    context.StandardComments.DeleteObject(eventTypeToDelete);

                    context.SaveChanges();

                }

                return (from e in context.StandardComments
                        where e.ChurchId == currentPerson.ChurchId
                        select new StandardCommentViewModel
                        {
                            StandardCommentId = e.StandardCommentId,
                            StandardComment = e.StandardComment1
                        }).ToList();

            }
        }

        public static List<SuburbViewModel> FetchSuburbs(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var defaultOption = new SuburbViewModel { SuburbId = 0, SuburbName = "Select..." };
                var suburbs = (from s in context.ChurchSuburbs
                        where s.ChurchId == currentPerson.ChurchId
                        select new SuburbViewModel
                        {
                            SuburbId = s.ChurchSuburbId,
                            SuburbName = s.Suburb
                        })
                        .ToList();
                suburbs.Insert(0, defaultOption);
                return suburbs;
            }
        }

        public static List<SuburbViewModel> AddSuburb(Person currentPerson, string suburbName)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(common.Permissions.AddSuburb))
                {//Check to see if it is not already in the db
                    var check = (from s in context.ChurchSuburbs
                                 where s.ChurchId == currentPerson.ChurchId
                                 && s.Suburb == suburbName
                                 select s).Count();
                    if (check == 0)
                    {
                        ChurchSuburb suburb = new ChurchSuburb();
                        suburb.Created = DateTime.Now;
                        suburb.Changed = DateTime.Now;
                        suburb.Suburb = suburbName;
                        suburb.ChurchId = currentPerson.ChurchId;

                        context.ChurchSuburbs.AddObject(suburb);
                        context.SaveChanges();
                    }

                }

                return (from s in context.ChurchSuburbs
                        where s.ChurchId == currentPerson.ChurchId
                        select new SuburbViewModel
                        {
                            SuburbId = s.ChurchSuburbId,
                            SuburbName = s.Suburb
                        }).ToList();
            }
        }

        public static List<SuburbViewModel> DeleteSuburb(Person currentPerson, int suburbId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(common.Permissions.DeleteSuburb))
                {
                    var peopleInSuburb = (from p in context.People.Include("Family").Include("Address")
                                          from c in p.PersonChurches
                                          join f in context.Families
                                            on p.FamilyId equals f.FamilyId
                                          join a in context.Addresses
                                            on f.AddressId equals a.AddressId
                                          where c.ChurchId == currentPerson.ChurchId
                                          && a.ChurchSuburbId == suburbId
                                          select p).ToList();

                    foreach (Person p in peopleInSuburb)
                    {
                        p.Family.Address.ChurchSuburbId = null;
                    }

                    var groupsInSuburb = (from g in context.Groups.Include("Address")
                                          join a in context.Addresses
                                            on g.AddressId equals a.AddressId
                                          where g.ChurchId == currentPerson.ChurchId
                                          && a.ChurchSuburbId == suburbId
                                          select g).ToList();

                    foreach (Group g in groupsInSuburb)
                    {
                        g.Address.ChurchSuburbId = null;
                    }

                    var churchSuburbToDelete = (from s in context.ChurchSuburbs
                                                where s.ChurchId == currentPerson.ChurchId
                                                && s.ChurchSuburbId == suburbId
                                                select s).FirstOrDefault();
                    
                    context.ChurchSuburbs.DeleteObject(churchSuburbToDelete);

                    context.SaveChanges();

                }

                return (from s in context.ChurchSuburbs
                        where s.ChurchId == currentPerson.ChurchId
                        select new SuburbViewModel
                        {
                            SuburbId = s.ChurchSuburbId,
                            SuburbName = s.Suburb
                        }).ToList();

            }
        }

        public static List<GroupClassificationViewModel> FetchGroupClassifications(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from g in context.GroupClassifications
                        where g.ChurchId == currentPerson.ChurchId
                        select new GroupClassificationViewModel
                        {
                            GroupClassificationId = g.GroupClassificationId,
                            GroupClassification = g.Name
                        }).ToList();
            }
        }

        public static List<GroupClassificationViewModel> AddGroupClassification(Person currentPerson, string groupClassification)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(common.Permissions.AddGroupClassification))
                {//Check to see if it is not already in the db
                    var check = (from g in context.GroupClassifications
                                 where g.ChurchId == currentPerson.ChurchId
                                 && g.Name == groupClassification
                                 select g).Count();
                    if (check == 0)
                    {
                        GroupClassification gc = new GroupClassification();
                        gc.Created = DateTime.Now;
                        gc.Changed = DateTime.Now;
                        gc.Name = groupClassification;
                        gc.ChurchId = currentPerson.ChurchId;
                        if (currentPerson.ChurchId == 3) //ebenezer
                        {
                            gc.GroupTypeId = (int)GroupTypes.LifeGroup;
                        }
                        else
                        {
                            gc.GroupTypeId = (int)GroupTypes.HomeGroup;
                        }

                        context.GroupClassifications.AddObject(gc);
                        context.SaveChanges();
                    }

                }

                return (from g in context.GroupClassifications
                        where g.ChurchId == currentPerson.ChurchId
                        select new GroupClassificationViewModel
                        {
                            GroupClassificationId = g.GroupClassificationId,
                            GroupClassification = g.Name
                        }).ToList();

            }
        }

        public static List<GroupClassificationViewModel> DeleteGroupClassification(Person currentPerson, int groupClassificationId)
        {

            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(common.Permissions.DeleteGroupClassification))
                {
                    var groups = (from g in context.Groups
                                          where g.ChurchId == currentPerson.ChurchId
                                          && g.GroupClassificationId == groupClassificationId
                                          select g).ToList();

                    foreach (Group g in groups)
                    {
                        g.GroupClassificationId = null;
                    }

                    var groupClassificationsToDelete = (from g in context.GroupClassifications
                                                        where g.ChurchId == currentPerson.ChurchId
                                                        && g.GroupClassificationId == groupClassificationId
                                                        select g).FirstOrDefault();

                    context.GroupClassifications.DeleteObject(groupClassificationsToDelete);

                    context.SaveChanges();

                }

                return (from g in context.GroupClassifications
                        where g.ChurchId == currentPerson.ChurchId
                        select new GroupClassificationViewModel
                        {
                            GroupClassificationId = g.GroupClassificationId,
                            GroupClassification = g.Name
                        }).ToList();

            }
        }
        public static void SaveBulkSmsDetails(Person currentPerson, string username, string password)
        {
            if (currentPerson.HasPermission(common.Permissions.EditBulkSmsDetails))
            {
                using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
                {
                    ChurchSmsProvider currentSettings = (from c in context.ChurchSmsProviders
                                                         where c.ChurchId == currentPerson.ChurchId
                                                             && c.SmsProviderId == (int)SmsProviders.BulkSmsSouthAfrica
                                                         select c)
                                                        .FirstOrDefault();

                    if (currentSettings == null)
                    {
                        currentSettings = new ChurchSmsProvider();
                        currentSettings.Created = DateTime.Now;
                        currentSettings.SmsProviderId = (int)SmsProviders.BulkSmsSouthAfrica;
                        currentSettings.ChurchId = currentPerson.ChurchId;
                        context.ChurchSmsProviders.AddObject(currentSettings);
                    }

                    currentSettings.Username = username;
                    currentSettings.Password = password;

                    context.SaveChanges();
                }
            }
        }

        public static void SaveChurchContactDetails(Person currentPerson, ChurchSettingsViewModel churchSettings)
        {
            if (currentPerson.HasPermission(common.Permissions.EditChurchContactDetails))
            {
                using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
                {
                    var churchToSave = (from c in context.Churches
                                        where c.ChurchId == currentPerson.ChurchId
                                        select c).FirstOrDefault();

                    PopulateChurchModel(churchSettings, churchToSave);
                    PopulateChurchAddress(churchSettings, context, churchToSave);

                    context.SaveChanges();
                }
            }
        }

        public static void CreateNewChurch(Person currentPerson, ChurchSettingsViewModel churchSettings)
        {
            if (!currentPerson.HasPermission(Permissions.SystemAdministrator)) return;
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var newChurch = new Church();
                context.AddToChurches(newChurch);
                newChurch.Created = DateTime.Now;

                PopulateChurchModel(churchSettings, newChurch);
                newChurch.EmailLogin = "support@oikonomos.co.za";
                newChurch.EmailPassword = "sandton2000";
                newChurch.Country = "South Africa";
                PopulateChurchAddress(churchSettings, context, newChurch);
                context.SaveChanges();

                //Save Roles
                var currentChurchRoles = context.Roles.Where(r => (r.ChurchId == currentPerson.ChurchId && r.Name != "System Administrator")).ToList();
                foreach (var currentRole in currentChurchRoles)
                {
                    var newChurchRole = new Role();
                    context.AddToRoles(newChurchRole);
                    newChurchRole.Created = DateTime.Now;
                    newChurchRole.Changed = DateTime.Now;
                    newChurchRole.Name = currentRole.Name;
                    newChurchRole.DisplayName = currentRole.DisplayName;
                    newChurchRole.ChurchId = newChurch.ChurchId;

                    foreach (var permission in currentRole.PermissionRoles)
                    {
                        var newRolePerm = new PermissionRole();
                        context.AddToPermissionRoles(newRolePerm);
                        newRolePerm.Created = DateTime.Now;
                        newRolePerm.Changed = DateTime.Now;
                        newRolePerm.PermissionId = permission.PermissionId;
                        newChurchRole.PermissionRoles.Add(newRolePerm);
                    }
                }

                context.SaveChanges();

                //Update Role that can be set by any role
                foreach (var currentRole in currentChurchRoles)
                {
                    var newRole = context.Roles.FirstOrDefault(r => (r.ChurchId == newChurch.ChurchId && r.Name == currentRole.Name));
                    foreach (var newRoleToSet in from roleToSet in currentRole.CanSetRoles where roleToSet.Name != "System Administrator" select context.Roles.FirstOrDefault(r => (r.ChurchId == newChurch.ChurchId && r.Name == roleToSet.Name)))
                    {
                        newRole.CanSetRoles.Add(newRoleToSet);
                    }
                }

                context.SaveChanges();

                var personAddress = new Address {Created = DateTime.Now, Changed = DateTime.Now, Line1 = string.Empty, Line2=string.Empty, Line3=string.Empty, Line4 = string.Empty};
                context.AddToAddresses(personAddress);

                var churchAdministrator              = new Person();
                context.AddToPeople(churchAdministrator);
                churchAdministrator.Created          = DateTime.Now;
                churchAdministrator.Changed          = DateTime.Now;
                churchAdministrator.Firstname        = churchSettings.ContactFirstname;
                churchAdministrator.Church           = newChurch;
                churchAdministrator.Email            = churchSettings.OfficeEmail;
                var churchAdministratorFamily        = new Family();
                context.AddToFamilies(churchAdministratorFamily);
                churchAdministratorFamily.FamilyName = churchSettings.ContactSurname;
                churchAdministratorFamily.Created    = DateTime.Now;
                churchAdministratorFamily.Changed    = DateTime.Now;
                churchAdministrator.Family           = churchAdministratorFamily;
                churchAdministrator.Family.Address   = personAddress;

                context.SaveChanges();

                //Set the new persons role to administrator
                var personChurchRecord = new PersonChurch
                    {
                        Person = churchAdministrator,
                        Church = newChurch,
                        Role = context.Roles.First(r => (r.ChurchId == newChurch.ChurchId && r.Name == "Church Administrator"))
                    };

                context.AddToPersonChurches(personChurchRecord);
                context.SaveChanges();

                //Update Church Optional Fields
                var churchOptionalFields = context.ChurchOptionalFields.Where(c=>c.ChurchId == currentPerson.ChurchId);
                foreach (var co in churchOptionalFields)
                {
                    var newCo = new ChurchOptionalField();
                    context.AddToChurchOptionalFields(newCo);
                    newCo.Created = DateTime.Now;
                    newCo.Changed = DateTime.Now;
                    newCo.ChurchId = newChurch.ChurchId;
                    newCo.OptionalFieldId = co.OptionalFieldId;
                    newCo.Visible = co.Visible;
                }

                context.SaveChanges();
            }
        }

        public static List<OptionalFieldViewModel> FetchChurchOptionalFields(int churchId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from c in context.OptionalFields
                        join cc in context.ChurchOptionalFields
                        on c.OptionalFieldId equals cc.OptionalFieldId into churchOptionalFields
                        from cc in churchOptionalFields.DefaultIfEmpty()
                        where (cc.ChurchId == churchId || cc.ChurchId == null)
                        select new OptionalFieldViewModel
                        {
                            ChurchOptionalFieldId = cc.ChurchOptionalFieldId == null ? 0 : cc.ChurchOptionalFieldId,
                            OptionalFieldId = c.OptionalFieldId,
                            Name = c.Name,
                            Regex = c.Regex ?? string.Empty,
                            Display = cc.Visible == null ? true : cc.Visible
                        }).ToList();
            }

        }
        
        public static SettingsViewModel FetchSettings(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                SettingsViewModel settings = new SettingsViewModel();
                settings.ChurchSettings = (from c in context.Churches
                                           where c.ChurchId == currentPerson.ChurchId
                                           select new ChurchSettingsViewModel
                                           {
                                               ChurchName = c.Name,
                                               OfficeEmail = c.OfficeEmail,
                                               OfficePhone = c.OfficePhone,
                                               SystemName = c.SiteHeader,
                                               UITheme = c.UITheme,
                                               Url = c.Url,
                                               AddressId = c.AddressId.HasValue ? c.AddressId.Value : 0,
                                               Address1 = c.Address.Line1,
                                               Address2 = c.Address.Line2,
                                               Address3 = c.Address.Line3,
                                               Address4 = c.Address.Line4,
                                               Lat = c.Address == null ? 0 : c.Address.Lat,
                                               Lng = c.Address == null ? 0 : c.Address.Long,
                                               AddressType = c.Address.AddressType,
                                               Province = c.Province,
                                               BulkSmsUsername = (from smsProv in c.ChurchSmsProviders where smsProv.SmsProviderId == (int)SmsProviders.BulkSmsSouthAfrica select smsProv.Username).FirstOrDefault(),
                                               BulkSmsPassword = (from smsProv in c.ChurchSmsProviders where smsProv.SmsProviderId == (int)SmsProviders.BulkSmsSouthAfrica select smsProv.Password).FirstOrDefault(),
                                               BirthdayAnniversayReminderFrequency = c.Reminders.FirstOrDefault(r=>r.ReminderType == "BirthdayAnniversary").ReminderFrequency
                                           }).FirstOrDefault();


                settings.GroupSettings = (from g in context.Groups
                                          where g.LeaderId == currentPerson.PersonId
                                          || g.AdministratorId == currentPerson.PersonId
                                          select new GroupDto
                                          {
                                              GroupId = g.GroupId,
                                              GroupName = g.Name,
                                              AddressId = g.AddressId.HasValue ? g.AddressId.Value : 0,
                                              Address1 = g.Address.Line1,
                                              Address2 = g.Address.Line2,
                                              Address3 = g.Address.Line3,
                                              Address4 = g.Address.Line4,
                                              AddressType = g.Address.AddressType
                                          }).FirstOrDefault();

                settings.OptionalFields = (from c in context.OptionalFields
                                           join cc in context.ChurchOptionalFields
                                           on c.OptionalFieldId equals cc.OptionalFieldId into churchOptionalFields
                                           from cc in churchOptionalFields.DefaultIfEmpty()
                                           where (cc.ChurchId == currentPerson.ChurchId || cc.ChurchId == null)
                                           select new OptionalFieldViewModel
                                           {
                                               ChurchOptionalFieldId = cc.ChurchOptionalFieldId == null ? 0 : cc.ChurchOptionalFieldId,
                                               OptionalFieldId = c.OptionalFieldId,
                                               Name = c.Name,
                                               Regex = c.Regex == null ? string.Empty : c.Regex,
                                               Display = cc.Visible == null ? true : cc.Visible
                                           }).ToList();

                settings.Sites = (from s in context.Sites
                                  where s.ChurchId == currentPerson.ChurchId
                                  select new SiteSettingsViewModel
                                  {
                                      Address1 = s.Address.Line1,
                                      Address2 = s.Address.Line2,
                                      Address3 = s.Address.Line3,
                                      Address4 = s.Address.Line4,
                                      Lat = s.Address == null ? 0 : s.Address.Lat,
                                      Lng = s.Address == null ? 0 : s.Address.Long,
                                      AddressId = s.AddressId.HasValue ? s.AddressId.Value : 0,
                                      AddressType = s.Address.AddressType,
                                      SiteId = s.SiteId,
                                      SiteName = s.Name
                                  }).ToList();

                settings.Roles = (from r in context.Roles
                                  where r.ChurchId == currentPerson.ChurchId
                                  select new RoleViewModel
                                  {
                                      RoleId = r.RoleId,
                                      Name = r.Name
                                  }).ToList();

                settings.RoleId = settings.Roles[0].RoleId;

                return settings;
            }
        }

        public static SysAdminViewModel FetchSysAdminViewModel(Person currentPerson)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var sysAdminViewModel = new SysAdminViewModel();
                if (currentPerson.HasPermission(Permissions.SystemAdministrator))
                {
                    sysAdminViewModel.EmailTemplates = (from et in context.EmailTemplates
                                                        select new EmailTemplateViewModel
                                                        {
                                                            EmailTemplateId = et.EmailTemplateId,
                                                            Name = et.Name
                                                        }).ToList();

                    sysAdminViewModel.EmailTemplateId = sysAdminViewModel.EmailTemplates[0].EmailTemplateId;

                    sysAdminViewModel.ChurchId = currentPerson.ChurchId;
                    sysAdminViewModel.Churches = ChurchDataAccessor.FetchChurches(currentPerson);
                }
                return sysAdminViewModel;
            }
        }


        private static void PopulateChurchModel(ChurchSettingsViewModel churchSettings, Church churchToSave)
        {
            churchToSave.Changed = DateTime.Now;
            churchToSave.Name = churchSettings.ChurchName;
            churchToSave.OfficeEmail = churchSettings.OfficeEmail;
            churchToSave.OfficePhone = churchSettings.OfficePhone;
            churchToSave.SiteHeader = churchSettings.SystemName;
            churchToSave.UITheme = churchSettings.UITheme;
            churchToSave.Url = churchSettings.Url;
            churchToSave.Province = churchSettings.Province;
            churchToSave.BackgroundImage = "default.png";
            churchToSave.StatusId = (int)ChurchStatuses.Pending;
        }


        private static void PopulateChurchAddress(ChurchSettingsViewModel churchSettings, oikonomosEntities context, Church churchToSave)
        {
            //Check to see if the address already exists
            var address = new Address();

            if (churchSettings.AddressId > 0)
            {
                address = (from a in context.Addresses
                           where a.AddressId == churchSettings.AddressId
                           select a).FirstOrDefault();

                if (address == null) //Should never happen, but just to be sure
                {
                    address = new Address();
                    address.Created = DateTime.Now;
                    churchSettings.AddressId = 0;
                }
            }
            else
            {
                address.Created = DateTime.Now;
            }

            address.Line1 = churchSettings.Address1 ?? string.Empty;
            address.Line2 = churchSettings.Address2 ?? string.Empty;
            address.Line3 = churchSettings.Address3 ?? string.Empty;
            address.Line4 = churchSettings.Address4 ?? string.Empty;
            address.AddressType = churchSettings.AddressType ?? string.Empty;
            address.Lat = churchSettings.Lat;
            address.Long = churchSettings.Lng;
            address.Changed = DateTime.Now;

            if (churchSettings.AddressId == 0)
            {
                context.Addresses.AddObject(address);
                churchToSave.Address = address;
            }
        }

    }
}
