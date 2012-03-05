using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace oikonomos.data.Services
{
    public class Sms
    {
        private static readonly string bulkSmsUrl="http://bulksms.2way.co.za:5567/eapi/submission/send_sms/2/2.0";

        public static string SendSmses(List<string> cellPhoneNos, string smsText, string username, string password)
        {
            StringBuilder urlString = new StringBuilder();
            urlString.Append(bulkSmsUrl);
            urlString.Append(string.Format("?username={0}", username));
            urlString.Append(string.Format("&password={0}", password));
            urlString.Append(string.Format("&message={0}", smsText));
            urlString.Append("&want_report=1");
            urlString.Append("&msisdn=");
            foreach (string cellPhone in cellPhoneNos)
            {
                urlString.Append(cellPhone);
                urlString.Append(",");
            }
            try
            {
                string completeUrl = urlString.ToString();
                completeUrl = completeUrl.Substring(0, completeUrl.Length - 1);
                return HTTPSend(completeUrl);
            }
            catch (Exception ex)
            {
                return "There was an error sending your Smses: " + ex.Message;
            }
        }

        private static string HTTPSend(string sHTTPInputData)
        {
            string sResult;                         
            HttpWebResponse httpResponse = null;    

            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(sHTTPInputData);
                httpReq.Method = "POST";
                httpResponse = (HttpWebResponse)(httpReq.GetResponse());
                StreamReader input = new StreamReader(httpResponse.GetResponseStream());
                sResult = input.ReadToEnd();

                string[] result = sResult.Split('|');
                if (result[0] == "0" || result[0] == "1")
                {
                    sResult = "Smses sent succesfully";
                }
                else
                {
                    sResult = "There was an error sending the Smses.  Error: " + result[1];
                }
            }

            catch (Exception e)
            {
                sResult = "There was an error sending your Smses: " + e.Message;
            }

            finally
            {
                if (httpResponse != null)
                    httpResponse.Close();
            }
            return sResult;
        }
 
    }
}
