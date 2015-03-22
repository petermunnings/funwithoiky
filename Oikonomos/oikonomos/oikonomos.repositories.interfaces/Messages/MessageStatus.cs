using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.repositories.interfaces.Messages
{
    public static class MessageStatus
    {
        public static string Success { get { return "Success"; } }
        public static string Queued { get { return "Queued"; } }
        public static string Failed { get { return "Failed"; } }
    }
}
