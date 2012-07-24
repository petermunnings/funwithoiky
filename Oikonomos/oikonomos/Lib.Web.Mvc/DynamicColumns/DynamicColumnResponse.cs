using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Web.Mvc.DynamicColumns
{
    public class DynamicColumnResponse
    {
        public List<ColumnModel> ColumnModel              { get; set; }
        public List<Dictionary<string, object>> RowValues { get; set; }
        public string Message                             { get; set; }
    }

    

}
