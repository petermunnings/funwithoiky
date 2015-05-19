using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Timers;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace MailChecker
{
    class Program
    {
        private const string Hostname = "sark.aserv.co.za";
        private const int Port = 995;
        private const bool UseSsl = true;
        private const string Username = "reply@oikonomos.co.za";
        private const string Password = "MQaz3xXwzc";
        private static Timer _aTimer;

        static void Main(string[] args)
        {
            _aTimer = new Timer(20000);
            _aTimer.Elapsed += CheckForNewEmails;
            _aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program... ");
            Console.ReadLine();
            Console.WriteLine("Terminating the application...");

        }

        private static async void CheckForNewEmails(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("{0}: Checking for new messages...", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.oikonomos.co.za/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("api/CheckForEmail");
                    if (!response.IsSuccessStatusCode) return;
                    var responseMessages = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseMessages);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static List<Message> GetNewMessages(Pop3Client pop3Client)
        {
            var allMessages = new List<Message>();

            pop3Client.Connect(Hostname, Port, UseSsl);
            pop3Client.Authenticate(Username, Password);
            var messageCount = pop3Client.GetMessageCount();

            for (var i = messageCount; i > 0; i--)
            {
                allMessages.Add(pop3Client.GetMessage(i));
            }
            return allMessages;
        }
    }
}
