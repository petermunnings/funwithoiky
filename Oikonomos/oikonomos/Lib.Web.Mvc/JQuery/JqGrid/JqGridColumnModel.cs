using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    /// <summary>
    /// Exposes jqGrid colModel options
    /// Description for every option: http://www.trirand.com/jqgridwiki/doku.php?id=wiki:colmodel_options
    /// </summary>
    public sealed class JqGridColumnModel
    {
        #region Properties
        public string name { get; set; }

        public string index { get; set; }

        public string align { get; set; }

        public bool sortable { get; set; }

        public int width { get; set; }

        public bool hidden { get; set; }

        public bool resizable { get; set; }

        public bool search { get; set; }

        public string stype { get; set; }

        public JqGridSearchOptions searchoptions { get; set; }
        #endregion

        #region Constructor
        public JqGridColumnModel()
        {
            width = 150;
            sortable = true;
            hidden = false;
            resizable = true;
            search = false;
            searchoptions = null;
        }
        #endregion
    }
}
