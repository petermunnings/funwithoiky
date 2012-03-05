using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    //Requires custom model binder, because it comes as a JSON string inside POST parameter
    [ModelBinder(typeof(JqGridModelBinder))]
    public sealed class JqGridFilters
    {
        #region Properties
        public string groupOp { get; set; }

        public List<JqGridFilterRule> rules { get; set; }
        #endregion

        #region Constructor
        public JqGridFilters()
        {
            groupOp = "AND";
        }
        #endregion
    }
}
