using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class SiteSettingsViewModel
    {
        public int SiteId { get; set; }
        public int AddressId { get; set; }
        public string SiteName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string AddressType { get; set; }
    }
}
