using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oikonomos.web.Helpers
{
    public static class JavascriptHelper
    {
        public static string Content(string path)
        {
            return path.Replace("~",string.Empty) + "?ver=19";
        }
    }
}