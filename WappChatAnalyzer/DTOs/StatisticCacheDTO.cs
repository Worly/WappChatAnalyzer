using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.DTOs
{
    public class StatisticCacheDTO
    {
        public int Id { get; set; }
        public string StatisticName { get; set; }
        public DateTime ForDate { get; set; }
        public Dictionary<int, int> TotalsForSenders { get; set; }
    }
}
