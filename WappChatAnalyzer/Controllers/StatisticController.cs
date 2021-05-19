using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        private IMessageService messageService;
        private IChatAnalyzerService chatAnalyzerService;
        private IStatisticService statisticService;
        private ICustomStatisticService customStatisticService;

        public StatisticController(IMessageService messageService, IChatAnalyzerService chatAnalyzerService, IStatisticService statisticService, ICustomStatisticService customStatisticService)
        {
            this.messageService = messageService;
            this.chatAnalyzerService = chatAnalyzerService;
            this.statisticService = statisticService;
            this.customStatisticService = customStatisticService;
        }

        private StatisticTotal GetStatisticTotal(IEnumerable<Message> messages, IEnumerable<string> senders, Func<IEnumerable<Message>, int> statisticFunc)
        {
            return new StatisticTotal()
            {
                Total = statisticFunc(messages),
                TotalForSenders = senders.ToDictionary(s => s, s => statisticFunc(messages.AsQueryable().Filter(s)))
            };
        }

        [HttpGet("getStatisticTotal/numberOfMessages")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfMessages([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();

            return GetStatisticTotal(messages, allSenders, chatAnalyzerService.TotalNumberOfMessages);
        }

        [HttpGet("getStatisticTotal/numberOfWords")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfWords([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();

            return GetStatisticTotal(messages, allSenders, chatAnalyzerService.TotalNumberOfWords);
        }

        [HttpGet("getStatisticTotal/numberOfCharacters")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfCharacters([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();

            return GetStatisticTotal(messages, allSenders, chatAnalyzerService.TotalNumberOfCharacters);
        }

        [HttpGet("getStatisticTotal/numberOfMedia")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfMedia([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();

            return GetStatisticTotal(messages, allSenders, chatAnalyzerService.TotalNumberOfMedia);
        }

        [HttpGet("getStatisticTotal/numberOfEmojis")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfEmojis([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();

            return GetStatisticTotal(messages, allSenders, chatAnalyzerService.TotalNumberOfEmojis);
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

        [HttpGet("getStatistic/custom/{id}")]
        public ActionResult<Statistic<int>> GetCustomStatistic(int id, [FromQuery] Filter filter)
        {
            var customStatistic = customStatisticService.GetCustomStatistic(id);

            var regex = new Regex(customStatistic.Regex, RegexOptions.IgnoreCase);

            var result = statisticService.GetStatistic(messageService, o => chatAnalyzerService.TotalNumberOfRegex(regex, o), customStatistic.Name, filter);

            return result;
        }

        [HttpGet("getStatisticTotal/custom/{id}")]
        public StatisticTotal GetCustomStatisticTotal(int id, [FromQuery] Filter filter)
        {
            var customStatistic = customStatisticService.GetCustomStatistic(id);
            var regex = new Regex(customStatistic.Regex, RegexOptions.IgnoreCase);

            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();
            var allSenders = messageService.GetAllSenders();

            return GetStatisticTotal(messages, allSenders, m => chatAnalyzerService.TotalNumberOfRegex(regex, m));
        }

        [HttpGet("getCustomStatistics")]
        public List<CustomStatistic> GetCustomStatistics()
        {
            return customStatisticService.GetCustomStatistics();
        }

        [HttpGet("getCustomStatistic/{id}")]
        public CustomStatistic GetCustomStatistics(int id)
        {
            return customStatisticService.GetCustomStatistic(id);
        }

        [HttpPost("saveCustomStatistic")]
        public CustomStatistic GetCustomStatistics([FromBody] CustomStatistic customStatistic)
        {
            return customStatisticService.SaveCustomStatistic(customStatistic);
        }
    }
}
