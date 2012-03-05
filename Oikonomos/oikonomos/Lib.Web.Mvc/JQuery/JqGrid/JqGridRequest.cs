using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    public class JqGridRequest
    {
        #region Properties
        /// <summary>
        /// Sorting column
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// Sorting direction
        /// </summary>
        public string sord { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Number of rows per page
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// Searching
        /// </summary>
        public bool _search { get; set; }

        /// <summary>
        /// Search field for single searching
        /// </summary>
        public string searchField { get; set; }

        /// <summary>
        /// Search value for single searching
        /// </summary>
        public string searchString { get; set; }

        /// <summary>
        /// Search operator for single searching
        /// </summary>
        public string searchOper { get; set; }

        /// <summary>
        /// Search filters for advanced searching
        /// </summary>
        public JqGridFilters filters { get; set; }
        #endregion
    }
}
