using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class StatisticTotal
    {
        public int Total { get; set; }
        public Dictionary<string, int> TotalForSenders { get; set; }
    }
}
