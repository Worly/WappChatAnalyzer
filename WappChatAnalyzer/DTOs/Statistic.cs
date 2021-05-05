using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class Statistic<T>
    {
        public string StatisticName { get; set; }

        public Filter Filter { get; set; }

        public T Total { get; set; }
        public Dictionary<string, T> TotalBySenders { get; set; }

        public List<DateTime> TimePeriods { get; set; }
        public Dictionary<string, List<T>> ValuesBySendersOnTimePeriods { get; set; }
    }
}
