using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class BasicInfoTotal
    {
        public BasicInfo BasicInfo { get; set; }
        public Dictionary<string, BasicInfo> BasicInfoForSenders { get; set; }
    }

    public class BasicInfo
    {
        public int TotalNumberOfMessages { get; set; }
        public int TotalNumberOfWords { get; set; }
        public int TotalNumberOfCharacters { get; set; } 
        public int TotalNumberOfMedia { get; set; }
        public int TotalNumberOfEmojis { get; set; }
    }
}
