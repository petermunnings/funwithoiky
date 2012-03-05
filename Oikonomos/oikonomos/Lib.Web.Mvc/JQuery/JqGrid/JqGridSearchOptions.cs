using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    /// <summary>
    /// Exposes jqGrid colModel search options
    /// Description for every option: http://www.trirand.com/jqgridwiki/doku.php?id=wiki:search_config
    /// </summary>
    public sealed class JqGridSearchOptions
    {
        #region Properties
        public string dataUrl { get; set; }

        public string[] sopt { get; set; }

        public string defaultValue { get; set; }
        #endregion

        #region Constructor
        public JqGridSearchOptions()
        {
            sopt = new string[] { "eq", "ne" };
        }
        #endregion
    }
}
