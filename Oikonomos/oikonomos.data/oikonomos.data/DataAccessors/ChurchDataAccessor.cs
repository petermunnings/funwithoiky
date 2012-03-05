using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common.Models;
using System.Configuration;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common;

namespace oikonomos.data.DataAccessors
{
    public class ChurchDataAccessor
    {
        public static void FetchBulkSmsUsernameAndPassword(Person currentPerson, out string username, out string password)
        {
            if (currentPerson.HasPermission(common.Permissions.SendSms))
            {
                using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
                {
                    username = (from c in context.ChurchSmsProviders
                                where c.ChurchId == currentPerson.ChurchId
                                    && c.SmsProviderId == (int)SmsProviders.BulkSmsSouthAfrica
                                select c.Username)
                                .FirstOrDefault();

                    password = (from c in context.ChurchSmsProviders
                                where c.ChurchId == currentPerson.ChurchId
                                    && c.SmsProviderId == (int)SmsProviders.BulkSmsSouthAfrica
                                select c.Password)
                                .FirstOrDefault();

                    return;
                }
            }

            username = null;
            password = null;
        }
        
        public static string DeleteSite(Person currentPerson, int siteId)
        {
            if (currentPerson.HasPermission(common.Permissions.DeleteSite))
            {
                using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
                {
                    var siteToDelete = (from s in context.Sites
                                        where s.ChurchId == currentPerson.ChurchId
                                        && s.SiteId == siteId
                                        select s).FirstOrDefault();
                    
                    if (siteToDelete == null)
                    {
                        return "Could not delete site";
                    }

                    //Remove all the people linked to this site
                    var peopleLinkedToSite = (from p in context.People
                                              where p.SiteId == siteId
                                              select p);

                    foreach (Person p in peopleLinkedToSite)
                    {
                        p.SiteId = null;
                        p.Changed = DateTime.Now;
                    }

                    context.Sites.DeleteObject(siteToDelete);

                    context.SaveChanges();
                    return "Site succesfully removed";
                }
            }
            else
            {
                return "Could not delete site";
            }

        }

