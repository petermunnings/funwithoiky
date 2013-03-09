using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class SmsSender : ISmsSender
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPersonRepository _personRepository;
        private readonly string _bulkSmsUrl;
        private readonly string _bulkSmsCommunityUrl;

        public SmsSender(IMessageRepository messageRepository, IPersonRepository personRepository)
        {
            _messageRepository = messageRepository;
            _personRepository = personRepository;
            _bulkSmsUrl = "http://bulksms.2way.co.za:5567/eapi/submission/send_sms/2/2.0";
            _bulkSmsCommunityUrl = "http://community.bulksms.com:5567/eapi/submission/send_sms/2/2.0";
        }
        
        public string SendSmses(string smsText, IEnumerable<string> cellPhoneNos, string username, string password, Person currentPerson)
        {
            //Must remove the &
            smsText = smsText.Replace("&", "and");
            var intCellPhoneNos = new List<string>();
            foreach (var intNo in cellPhoneNos.Select(cellPhoneNo => Utils.ConvertCellPhoneToInternational(cellPhoneNo, currentPerson.Church.Country)).Where(intNo => !intCellPhoneNos.Contains(intNo)))
            {
                intCellPhoneNos.Add(intNo);
            }

            var urlString = BuildQueryParams(intCellPhoneNos, smsText, username, password, _bulkSmsUrl);
            try
            {
                var messageId = _messageRepository.SaveMessage(currentPerson.PersonId, smsText, string.Empty, "Sms");
                var completeUrl = urlString.ToString();
                var firstTry = completeUrl.Substring(0, completeUrl.Length - 1);
                var response = HttpSend(firstTry);
                var result = response.Split('|');
                if (result[0].Contains("invalid credentials"))
                {
                    if (smsText.Length > 143)
                        smsText = smsText.Substring(0, 143);
                    urlString = BuildQueryParams(intCellPhoneNos, smsText, username, password, _bulkSmsCommunityUrl);
                    completeUrl = urlString.ToString();
                    var secondTry = completeUrl.Substring(0, completeUrl.Length - 1);
                    var response2 = HttpSend(secondTry);
                    SaveMessage(cellPhoneNos, currentPerson, response2, messageId);
                    return response2;
                }
                SaveMessage(cellPhoneNos, currentPerson, response, messageId);
                return response;
            }
            catch (Exception ex)
            {
                return "There was an error sending your Smses: " + ex.Message;
            }
        }

        private void SaveMessage(IEnumerable<string> cellPhoneNos, Person currentPerson, string response, int messageId)
        {
            var messageStatus = response == "Smses sent succesfully" ? "Success" : "Failure";
            _messageRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromCellPhoneNos(cellPhoneNos, currentPerson.ChurchId), messageStatus, messageStatus == "Success" ? string.Empty : response);
        }

        private static StringBuilder BuildQueryParams(IEnumerable<string> cellPhoneNos, string smsText, string username, string password, string url)
        {
            var urlString = new StringBuilder();
            urlString.Append(url);
            urlString.Append(string.Format("?username={0}", username));
            urlString.Append(string.Format("&password={0}", password));
            urlString.Append(string.Format("&message={0}", smsText));
            urlString.Append("&want_report=1");
            urlString.Append("&allow_concat_text_sms=1");
            urlString.Append("&msisdn=");
            foreach (var cellPhone in cellPhoneNos)
            {
                urlString.Append(cellPhone);
                urlString.Append(",");
            }
            return urlString;
        }

        private static string HttpSend(string sHttpInputData)
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