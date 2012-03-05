using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class HomeGroupEventViewModel
    {
        public int GroupId { get; set; }
        public DateTime EventDate { get; set; }
        public string NoVisitors { get; set; }
        public List<PersonEventViewModel> Events { get; set; }
    }
}
