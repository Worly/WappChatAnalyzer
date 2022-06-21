using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class EventInfoDTO
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public string GroupName { get; set; }
    }

    public class EventDTO
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public EventGroupDTO EventGroup { get; set; }
    }

    public class EventTemplateDTO
    {
        public string Name { get; set; }
        public string Emoji { get; set; }
        public int? EventGroupId { get; set; }
        /// <summary>
        /// This field is only informational
        /// </summary>
        public string EventGroupName { get; set; }

        public int GetComplexity()
        {
            int c = 0;
            if (Name != null) c++;
            if (Emoji != null) c++;
            if (EventGroupId != null) c++;

            return c;
        }

        public bool IsContainedIn(EventTemplateDTO other)
        {
            if (this.Name != null && this.Name != other.Name)
                return false;

            if (this.Emoji != null && this.Emoji != other.Emoji)
                return false;

            if (this.EventGroupId != null && this.EventGroupId != other.EventGroupId)
                return false;

            return true;
        }
    }

    public class EventGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
