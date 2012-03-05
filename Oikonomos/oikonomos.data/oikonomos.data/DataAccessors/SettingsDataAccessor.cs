﻿using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common.Models;
using System.Configuration;
using oikonomos.common;

namespace oikonomos.data.DataAccessors
{
    public class SettingsDataAccessor
    {
        public static List<EventTypeViewModel> FetchEventTypes(Person currentPerson, string eventFor)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var tableId = (from t in context.Tables
                               where t.Name == eventFor
                               select t.TableId).FirstOrDefault();

                return (from e in context.EventTypes
                        where e.ChurchId == currentPerson.ChurchId
                        && e.TableId==tableId
                        select new EventTypeViewModel
                        {
                            EventTypeId = e.EventTypeId,
                            EventType = e.Name
                        }).ToList();
            }
        }

        public static List<EventTypeViewModel> AddEventType(Person currentPerson, string eventType, string eventFor)
        {

            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var tableId = (from t in context.Tables
                               where t.Name == eventFor
                               select t.TableId).FirstOrDefault();

                if (currentPerson.HasPermission(common.Permissions.AddEvent))
                {//Check to see if it is not already in the db


                    var check = (from e in context.EventTypes
                                 where e.ChurchId == currentPerson.ChurchId
                                 && e.TableId == tableId
                                 && e.Name == eventType
                                 select e).Count();
                    
                    if (check == 0)
                    {
                        EventType newEventType = new EventType();
                        newEventType.Created = DateTime.Now;
                        newEventType.Changed = DateTime.Now;
                        newEventType.Name = eventType;
                        newEventType.ChurchId = currentPerson.ChurchId;
                        newEventType.TableId = tableId;

                        context.EventTypes.AddObject(newEventType);
                        context.SaveChanges();
                    }

                }

                return (from e in context.EventTypes
                        where e.ChurchId == currentPerson.ChurchId
                        && e.TableId == tableId
                        select new EventTypeViewModel
                        {
                            EventTypeId = e.EventTypeId,
                            EventType = e.Name
                        }).ToList();
            }
        }

        public static List<EventTypeViewModel> DeleteEventType(Person currentPerson, int eventTypeId, string eventFor)
        {

            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var tableId = (from t in context.Tables
                               where t.Name == eventFor
                               select t.TableId).FirstOrDefault();
                
                if (currentPerson.HasPermission(common.Permissions.DeleteEvent))
                {
                    var eventTypeToDelete = (from e in context.EventTypes
                                             where e.ChurchId == currentPerson.ChurchId
                                             && e.TableId == tableId
                                             && e.EventTypeId == eventTypeId
                                             select e).FirstOrDefault();

                    context.EventTypes.DeleteObject(eventTypeToDelete);

                    context.SaveChanges();

                }

                return (from e in context.EventTypes
                        where e.ChurchId == currentPerson.ChurchId
                        && e.TableId == tableId
                        select new EventTypeViewModel
                        {
                            EventTypeId = e.EventTypeId,
                            EventType = e.Name
                        }).ToList();

            }
        }

        public static List<SuburbViewModel> FetchSuburbs(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from s in context.ChurchSuburbs
                        where s.ChurchId == currentPerson.ChurchId
                        select new SuburbViewModel
                        {
                            SuburbId = s.ChurchSuburbId,
                            SuburbName = s.Suburb
                        }).ToList();
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
                                          join f in context.Families
                                            on p.FamilyId equals f.FamilyId
                                          join a in context.Addresses
                                            on f.AddressId equals a.AddressId
                                          where p.ChurchId == currentPerson.ChurchId
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

                    churchToSave.Changed = DateTime.Now;
                    churchToSave.Name = churchSettings.ChurchName;
                    churchToSave.OfficeEmail = churchSettings.OfficeEmail;
                    churchToSave.OfficePhone = churchSettings.OfficePhone;
                    churchToSave.SiteHeader = churchSettings.SystemName;
                    churchToSave.UITheme = churchSettings.UITheme;
                    churchToSave.Url = churchSettings.Url;
                    churchToSave.Province = churchSettings.Province;

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

                    address.Line1 = churchSettings.Address1;
                    address.Line2 = churchSettings.Address2;
                    address.Line3 = churchSettings.Address3;
                    address.Line4 = churchSettings.Address4;
                    address.AddressType = churchSettings.AddressType;
                    address.Lat = churchSettings.Lat;
                    address.Long = churchSettings.Lng;
                    address.Changed = DateTime.Now;

                    if (churchSettings.AddressId == 0)
                    {
                        context.Addresses.AddObject(address);
                        churchToSave.Address = address;
                    }

                    context.SaveChanges();
                }
            }
        }

        public static List<OptionalFieldViewModel> FetchChurchOptionalFields(int churchId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                            Regex = c.Regex == null ? string.Empty : c.Regex,
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
                                               BulkSmsPassword = (from smsProv in c.ChurchSmsProviders where smsProv.SmsProviderId == (int)SmsProviders.BulkSmsSouthAfrica select smsProv.Password).FirstOrDefault()
                                           }).FirstOrDefault();


                settings.GroupSettings = (from g in context.Groups
                                          where g.LeaderId == currentPerson.PersonId
                                          || g.AdministratorId == currentPerson.PersonId
                                          select new GroupSettingsViewModel
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

                return settings;
            }
        }
    }
}
