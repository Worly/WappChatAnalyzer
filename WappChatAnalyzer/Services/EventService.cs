using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IEventService
    {
        List<EventDTO> GetEvents();
    }

    public class EventService : IEventService
    {
        private MainDbContext mainDbContext;

        public EventService(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public List<EventDTO> GetEvents()
        {
            return mainDbContext.Events.Include(o => o.EventGroup).Select(o => new EventDTO()
            {
                Id = o.Id,
                Name = o.Name,
                Date = o.DateTime.ToString("yyyy-MM-dd"),
                Emoji = o.Emoji,
                GroupName = o.EventGroup.Name
            }).ToList();
        }
    }
}
