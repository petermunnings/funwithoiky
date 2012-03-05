using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    public sealed class JqGridFilterRule
    {
        #region Properties
        public string field { get; set; }

        public string op { get; set; }

        public string data { get; set; }
        #endregion
    }
}
