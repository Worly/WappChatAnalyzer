using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Domain
{
    public class StatisticCacheForSender
    {
        public int Id { get; set; }
        public int StatisticCacheId { get; set; }
        public int SenderId { get; set; }
        public Sender Sender { get; set; }
        public int Total { get; set; }
    }
}
