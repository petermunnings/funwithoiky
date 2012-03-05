using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lib.Web.Mvc.JQuery.TreeView
{
    public class TreeViewNode
    {
        #region Properties
        /// <summary>
        /// Node identifier
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Node text
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Whether node is expanded
        /// </summary>
        public bool expanded { get; set; }

        /// <summary>
        /// Whether node has children
        /// </summary>
        public bool hasChildren { get; set; }

        /// <summary>
        /// CSS classes for node
        /// </summary>
        public string classes { get; set; }

        /// <summary>
        /// Node childrens
        /// </summary>
        public TreeViewNode[] children { get; set; }
        #endregion
    }
}