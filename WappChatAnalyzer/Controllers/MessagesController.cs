using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult UploadChatExport(IFormFile file)
        {
            if (file == null)
                return BadRequest("file is null");

            var lines = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    lines.Add(reader.ReadLine());
            }

            var senders = messageService.GetAllSenders();
            var messages = MessageUtils.ParseMessages(lines.ToArray(), senders);

            int importedMessagesCount = messageService.ImportMessages(messages, out Message firstImportedMessage);
            statisticCacheService.ClearCacheAfter(firstImportedMessage.NormalizedSentDate);

            return Ok(importedMessagesCount);
        }

        [HttpGet("getImportHistory")]
        public List<ImportHistory> GetImportHistories()
        {
            return messageService.GetImportHistory();
        }

        [HttpGet("getLastMessageId")]
        public int GetLastMessageId()
        {
            return messageService.GetAllMessages().OrderBy(o => o.Id).LastOrDefault().Id;
        }

        [HttpGet("getMessages")]
        public List<MessageDTO> GetMessages(int fromId, int toId)
        {
            if (fromId > toId)
            {
                var temp = fromId;
                fromId = toId;
                toId = temp;
            }

            return messageService.GetMessages(fromId, toId).Select(o => new MessageDTO()
            {
                Id = o.Id,
                Sender = SenderDTO.From(o.Sender),
                SentDateTime = o.SentDateTime,
                Text = o.Text,
                IsMedia = o.IsMedia
            }).ToList();
        }

        [HttpGet("getFirstMessageOfDayBefore")]
        public MessageDTO GetFirstMessageOfDayBefore(DateTime dateTime)
        {
            var message = this.messageService.GetFirstMessageOfDayBefore(dateTime);
            if (message == null)
                return null;
            return new MessageDTO()
            {
                Id = message.Id,
                Sender = SenderDTO.From(message.Sender),
                IsMedia = message.IsMedia,
                SentDateTime = message.SentDateTime,
                Text = message.Text
            };
        }

        [HttpGet("getFirstMessageOfDayAfter")]
        public MessageDTO GetFirstMessageOfDayAfter(DateTime dateTime)
        {
            var message = this.messageService.GetFirstMessageOfDayAfter(dateTime);
            if (message == null)
                return null;
            return new MessageDTO()
            {
                Id = message.Id,
                Sender = SenderDTO.From(message.Sender),
                IsMedia = message.IsMedia,
                SentDateTime = message.SentDateTime,
                Text = message.Text
            };
        }

        [HttpGet("getAllSenders")]
        public List<SenderDTO> GetAllSenders()
        {
            return this.messageService.GetAllSenders().Select(o => SenderDTO.From(o)).ToList();
        }
    }
}
