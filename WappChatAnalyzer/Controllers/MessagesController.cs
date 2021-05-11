using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
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
            using(var reader = new StreamReader(file.OpenReadStream()))
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
    }
}
