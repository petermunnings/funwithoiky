using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using oikonomos.data;

namespace oikonomos.repositories
{
    public class CurrentContext
    {
        private static Dictionary<string, oikonomosEntities> instances;

        public static oikonomosEntities Instance
        {
            get
            {
                if (instances == null)
                    instances = new Dictionary<string, oikonomosEntities>();
                var key = HttpContext.Current==null ? "Test" : HttpContext.Current.Session.SessionID;
                if (!instances.ContainsKey(key))
                    instances.Add(key,new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString));
                return instances[key];
            }
        }

        public static void EndSession(string sessionId)
        {
            try
            {
                if (instances.ContainsKey(sessionId))
                    instances.Remove(sessionId);
            }
            catch
            {
            }

        }
    }
}
