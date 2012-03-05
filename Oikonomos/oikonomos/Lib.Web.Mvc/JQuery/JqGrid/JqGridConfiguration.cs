using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    /// <summary>
    /// jqGrid configuration container
    /// It can have two properties:
    /// - one for settings
    /// - one for data
    /// </summary>
    public sealed class JqGridConfiguration
    {
        #region Properties
        public JqGridSettings Settings { get; set; }
        #endregion
    }
}
