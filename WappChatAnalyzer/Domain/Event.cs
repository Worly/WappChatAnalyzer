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
        public int Order { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public int EventGroupId { get; set; }
        public EventGroup EventGroup { get; set; }
    }
}