        public static List<SiteSettingsViewModel> FetchSites(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from s in context.Sites
                        where s.ChurchId == currentPerson.ChurchId
                        select new SiteSettingsViewModel
                        {
                            Address1 = s.Address.Line1,
                            Address2 = s.Address.Line2,
                            Address3 = s.Address.Line3,
                            Address4 = s.Address.Line4,
                            AddressId = s.AddressId.HasValue ? s.AddressId.Value : 0,
                            AddressType = s.Address.AddressType,
                            Lat = s.Address.Lat == null ? 0 : s.Address.Lat,
                            Lng = s.Address.Long == null ? 0 : s.Address.Long,
                            SiteName = s.Name,
                            SiteId = s.SiteId
                        }).ToList();
            }

        }

        public static SiteSettingsViewModel FetchSite(Person currentPerson, int siteId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from s in context.Sites
                        where s.ChurchId == currentPerson.ChurchId
                        && s.SiteId == siteId
                        select new SiteSettingsViewModel
                        {
                            Address1 = s.Address.Line1,
                            Address2 = s.Address.Line2,
                            Address3 = s.Address.Line3,
                            Address4 = s.Address.Line4,
                            AddressId = s.AddressId.HasValue ? s.AddressId.Value : 0,
                            AddressType = s.Address.AddressType,
                            Lat = s.Address.Lat==null ? 0 : s.Address.Lat,
                            Lng = s.Address.Long == null ? 0 : s.Address.Long,
                            SiteName = s.Name,
                            SiteId = s.SiteId
                        }).FirstOrDefault();
            }

        }
        public static void SaveSite(Person currentPerson, SiteSettingsViewModel siteSettings)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                //Check Roles
                if (currentPerson.HasPermission(common.Permissions.AddSite) ||
                    currentPerson.HasPermission(common.Permissions.EditSite))
                {
                    Site siteToSave = new Site();
                    if (siteSettings.SiteId == 0)
                    {
                        if (currentPerson.HasPermission(common.Permissions.AddSite))
                        {
                            siteToSave.Created = DateTime.Now;
                            siteToSave.ChurchId = currentPerson.ChurchId;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (currentPerson.HasPermission(Permissions.EditSite))
                        {
                            siteToSave = (from s in context.Sites.Include("Address")
                                          where s.SiteId == siteSettings.SiteId
                                          select s).FirstOrDefault();

                            if (siteToSave == null)
                            {
                                siteToSave = new Site();
                                siteToSave.Created = DateTime.Now;
                                siteToSave.ChurchId = currentPerson.ChurchId;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                    siteToSave.Changed = DateTime.Now;
                    siteToSave.Name = siteSettings.SiteName;

                    if (siteSettings.AddressId == 0)
                    {
                        siteToSave.Address = new Address();
                        siteToSave.Address.Created = DateTime.Now;
                    }

                    siteToSave.Address.Changed = DateTime.Now;
                    siteToSave.Address.Line1 = siteSettings.Address1 ?? string.Empty;
                    siteToSave.Address.Line2 = siteSettings.Address2 ?? string.Empty;
                    siteToSave.Address.Line3 = siteSettings.Address3 ?? string.Empty;
                    siteToSave.Address.Line4 = siteSettings.Address4 ?? string.Empty;
                    siteToSave.Address.Lat = siteSettings.Lat;
                    siteToSave.Address.Long = siteSettings.Lng;
                    siteToSave.Address.AddressType = siteSettings.AddressType ?? string.Empty;

                    context.Sites.AddObject(siteToSave);

                    context.SaveChanges();
                }
            }
        }

        public static JqGridData FetchSitesJQGrid(Person currentPerson, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var sites = (from s in context.Sites.Include("Address")
                              where s.ChurchId == currentPerson.ChurchId
                              select s);

                if (request._search)
                {
                    sites = (from s in sites
                             where s.Name.Contains(request.searchString)
                              || s.Address.Line1.Contains(request.searchString)
                              || s.Address.Line2.Contains(request.searchString)
                              || s.Address.Line3.Contains(request.searchString)
                              || s.Address.Line4.Contains(request.searchString)
                              select s);
                }

                int totalRecords = sites.Count();

                switch (request.sidx)
                {
                    case "SiteName":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                sites = sites.OrderBy(s => s.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                sites = sites.OrderByDescending(s => s.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Address1":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                sites = sites.OrderBy(s => s.Address.Line1).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                sites = sites.OrderByDescending(s => s.Address.Line1).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Address2":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                sites = sites.OrderBy(s => s.Address.Line2).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                sites = sites.OrderByDescending(s => s.Address.Line2).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Address3":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                sites = sites.OrderBy(s => s.Address.Line3).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                sites = sites.OrderByDescending(s => s.Address.Line3).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Address4":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                sites = sites.OrderBy(s => s.Address.Line4).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                sites = sites.OrderByDescending(s => s.Address.Line4).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData sitesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from s in sites.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = s.SiteId.ToString(),
                                cell = new string[] {
                                                    s.SiteId.ToString(),
                                                    s.Name,
                                                    s.Address.Line1,
                                                    s.Address.Line2,
                                                    s.Address.Line3,
                                                    s.Address.Line4
                                                }
                            }).ToArray()
                };
                return sitesGridData;
            }
        }
        
        public static ChurchViewModel FetchChurch(string churchName)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from c in context.Churches
                        where c.Name == churchName
                        select new ChurchViewModel
                        {
                            ChurchId = c.ChurchId,
                            ChurchName = c.Name,
                            SiteHeader = c.SiteHeader,
                            SiteDescription = c.SiteDescription,
                            BackgroundImage = c.BackgroundImage,
                            UITheme = c.UITheme,
                            GoogleSearchRegion = c.Province,
                            ShowFacebookLogin = c.ChurchOptionalFields.Where<ChurchOptionalField>(co => co.OptionalFieldId == (int)OptionalFields.Facebook).FirstOrDefault().Visible
                        }).FirstOrDefault();

            }
        }

        public static void SaveChurchOptionalFields(int churchId, List<OptionalFieldViewModel> optionalFields)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                foreach (OptionalFieldViewModel optionalField in optionalFields)
                {
                    ChurchOptionalField ct = new ChurchOptionalField();
                    if (optionalField.ChurchOptionalFieldId != 0)
                    {
                        ct = (from c in context.ChurchOptionalFields
                              where c.ChurchOptionalFieldId == optionalField.ChurchOptionalFieldId
                              select c).FirstOrDefault();
                    }
                    else
                    {
                        ct.ChurchId = churchId;
                        ct.OptionalFieldId = optionalField.OptionalFieldId;
                        ct.Created = DateTime.Now;
                        context.AddToChurchOptionalFields(ct);
                    }

                    if (optionalField.Display != ct.Visible || ct.Changed==DateTime.MinValue)
                        ct.Changed = DateTime.Now;
                    ct.Visible = optionalField.Display;
                }

                context.SaveChanges();
            }
        }
    
    }
}
