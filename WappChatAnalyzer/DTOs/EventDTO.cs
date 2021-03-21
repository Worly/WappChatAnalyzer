using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public string GroupName { get; set; }
    }
}
