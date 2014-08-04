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
            var intCellPhoneNos = ConvertCellNosToInternationalFormat(cellPhoneNos, currentPerson.Church.Country);

            var returnText = string.Empty;
            var completedNos = 0;
            const int reasonableNo = 20;
            while (completedNos < intCellPhoneNos.Count)
            {
                var shortListOfInternationalFormattedNos = intCellPhoneNos.Skip(completedNos).Take(reasonableNo);
                returnText += string.Format(" Batch {0}: {1}    ---", (completedNos/reasonableNo)+1, SendToListOfNumbers(smsText, username, password, shortListOfInternationalFormattedNos, currentPerson.PersonId, currentPerson.ChurchId));
                completedNos += reasonableNo;
            }
            return returnText;
        }

        private static Dictionary<string, string> ConvertCellNosToInternationalFormat(IEnumerable<string> cellPhoneNos, string country)
        {
            var intCellPhoneNos = new Dictionary<string, string>();
            foreach (var cellNo in cellPhoneNos)
            {
                var intCellNo = Utils.ConvertCellPhoneToInternational(cellNo, country);

                if (!intCellPhoneNos.ContainsKey(intCellNo))
                    intCellPhoneNos.Add(intCellNo, cellNo);
            }
            return intCellPhoneNos;
        }

        private string SendToListOfNumbers(string smsText, string username, string password, IEnumerable<KeyValuePair<string, string>> cellPhoneNos, int fromPersonId, int churchId)
        {
            var messageId = _messageRepository.SaveMessage(fromPersonId, smsText, string.Empty, "Sms");
            var responseText = string.Empty;
            var urlString = BuildQueryParams(smsText, username, password, _bulkSmsUrl, cellPhoneNos.Select(k=>k.Key));
            try
            {
                var completeUrl = urlString.ToString();
                var firstTry = completeUrl.Substring(0, completeUrl.Length - 1);
                var response = _httpPostService.HttpSend(firstTry);
                var result = response.Split('|');
                if (result[0].Contains("invalid credentials"))
                {
                    responseText = SendMessageUsingCommunityBulkSms(smsText, username, password, cellPhoneNos, messageId, responseText, churchId);
                }
                else
                {
                    SaveMessage(response, messageId, churchId, cellPhoneNos.Select(k=>k.Value));
                    responseText = response;
                }
            }
            catch (Exception ex)
            {
                responseText += "There was an error sending your Smses: " + ex.Message;
            }
            return responseText;
        }

        private string SendMessageUsingCommunityBulkSms(string smsText, string username, string password, IEnumerable<KeyValuePair<string, string>> cellPhoneNos, int messageId, string responseText, int churchId)
        {
            var urlString = BuildQueryParams(smsText, username, password, _bulkSmsCommunityUrl, cellPhoneNos.Select(k=>k.Key));
            var completeUrl = urlString.ToString();
            var secondTry = completeUrl.Substring(0, completeUrl.Length - 1);
            var response2 = _httpPostService.HttpSend(secondTry);
            SaveMessage(response2, messageId, churchId, cellPhoneNos.Select(k=>k.Value));
            responseText += response2;
            return responseText;
        }

        private void SaveMessage(string response, int messageId, int churchId, IEnumerable<string> standardFormatCellNos)
        {
            var messageStatus = response == "Smses sent succesfully" ? "Success" : "Failure";
            _messageRecepientRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromCellPhoneNos(standardFormatCellNos, churchId), messageStatus, messageStatus == "Success" ? string.Empty : response);
        }

        private static StringBuilder BuildQueryParams(string smsText, 
            string username, 
            string password, 
            string url, 
            IEnumerable<string> internationFormatCellNos)
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
            foreach (var cellPhone in internationFormatCellNos)
            {
                urlString.Append(cellPhone);
                urlString.Append(",");
            }
            return urlString;
        }
    }
}