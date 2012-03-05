using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class MapDataViewModel
    {
        public string ChurchName { get; set; }
        public decimal ChurchLat { get; set; }
        public decimal ChurchLng { get; set; }
        public List<MapDataMember> Members { get; set; }
        public List<MapDataHomeGroup> HomeGroups { get; set; }
    }

    public class MapDataMember
    {
        public string Surname { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }

    public class MapDataHomeGroup
    {
        public string HomeGroupName { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}
