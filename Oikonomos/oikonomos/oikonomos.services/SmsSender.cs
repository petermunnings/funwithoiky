using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class SmsSender : ISmsSender
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageRecepientRepository _messageRecepientRepository;
        private readonly IPersonRepository _personRepository;
        private readonly string _bulkSmsUrl;
        private readonly string _bulkSmsCommunityUrl;
        private readonly IHttpPostService _httpPostService;

        public SmsSender(IMessageRepository messageRepository, IMessageRecepientRepository messageRecepientRepository, IPersonRepository personRepository, IHttpPostService httpPostService)
        {
            _messageRepository = messageRepository;
            _messageRecepientRepository = messageRecepientRepository;
            _personRepository = personRepository;
            _httpPostService = httpPostService;
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

            var messageId = _messageRepository.SaveMessage(currentPerson.PersonId, smsText, string.Empty, "Sms");
            var responseText = string.Empty;
            var urlString = BuildQueryParams(intCellPhoneNos, smsText, username, password, _bulkSmsUrl);
            try
            {
                var completeUrl = urlString.ToString();
                var firstTry = completeUrl.Substring(0, completeUrl.Length - 1);
                var response = _httpPostService.HttpSend(firstTry);
                var result = response.Split('|');
                if (result[0].Contains("invalid credentials"))
                {
                    return sendMessageUsingCommunityBulkSms(smsText, cellPhoneNos, username, password, currentPerson, intCellPhoneNos, messageId, responseText);
                }
                SaveMessage(cellPhoneNos, currentPerson, response, messageId);
                return response;
            }
            catch (Exception ex)
            {
                responseText += "There was an error sending your Smses: " + ex.Message;
            }

            return responseText;
        }

        private string sendMessageUsingCommunityBulkSms(string smsText, IEnumerable<string> cellPhoneNos, string username,
                                                        string password, Person currentPerson, IEnumerable<string> intCellPhoneNos,
                                                        int messageId, string responseText)
        {
            var urlString = BuildQueryParams(intCellPhoneNos, smsText, username, password, _bulkSmsCommunityUrl);
            var completeUrl = urlString.ToString();
            var secondTry = completeUrl.Substring(0, completeUrl.Length - 1);
            var response2 = _httpPostService.HttpSend(secondTry);
            SaveMessage(cellPhoneNos, currentPerson, response2, messageId);
            responseText += response2;
            return responseText;
        }

        private void SaveMessage(IEnumerable<string> cellPhoneNos, Person currentPerson, string response, int messageId)
        {
            var messageStatus = response == "Smses sent succesfully" ? "Success" : "Failure";
            _messageRecepientRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromCellPhoneNos(cellPhoneNos, currentPerson.ChurchId), messageStatus, messageStatus == "Success" ? string.Empty : response);
        }

        private static StringBuilder BuildQueryParams(
            IEnumerable<string> cellPhoneNos, 
            string smsText, 
            string username, 
            string password, 
            string url)
        {
            var urlString = new StringBuilder();
            urlString.Append(url);
            urlString.Append(string.Format("?username={0}", username));
            urlString.Append(string.Format("&password={0}", password));
            urlString.Append(string.Format("&message={0}", smsText));
            
           urlString.Append("&allow_concat_text_sms=1");
           urlString.Append("&concat_text_sms_max_parts=10");
           urlString.Append("&want_report=1");
            
            urlString.Append("&msisdn=");
            foreach (var cellPhone in cellPhoneNos)
            {
                urlString.Append(cellPhone);
                urlString.Append(",");
            }
            return urlString;
        }
    }
}