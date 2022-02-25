using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WappChatAnalyzer.Auth;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;
using WappChatAnalyzer.Services.Workspaces;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private IMessageService messageService;
        private IStatisticCacheService statisticCacheService;

        public MessagesController(IMessageService messageService, IStatisticCacheService statisticCacheService)
        {
            this.messageService = messageService;
            this.statisticCacheService = statisticCacheService;
        }

        [HttpPost("uploadChatExport")]
        [SelectedWorkspace]
        public ActionResult<int> UploadChatExport(IFormFile file)
        {
            if (file == null)
                return BadRequest("file is null");

            var lines = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    lines.Add(reader.ReadLine());
            }

            var senders = messageService.GetAllSenders(HttpContext.SelectedWorkspace());
            var messages = MessageUtils.ParseMessages(lines.ToArray(), senders);

            int importedMessagesCount = messageService.ImportMessages(HttpContext.SelectedWorkspace(), messages, out Message firstImportedMessage);
            statisticCacheService.ClearCacheAfter(firstImportedMessage.NormalizedSentDate, HttpContext.SelectedWorkspace());

            return Ok(importedMessagesCount);
        }

        [HttpGet("getImportHistory")]
        [SelectedWorkspace]
        public List<ImportHistoryDTO> GetImportHistories()
        {
            return messageService
                .GetImportHistory(HttpContext.SelectedWorkspace())
                .Select(o => o.GetDTO())
                .ToList();
        }

        [HttpGet("getLastMessageId")]
        [SelectedWorkspace]
        public int GetLastMessageId()
        {
            return messageService.GetAllMessages(HttpContext.SelectedWorkspace()).OrderBy(o => o.Id).LastOrDefault().Id;
        }

        [HttpGet("getMessages")]
        [SelectedWorkspace]
        public List<MessageDTO> GetMessages(int fromId, int toId)
        {
            if (fromId > toId)
            {
                var temp = fromId;
                fromId = toId;
                toId = temp;
            }

            return messageService
                .GetMessages(HttpContext.SelectedWorkspace(), fromId, toId)
                .Select(o => o.GetDTO())
                .ToList();
        }

        [HttpGet("getFirstMessageOfDayBefore")]
        [SelectedWorkspace]
        public MessageDTO GetFirstMessageOfDayBefore(DateOnly date)
        {
            var message = this.messageService.GetFirstMessageOfDayBefore(HttpContext.SelectedWorkspace(), date);
            if (message == null)
                return null;

            return message.GetDTO();
        }

        [HttpGet("getFirstMessageOfDayAfter")]
        [SelectedWorkspace]
        public MessageDTO GetFirstMessageOfDayAfter(DateOnly date)
        {
            var message = this.messageService.GetFirstMessageOfDayAfter(HttpContext.SelectedWorkspace(), date);
            if (message == null)
                return null;

            return message.GetDTO();
        }

        [HttpGet("getAllSenders")]
        [SelectedWorkspace]
        public List<SenderDTO> GetAllSenders()
        {
            return this.messageService
                .GetAllSenders(HttpContext.SelectedWorkspace())
                .Select(o => o.GetDTO())
                .ToList();
        }
    }
}
