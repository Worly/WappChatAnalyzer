using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class BasicInfoTotal
    {
        public StatisticTotal TotalNumberOfMessages { get; set; }
        public StatisticTotal TotalNumberOfWords { get; set; }
        public StatisticTotal TotalNumberOfCharacters { get; set; }
        public StatisticTotal TotalNumberOfMedia { get; set; }
        public StatisticTotal TotalNumberOfEmojis { get; set; }
    }

    public class StatisticTotal
    {
        public int Total { get; set; }
        public Dictionary<string, int> TotalForSenders { get; set; }
    }
}
