using System;

namespace oikonomos.common.Models
{
    public class MessageWithStatusViewModel
    {
        public string MessageRecepientId { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Status { get; set; }
        public DateTime Sent { get; set; }
        public string StatusDetail { get; set; }
    }
}
