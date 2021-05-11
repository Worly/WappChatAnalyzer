using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasicController : ControllerBase
    {
        private IMessageService messageService;
        private IChatAnalyzerService chatAnalyzerService;
        private IStatisticService statisticService;

        public BasicController(IMessageService messageService, IChatAnalyzerService chatAnalyzerService, IStatisticService statisticService)
        {
            this.messageService = messageService;
            this.chatAnalyzerService = chatAnalyzerService;
            this.statisticService = statisticService;
        }

        [HttpGet("getBasicInfoTotal")]
        public BasicInfoTotal GetBasicInfoTotal([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();
            return new BasicInfoTotal()
            {
                BasicInfo = GetBasicInfo(messages),
                BasicInfoForSenders = allSenders.ToDictionary(sender => sender, sender => GetBasicInfo(messages.AsQueryable().Filter(sender)))
            };
        }

        [HttpGet("getStatistic/numberOfMessages")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMessages([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(messageService, chatAnalyzerService.TotalNumberOfMessages, "NumberOfMessages", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfWords")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfWords([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(messageService, chatAnalyzerService.TotalNumberOfWords, "NumberOfWords", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfCharacters")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfCharacters([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(messageService, chatAnalyzerService.TotalNumberOfCharacters, "NumberOfCharacters", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfMedia")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMedia([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(messageService, chatAnalyzerService.TotalNumberOfMedia, "NumberOfMedia", filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfEmojis")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfEmojis([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(messageService, chatAnalyzerService.TotalNumberOfEmojis, "NumberOfEmojis", filter);

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
