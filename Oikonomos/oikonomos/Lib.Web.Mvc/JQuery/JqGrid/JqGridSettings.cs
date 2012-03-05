using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    /// <summary>
    /// Exposes jqGrid options
    /// Description for every option: http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
    /// </summary>
    public sealed class JqGridSettings
    {
        #region Properties
        public string altclass { get; set; }

        public bool altRows { get; set; }

        public bool autowidth { get; set; }

        public string caption { get; set; }

        public bool gridview { get; set; }

        public string url { get; set; }

        public string height { get; set; }

        public bool rownumbers { get; set; }

        public int[] rowList { get; set; }

        public int rowNum { get; set; }

        public string pager { get; set; }

        public bool pgbuttons { get; set; }

        public bool pginput { get; set; }

        public JqGridColumnModel[] colModel { get; set; }

        public string[] colNames { get; set; }

        public int scrollOffset { get; set; }

        public string sortorder { get; set; }

		public string sortname { get; set; }

		public string datatype { get; set; }

        public string mtype { get; set; }

        public bool viewrecords { get; set; }

		public int[] remapColumns { get; set; }
		
		public string width { get; set; }

        public string id { get; set; }
        #endregion

        #region Constructor
        public JqGridSettings()
        {
            altRows = false;
            altclass = "ui-priority-secondary";
            autowidth = false;
            gridview = true;
            sortorder = "asc";
            rownumbers = false;
            rowList = new int[] { };
            rowNum = 20;
            pgbuttons = true;
            pginput = true;
            scrollOffset = 18;
            viewrecords = false;
        }
        #endregion
    }
}
