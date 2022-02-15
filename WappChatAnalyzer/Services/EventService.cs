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
        List<Event> GetEvents(int workspaceId, int[] notSelectedGroups, string searchTerm, DateTime? fromDate, DateTime? toDate, int? skip, int? take);
        int GetEventCount(int workspaceId, int[] notSelectedGroups, string searchTerm, DateTime? fromDate, DateTime? toDate);
        List<EventGroupDTO> GetEventGroups();
        Event GetEvent(int id, int workspaceId);
        Event SaveEvent(EventDTO eventDTO, int workspaceId);
        Event AddEvent(EventDTO eventDTO, int workspaceId);
        bool DeleteEvent(int id, int workspaceId);
    }

    public class EventService : IEventService
    {
        private MainDbContext mainDbContext;

        public EventService(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public List<Event> GetEvents(int workspaceId, int[] notSelectedGroups, string searchTerm, DateTime? fromDate, DateTime? toDate, int? skip, int? take)
        {
            IQueryable<Event> query = mainDbContext.Events
                .Where(o => o.WorkspaceId == workspaceId)
                .Include(o => o.EventGroup)
                .OrderByDescending(o => o.DateTime)
                .ThenBy(o => o.Id);

            query = query.Where(o => !notSelectedGroups.Contains(o.EventGroupId));

            if (fromDate != null)
                query = query.Where(o => o.DateTime.Date >= fromDate.Value.Date);
            if (toDate != null)
                query = query.Where(o => o.DateTime.Date <= toDate.Value.Date);

            if (searchTerm != null && searchTerm != "")
            {
                var search = searchTerm.Split(' ').Aggregate((f, s) => f + "* " + s) + "*";
                query = query.Where(o => EF.Functions.Match(o.Name, search, MySqlMatchSearchMode.Boolean));
            }

            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);

            return query.ToList();
        }

        public int GetEventCount(int workspaceId, int[] notSelectedGroups, string searchTerm, DateTime? fromDate, DateTime? toDate)
        {
            IQueryable<Event> query = mainDbContext.Events.Where(o => o.WorkspaceId == workspaceId);

            query = query.Where(o => !notSelectedGroups.Contains(o.EventGroupId));

            if (fromDate != null)
                query = query.Where(o => o.DateTime.Date >= fromDate.Value.Date);
            if (toDate != null)
                query = query.Where(o => o.DateTime.Date <= toDate.Value.Date);

            if (searchTerm != null && searchTerm != "")
            {
                var search = searchTerm.Split(' ').Aggregate((f, s) => f + "* " + s) + "*";
                query = query.Where(o => EF.Functions.Match(o.Name, search, MySqlMatchSearchMode.Boolean));
            }

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

        public Event GetEvent(int id, int workspaceId)
        {
            var ev = mainDbContext.Events
                .Include(o => o.EventGroup)
                .Where(o => o.WorkspaceId == workspaceId)
                .FirstOrDefault(o => o.Id == id);
            return ev;
        }

        public Event SaveEvent(EventDTO eventDTO, int workspaceId)
        {
            var ev = mainDbContext.Events
                .Include(o => o.EventGroup)
                .Where(o => o.WorkspaceId == workspaceId)
                .FirstOrDefault(o => o.Id == eventDTO.Id);

            if (ev == null)
                return null;

            var oldOrder = ev.Order;

            ev.Name = eventDTO.Name;
            ev.Emoji = eventDTO.Emoji;
            ev.EventGroupId = eventDTO.EventGroup.Id;
            ev.DateTime = DateTime.Parse(eventDTO.Date);
            ev.Order = eventDTO.Order;

            if (ev.Order > oldOrder)
            {
                foreach(var evnt in mainDbContext.Events.Where(o => o.WorkspaceId == workspaceId).Where(o => o.DateTime == ev.DateTime && o.Order > oldOrder && o.Order <= ev.Order))
                {
                    if (evnt.Id == ev.Id)
                        continue;

                    evnt.Order--;
                }
            }
            else if (ev.Order < oldOrder)
            {
                foreach (var evnt in mainDbContext.Events.Where(o => o.WorkspaceId == workspaceId).Where(o => o.DateTime == ev.DateTime && o.Order >= ev.Order && o.Order < oldOrder))
                {
                    if (evnt.Id == ev.Id)
                        continue;

                    evnt.Order++;
                }
            }

            mainDbContext.SaveChanges();

            return ev;
        }

        public Event AddEvent(EventDTO eventDTO, int workspaceId)
        {
            var eventGroup = mainDbContext.EventGroups.FirstOrDefault(o => o.Id == eventDTO.EventGroup.Id);
            if (eventGroup == null)
                return null;

            var newEvent = new Event()
            {
                Emoji = eventDTO.Emoji,
                DateTime = DateTime.Parse(eventDTO.Date),
                EventGroup = eventGroup,
                Name = eventDTO.Name,
                WorkspaceId = workspaceId
            };

            var orders = mainDbContext.Events
                .Where(o => o.WorkspaceId == workspaceId)
                .Where(o => o.DateTime == newEvent.DateTime)
                .Select(o => o.Order)
                .ToList();
            newEvent.Order = orders.Count == 0 ? 1 : orders.Max() + 1;

            mainDbContext.Events.Add(newEvent);
            mainDbContext.SaveChanges();

            return newEvent;
        }

        public bool DeleteEvent(int id, int workspaceId)
        {
            var ev = mainDbContext.Events
                .Where(o => o.WorkspaceId == workspaceId)
                .FirstOrDefault(o => o.Id == id);

            if (ev == null)
                return false;

            foreach (var evnt in mainDbContext.Events.Where(o => o.WorkspaceId == workspaceId).Where(o => o.DateTime == ev.DateTime && o.Order > ev.Order))
                evnt.Order--;

            mainDbContext.Events.Remove(ev);
            mainDbContext.SaveChanges();

            return true;
        }
    }
}
