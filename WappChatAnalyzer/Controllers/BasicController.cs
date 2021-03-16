using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasicController : ControllerBase
    {
        private IChat chat;
        private IChatAnalyzerService chatAnalyzerService;

        public BasicController(IChat chat, IChatAnalyzerService chatAnalyzerService)
        {
            this.chat = chat;
            this.chatAnalyzerService = chatAnalyzerService;
        }

        [HttpGet("getBasicInfoTotal")]
        public BasicInfoTotal GetBasicInfoTotal()
        {
            return new BasicInfoTotal()
            {
                BasicInfo = GetBasicInfo(chat.Messages),
                BasicInfoForSenders = chat.Messages.Select(o => o.Sender).Distinct().ToDictionary(sender => sender, sender => GetBasicInfo(chat.Messages.Filter(sender)))
            };
        }

        [HttpGet("getStatistic")]
        public ActionResult<Statistic<int>> GetStatistic([FromQuery] string statisticName)
        { 
            Func<IEnumerable<Message>, int> statisticFunc = null;
            switch (statisticName)
            {
                case "NumberOfMessages":
                    statisticFunc = chatAnalyzerService.TotalNumberOfMessages;
                    break;
                case "NumberOfWords":
                    statisticFunc = chatAnalyzerService.TotalNumberOfWords;
                    break;
                case "NumberOfCharacters":
                    statisticFunc = chatAnalyzerService.TotalNumberOfCharacters;
                    break;
                case "NumberOfMedia":
                    statisticFunc = chatAnalyzerService.TotalNumberOfMedia;
                    break;
                case "NumberOfEmojis":
                    statisticFunc = chatAnalyzerService.TotalNumberOfEmojis;
                    break;
            }

            if (statisticFunc == null)
                return NotFound();

            var valuesBySendersOnDates = chat.Senders.ToDictionary(o => o, o =>
                chat.Messages.Filter(o).GroupBy(i => i.SentDateNormalized).AsParallel().AsOrdered().Select(i => statisticFunc(i))
                .ToList());
            var totalBySenders = valuesBySendersOnDates.ToDictionary(o => o.Key, o => o.Value.Sum());
            var total = totalBySenders.Values.Sum();

            var result = new Statistic<int>()
            {
                StatisticName = statisticName,
                Total = total,
                TotalBySenders = totalBySenders,
                Dates = chat.Messages.Select(o => o.SentDateNormalized).Distinct().OrderBy(o => o).ToList(),
                ValuesBySendersOnDates = valuesBySendersOnDates
            };

            return result;
        }

        private BasicInfo GetBasicInfo(IEnumerable<Message> messages)
        {
            return new BasicInfo()
            {
                TotalNumberOfMessages = chatAnalyzerService.TotalNumberOfMessages(messages),
                TotalNumberOfWords = chatAnalyzerService.TotalNumberOfWords(messages),
                TotalNumberOfCharacters = chatAnalyzerService.TotalNumberOfCharacters(messages),
                TotalNumberOfMedia = chatAnalyzerService.TotalNumberOfMedia(messages),
                TotalNumberOfEmojis = chatAnalyzerService.TotalNumberOfEmojis(messages)
            };
        }
    }
}
