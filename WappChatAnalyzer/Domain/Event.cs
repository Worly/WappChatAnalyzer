using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public EventGroup EventGroup { get; set; }
    }
}
