using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using oikonomos.data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

namespace oikonomos.repositories
{
    public class CurrentContext
    {
        private static Dictionary<string, oikonomosEntities> instances;
        private static string _connectionString;
        private static readonly Mutex updateMutex = new Mutex();

        public static oikonomosEntities Instance
        {
            get
            {
                if (instances == null)
                    instances = new Dictionary<string, oikonomosEntities>();
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                }

                var key = HttpContext.Current==null ? "Test" : HttpContext.Current.Session.SessionID;
                if (!instances.ContainsKey(key))
                {
                    instances.Add(key,new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString));
                    Task.Factory.StartNew(() => SaveKey(key));
                }
                
                if(HttpContext.Current.Session["LoggedOnPerson"]!=null)
                {
                    var person = (Person)HttpContext.Current.Session["LoggedOnPerson"];

                    Task.Factory.StartNew(() => UpdateCurrentUser(key, person.Username, person.Church==null ? string.Empty: person.Church.Name));
                }
                return instances[key];
            }
        }

        public static void EndSession(string sessionId)
        {
            try
            {
                if (!instances.ContainsKey(sessionId)) return;
                Task.Factory.StartNew(() => SaveEndSession(sessionId));
                instances.Remove(sessionId);
            }
            catch
            {
            }
        }

        private static void SaveEndSession(string sessionId)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(string.Format("UPDATE oiky.Session SET SessionEnded = '{0}' WHERE SessionId = '{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), sessionId), con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                //Do nothing - fail gracefully
            }
        }

        private static void SaveKey(string key)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(string.Format("INSERT INTO oiky.Session (SessionId, SessionStarted) VALUES ('{0}', '{1}')", key, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")), con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                //Do nothing - fail gracefully
            }
        }

        private static void UpdateCurrentUser(string key, string userName, string church)
        {
            try
            {
                updateMutex.WaitOne();
                
                using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(string.Format("SELECT UserName FROM oiky.Session WHERE SessionId = '{0}'", key), con))
                    {
                        var userNameExisting = cmd.ExecuteScalar();
                        if (userNameExisting == DBNull.Value)
                        {
                            cmd.CommandText = string.Format("UPDATE oiky.Session SET UserName = '{0}', Church='{1}' WHERE SessionId = '{2}'", userName, church, key);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                updateMutex.ReleaseMutex();
            }
            catch
            {
                //Do nothing - fail gracefully
            }
        }
    }
}
