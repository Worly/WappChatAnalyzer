using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using WappChatAnalyzer.Auth;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EventController : ControllerBase
    {
        private IEventService eventService;
        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet("getEvents")]
        public List<EventInfoDTO> GetEvents([FromQuery] int? skip, [FromQuery] int? take, [FromQuery] string notSelectedGroupsJSON, [FromQuery] string searchTerm, [FromQuery] Filter filter)
        {
            var notSelectedGroups = JsonConvert.DeserializeObject<int[]>(notSelectedGroupsJSON);

            return eventService.GetEvents(notSelectedGroups, searchTerm, filter.FromDate, filter.ToDate, skip, take);
        }

        [HttpGet("getEventCount")]
        public int GetEventCount([FromQuery] string notSelectedGroupsJSON, [FromQuery] string searchTerm, [FromQuery] Filter filter)
        {
            var notSelectedGroups = JsonConvert.DeserializeObject<int[]>(notSelectedGroupsJSON);

            return eventService.GetEventCount(notSelectedGroups, searchTerm, filter.FromDate, filter.ToDate);
        }

        [HttpGet("getEventGroups")]
        public List<EventGroupDTO> GetEventGroups()
        {
            return eventService.GetEventGroups();
        }

        [HttpGet("getEvent/{id}")]
        public EventDTO GetEvent([FromRoute] int id)
        {
            return eventService.GetEvent(id);
        }

        [HttpPost("saveEvent/{id}")]
        public EventDTO SaveEvent([FromBody] EventDTO eventDTO)
        {
            if (eventDTO.Id != 0)
            {
                eventService.SaveEvent(eventDTO);
                return eventService.GetEvent(eventDTO.Id);
            }
            else
            {
                var id = eventService.AddEvent(eventDTO);
                return eventService.GetEvent(id);
            }
        }

        [HttpDelete("deleteEvent/{id}")]
        public void DeleteEvent([FromRoute] int id)
        {
            eventService.DeleteEvent(id);
        }
    }
}
