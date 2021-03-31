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
        private IStatisticService statisticService;

        public BasicController(IChat chat, IChatAnalyzerService chatAnalyzerService, IStatisticService statisticService)
        {
            this.chat = chat;
            this.chatAnalyzerService = chatAnalyzerService;
            this.statisticService = statisticService;
        }

        [HttpGet("getBasicInfoTotal")]
        public BasicInfoTotal GetBasicInfoTotal([FromQuery] Filter filter)
        {
            return new BasicInfoTotal()
            {
                BasicInfo = GetBasicInfo(chat.Messages.FilterDateRange(filter.FromDate, filter.ToDate)),
                BasicInfoForSenders = chat.Messages.Select(o => o.Sender).Distinct().ToDictionary(sender => sender, sender => GetBasicInfo(chat.Messages.Filter(sender).FilterDateRange(filter.FromDate, filter.ToDate)))
            };
        }

        [HttpGet("getStatistic/numberOfMessages")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMessages([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfMessages, "NumberOfMessages", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfWords")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfWords([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfWords, "NumberOfWords", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfCharacters")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfCharacters([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfCharacters, "NumberOfCharacters", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfMedia")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMedia([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfMedia, "NumberOfMedia", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfEmojis")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfEmojis([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfEmojis, "NumberOfEmojis", filter);

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
