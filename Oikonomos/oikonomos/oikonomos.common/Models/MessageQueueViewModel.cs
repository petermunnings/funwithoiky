using System.Collections.Generic;
using System.Net.Mail;

namespace oikonomos.common.Models
{
    public class MessageQueueViewModel
    {
        public int MessageRecepientId { get; set; }
        public int MessageToId { get; set; }
        public string MessageToEmail { get; set; }
        public string MessageToFullName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailLogin { get; set; }
        public string EmailPassword { get; set; }
        public int MessageFromId { get; set; }
        public int ChurchId  { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public int MessageId { get; set; }
        public string ChurchName { get; set; }
    }
}