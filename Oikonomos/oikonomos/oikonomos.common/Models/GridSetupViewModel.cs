using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class GridSetupViewModel
    {
        public List<string> colNames { get; set; }
        public List<GridColModel> colModel { get; set; }
    }

    public class GridColModel
    {
        public string name {get; set;}
        public string index {get; set;}
        public string align{get; set;}
        public int width {get; set;}
        public bool search {get; set;}
    }
}
