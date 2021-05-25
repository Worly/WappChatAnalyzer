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

        public Dictionary<int, SenderDTO> Senders { get; set; }

        public T Total { get; set; }
        public Dictionary<int, T> TotalBySenders { get; set; }

        public List<DateTime> TimePeriods { get; set; }
        public Dictionary<int, List<T>> ValuesBySendersOnTimePeriods { get; set; }
    }
}
