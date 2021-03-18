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
        public BasicInfoTotal GetBasicInfoTotal()
        {
            return new BasicInfoTotal()
            {
                BasicInfo = GetBasicInfo(chat.Messages),
                BasicInfoForSenders = chat.Messages.Select(o => o.Sender).Distinct().ToDictionary(sender => sender, sender => GetBasicInfo(chat.Messages.Filter(sender)))
            };
        }

        [HttpGet("getStatistic/numberOfMessages")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMessages()
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfMessages, "NumberOfMessages");

            return result;
        }

        [HttpGet("getStatistic/numberOfWords")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfWords()
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfWords, "NumberOfWords");

            return result;
        }

        [HttpGet("getStatistic/numberOfCharacters")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfCharacters()
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfCharacters, "NumberOfCharacters");

            return result;
        }

        [HttpGet("getStatistic/numberOfMedia")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMedia()
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfMedia, "NumberOfMedia");

            return result;
        }

        [HttpGet("getStatistic/numberOfEmojis")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfEmojis()
        {
            var result = statisticService.GetStatistic(chat, chatAnalyzerService.TotalNumberOfEmojis, "NumberOfEmojis");

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
