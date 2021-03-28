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
        List<EventInfoDTO> GetEvents(int? skip, int? take);
        List<EventGroupDTO> GetEventGroups();
        EventDTO GetEvent(int id);
        void SaveEvent(EventDTO eventDTO);
        int AddEvent(EventDTO eventDTO);
        void DeleteEvent(int id);
    }

    public class EventService : IEventService
    {
        private MainDbContext mainDbContext;

        public EventService(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public List<EventInfoDTO> GetEvents(int? skip, int? take)
        {
            IQueryable<Event> query = mainDbContext.Events.Include(o => o.EventGroup).OrderByDescending(o => o.DateTime).ThenBy(o => o.Id);

            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);

            return query.Select(o => new EventInfoDTO()
            {
                Id = o.Id,
                Name = o.Name,
                Date = o.DateTime.ToString("yyyy-MM-dd"),
                Emoji = o.Emoji,
                GroupName = o.EventGroup.Name
            }).ToList();
        }

        public List<EventGroupDTO> GetEventGroups()
        {
            return mainDbContext.EventGroups.Select(o => new EventGroupDTO()
            {
                Id = o.Id,
                Name = o.Name
            }).ToList();
        }

        public EventDTO GetEvent(int id)
        {
            var ev = mainDbContext.Events.Include(o => o.EventGroup).FirstOrDefault(o => o.Id == id);
            return new EventDTO()
            {
                Id = ev.Id,
                Name = ev.Name,
                Date = ev.DateTime.ToString("yyyy-MM-dd"),
                Emoji = ev.Emoji,
                EventGroup = new EventGroupDTO()
                {
                    Id = ev.EventGroup.Id,
                    Name = ev.EventGroup.Name
                }
            };
        }

        public void SaveEvent(EventDTO eventDTO)
        {
            var ev = mainDbContext.Events.Include(o => o.EventGroup).FirstOrDefault(o => o.Id == eventDTO.Id);

            ev.Name = eventDTO.Name;
            ev.Emoji = eventDTO.Emoji;
            ev.EventGroupId = eventDTO.EventGroup.Id;
            ev.DateTime = DateTime.Parse(eventDTO.Date);

            mainDbContext.SaveChanges();
        }

        public int AddEvent(EventDTO eventDTO)
        {
            var newEvent = new Event()
            {
                Emoji = eventDTO.Emoji,
                DateTime = DateTime.Parse(eventDTO.Date),
                EventGroupId = eventDTO.EventGroup.Id,
                Name = eventDTO.Name
            };

            mainDbContext.Events.Add(newEvent);
            mainDbContext.SaveChanges();

            return newEvent.Id;
        }

        public void DeleteEvent(int id)
        {
            var ev = mainDbContext.Events.FirstOrDefault(o => o.Id == id);
            mainDbContext.Events.Remove(ev);
            mainDbContext.SaveChanges();
        }
    }
}
