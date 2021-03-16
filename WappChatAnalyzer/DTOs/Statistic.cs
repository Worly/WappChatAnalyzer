using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class Statistic<T>
    {
        public string StatisticName { get; set; }

        public T Total { get; set; }
        public Dictionary<string, T> TotalBySenders { get; set; }

        public List<DateTime> Dates { get; set; }
        public Dictionary<string, List<T>> ValuesBySendersOnDates { get; set; }
    }
}
