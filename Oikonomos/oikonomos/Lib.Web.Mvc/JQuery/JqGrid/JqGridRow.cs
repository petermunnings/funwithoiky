using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    public sealed class JqGridRow
    {
        #region Properties
        /// <summary>
        /// Row identifier
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Table of row cells values
        /// </summary>
        public string[] cell { get; set; }
        #endregion
    }
}
