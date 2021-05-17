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
        List<EventInfoDTO> GetEvents(int[] notSelectedGroups, DateTime? fromDate, DateTime? toDate, int? skip, int? take);
        int GetEventCount(int[] notSelectedGroups, DateTime? fromDate, DateTime? toDate);
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

        public List<EventInfoDTO> GetEvents(int[] notSelectedGroups, DateTime? fromDate, DateTime? toDate, int? skip, int? take)
        {
            IQueryable<Event> query = mainDbContext.Events.Include(o => o.EventGroup).OrderByDescending(o => o.DateTime).ThenBy(o => o.Id);

            query = query.Where(o => !notSelectedGroups.Contains(o.EventGroupId));

            if (fromDate != null)
                query = query.Where(o => o.DateTime.Date >= fromDate.Value.Date);
            if (toDate != null)
                query = query.Where(o => o.DateTime.Date <= toDate.Value.Date);

            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);

            return query.Select(o => new EventInfoDTO()
            {
                Id = o.Id,
                Name = o.Name,
                Order = o.Order,
                Date = o.DateTime.ToString("yyyy-MM-dd"),
                Emoji = o.Emoji,
                GroupName = o.EventGroup.Name
            }).ToList();
        }

        public int GetEventCount(int[] notSelectedGroups, DateTime? fromDate, DateTime? toDate)
        {
            IQueryable<Event> query = mainDbContext.Events;

            query = query.Where(o => !notSelectedGroups.Contains(o.EventGroupId));

            if (fromDate != null)
                query = query.Where(o => o.DateTime.Date >= fromDate.Value.Date);
            if (toDate != null)
                query = query.Where(o => o.DateTime.Date <= toDate.Value.Date);

            return query.Count();
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
                Order = ev.Order,
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

            var oldOrder = ev.Order;

            ev.Name = eventDTO.Name;
            ev.Emoji = eventDTO.Emoji;
            ev.EventGroupId = eventDTO.EventGroup.Id;
            ev.DateTime = DateTime.Parse(eventDTO.Date);
            ev.Order = eventDTO.Order;

            if (ev.Order > oldOrder)
            {
                foreach(var evnt in mainDbContext.Events.Where(o => o.DateTime == ev.DateTime && o.Order > oldOrder && o.Order <= ev.Order))
                {
                    if (evnt.Id == ev.Id)
                        continue;

                    evnt.Order--;
                }
            }
            else if (ev.Order < oldOrder)
            {
                foreach (var evnt in mainDbContext.Events.Where(o => o.DateTime == ev.DateTime && o.Order >= ev.Order && o.Order < oldOrder))
                {
                    if (evnt.Id == ev.Id)
                        continue;

                    evnt.Order++;
                }
            }

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

            var orders = mainDbContext.Events.Where(o => o.DateTime == newEvent.DateTime).Select(o => o.Order).ToList();
            newEvent.Order = orders.Count == 0 ? 1 : orders.Max() + 1;

            mainDbContext.Events.Add(newEvent);
            mainDbContext.SaveChanges();

            return newEvent.Id;
        }

        public void DeleteEvent(int id)
        {
            var ev = mainDbContext.Events.FirstOrDefault(o => o.Id == id);

            foreach (var evnt in mainDbContext.Events.Where(o => o.DateTime == ev.DateTime && o.Order > ev.Order))
                evnt.Order--;

            mainDbContext.Events.Remove(ev);
            mainDbContext.SaveChanges();
        }
    }
}
