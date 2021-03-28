﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private IEventService eventService;
        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet("getEvents")]
        public List<EventInfoDTO> GetEvents([FromQuery] int? skip, [FromQuery] int? take)
        {
            return eventService.GetEvents(skip, take);
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
