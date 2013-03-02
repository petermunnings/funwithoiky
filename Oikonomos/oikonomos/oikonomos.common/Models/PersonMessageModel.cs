using System;

namespace oikonomos.common.Models
{
    public class PersonMessageModel
    {
        public int PersonId { get; set; }
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MessageType { get; set; }
        public string Status { get; set; }
        public DateTime Sent { get; set; }
        public string StatusDetail { get; set; }
        public string SentBy { get; set; }
    }
}