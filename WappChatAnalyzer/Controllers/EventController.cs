using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WappChatAnalyzer.Auth;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;
using WappChatAnalyzer.Services.Workspaces;

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
        [SelectedWorkspace]
        public List<EventInfoDTO> GetEvents([FromQuery] int? skip, [FromQuery] int? take, [FromQuery] string notSelectedGroupsJSON, [FromQuery] string searchTerm, [FromQuery] Filter filter)
        {
            var notSelectedGroups = JsonConvert.DeserializeObject<int[]>(notSelectedGroupsJSON);

            return eventService.GetEvents(HttpContext.SelectedWorkspace(), notSelectedGroups, searchTerm, filter.FromDate, filter.ToDate, skip, take)
                .Select(o => o.GetInfoDTO())
                .ToList();
        }

        [HttpGet("getEventCount")]
        [SelectedWorkspace]
        public int GetEventCount([FromQuery] string notSelectedGroupsJSON, [FromQuery] string searchTerm, [FromQuery] Filter filter)
        {
            var notSelectedGroups = JsonConvert.DeserializeObject<int[]>(notSelectedGroupsJSON);

            return eventService.GetEventCount(HttpContext.SelectedWorkspace(), notSelectedGroups, searchTerm, filter.FromDate, filter.ToDate);
        }

        [HttpGet("getEventGroups")]
        public List<EventGroupDTO> GetEventGroups()
        {
            return eventService.GetEventGroups();
        }

        [HttpGet("getEvent/{id}")]
        [SelectedWorkspace]
        public ActionResult<EventDTO> GetEvent([FromRoute] int id)
        {
            var e = eventService.GetEvent(id, HttpContext.SelectedWorkspace());
            if (e == null)
                return NotFound();

            return e.GetDTO();
        }

        [HttpPost("saveEvent/{id}")]
        [SelectedWorkspace]
        public ActionResult<EventDTO> SaveEvent([FromBody] EventDTO eventDTO)
        {
            if (eventDTO.Id != 0)
            {
                var e = eventService.SaveEvent(eventDTO, HttpContext.SelectedWorkspace());
                if (e == null)
                    return NotFound();
                return e.GetDTO();
            }
            else
            {
                var e = eventService.AddEvent(eventDTO, HttpContext.SelectedWorkspace());
                return e.GetDTO();
            }
        }

        [HttpDelete("deleteEvent/{id}")]
        [SelectedWorkspace]
        public ActionResult DeleteEvent([FromRoute] int id)
        {
            if (!eventService.DeleteEvent(id, HttpContext.SelectedWorkspace()))
                return NotFound();

            return Ok();
        }

        [HttpGet("getTemplates")]
        [SelectedWorkspace]
        public ActionResult<List<EventTemplateDTO>> GetTemplates()
        {
            return eventService.GetTemplates(HttpContext.SelectedWorkspace());
        }
    }
}
