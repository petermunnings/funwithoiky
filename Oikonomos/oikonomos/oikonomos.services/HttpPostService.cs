using System;
using System.IO;
using System.Net;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class HttpPostService : IHttpPostService
    {
        public string HttpSend(string sHttpInputData)
        {
            HttpWebResponse httpResponse = null;

            try
            {
                var httpReq = (HttpWebRequest)WebRequest.Create(sHttpInputData);
                httpReq.Method = "POST";
                httpResponse = (HttpWebResponse)(httpReq.GetResponse());
                var responseStream = httpResponse.GetResponseStream();
                if (responseStream == null)
                {
                    return "There was an error sending the Smses.  Did not receive any response from the Sms Service Provider";
                }
                else
                {
                    var input = new StreamReader(responseStream);
                    var sResult = input.ReadToEnd();
                    var result = sResult.Split('|');
                    if (result[0] == "0" || result[0] == "1")
                    {
                        return "Smses sent succesfully";
                    }
                    else
                    {
                        return "There was an error sending the Smses.  Error: " + result[1];
                    }
                }
            }

            catch (Exception e)
            {
                return "There was an error sending your Smses: " + e.Message;
            }

            finally
            {
                if (httpResponse != null)
                    httpResponse.Close();
            }
        }
    }
}