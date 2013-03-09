using System;

namespace oikonomos.common.Models
{
    public class MessageRecepientViewModel
    {
        public int MessageRecepientId { get; set; }
        public int MessageToId { get; set; }
        public string MessageToEmail { get; set; }
        public string MessageToFullName { get; set; }
        public DateTime MessageSent { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MessageToCellNo { get; set; }
        
    }
}