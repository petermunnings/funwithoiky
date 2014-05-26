using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oikonomos.common.Models
{
    public class HomeGroupsViewModel
    {
        public int GroupId { get; set; }
        public string ChurchName { get; set; }
        public string GroupName { get; set; }
        public string LeaderName { get; set; }
        public int LeaderId { get; set; }
        public string AdministratorName { get; set; }
        public int AdministratorId { get; set; }
        public string OverseeingElderName { get; set; }
        public int OverseeingElderId { get; set; }
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string AddressType { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public int GroupClassificationId { get; set; }
        public int SuburbId { get; set; }
    }
}