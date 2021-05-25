using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Domain
{
    public class StatisticCache
    {
        public int Id { get; set; }
        public string StatisticName { get; set; }
        public DateTime ForDate { get; set; }
        public List<StatisticCacheForSender> ForSenders { get; set; }
    }
}
