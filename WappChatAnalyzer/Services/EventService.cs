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
        List<EventDTO> GetEvents(int? skip, int? take);
    }

    public class EventService : IEventService
    {
        private MainDbContext mainDbContext;

        public EventService(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public List<EventDTO> GetEvents(int? skip, int? take)
        {
            IQueryable<Event> query = mainDbContext.Events.Include(o => o.EventGroup).OrderByDescending(o => o.DateTime).ThenBy(o => o.Id);

            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);

            return query.Select(o => new EventDTO()
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
