using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class Statistic
    {
        public string StatisticName { get; set; }

        public Filter Filter { get; set; }

        public Dictionary<int, SenderDTO> Senders { get; set; }

        public float Total { get; set; }
        public Dictionary<int, float> TotalBySenders { get; set; }

        public List<DateTime> TimePeriods { get; set; }
        public Dictionary<int, List<float>> ValuesBySendersOnTimePeriods { get; set; }
        public List<float> TotalsOnTimePeriods { get; set; }
    }
}
