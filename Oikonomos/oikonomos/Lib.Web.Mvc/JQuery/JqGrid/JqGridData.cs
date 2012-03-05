using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    /// <summary>
    /// Represents the data for jqGrid
    /// </summary>
    public sealed class JqGridData
    {
        #region Properties
        /// <summary>
        /// Total pages count
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Total records count
        /// </summary>
        public int records { get; set; }

        /// <summary>
        /// Rows table
        /// </summary>
        public JqGridRow[] rows { get; set; }

        /// <summary>
        /// UserData object
        /// </summary>
        public object userdata { get; set; }
        #endregion
    }
}
