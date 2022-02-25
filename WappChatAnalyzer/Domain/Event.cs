using System;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public int EventGroupId { get; set; }
        public EventGroup EventGroup { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public EventDTO GetDTO()
        {
            return new EventDTO()
            {
                Id = Id,
                Name = Name,
                Date = Date.ToString("yyyy-MM-dd"),
                Order = Order,
                Emoji = Emoji,
                EventGroup = new EventGroupDTO()
                {
                    Id = EventGroup.Id,
                    Name = EventGroup.Name
                }
            };
        }

        public EventInfoDTO GetInfoDTO()
        {
            return new EventInfoDTO()
            {
                Id = Id,
                Name = Name,
                Date = Date.ToString("yyyy-MM-dd"),
                Order = Order,
                Emoji = Emoji,
                GroupName = EventGroup.Name
            };
        }
    }
}
