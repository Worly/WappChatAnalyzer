using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public DateTime SentDateTime { get; set; }
        public SenderDTO Sender { get; set; }
        public string Text { get; set; }
        public bool IsMedia { get; set; }
    }
}
