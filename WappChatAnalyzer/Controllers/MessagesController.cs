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

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
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

            var messages = MessageUtils.ParseMessages(lines.ToArray());

            return Ok(messageService.ImportMessages(messages));
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
                Sender = o.Sender,
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
                Sender = message.Sender,
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
                Sender = message.Sender,
                IsMedia = message.IsMedia,
                SentDateTime = message.SentDateTime,
                Text = message.Text
            };
        }

        [HttpGet("getAllSenders")]
        public List<string> GetAllSenders()
        {
            return this.messageService.GetAllSenders();
        }
    }
}
