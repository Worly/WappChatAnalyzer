using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class EmojiInfoTotal
    {
        public List<EmojiInfo> EmojiInfos { get; set; }
    }

    public class EmojiInfo
    {
        public string EmojiCodePoints { get; set; }
        public int Total { get; set; }
        public Dictionary<int, SenderDTO> Senders { get; set; }
        public Dictionary<int, int> BySenders { get; set; }
    }
}
