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
        private IStatisticFuncsService statisticFuncsService;
        private IStatisticService statisticService;
        private ICustomStatisticService customStatisticService;
        private IStatisticCacheService statisticCacheService;

        public StatisticController(IMessageService messageService, IStatisticFuncsService chatAnalyzerService, IStatisticService statisticService, ICustomStatisticService customStatisticService, IStatisticCacheService statisticCacheService)
        {
            this.messageService = messageService;
            this.statisticFuncsService = chatAnalyzerService;
            this.statisticService = statisticService;
            this.customStatisticService = customStatisticService;
            this.statisticCacheService = statisticCacheService;
        }

        private StatisticTotal GetStatisticTotal(StatisticFunc statisticFunc, DateTime? from, DateTime? to)
        {
            var caches = statisticCacheService.GetStatisticCaches(statisticFunc, from, to);
            var senders = messageService.GetAllSenders();
            return new StatisticTotal()
            {
                Total = caches.Sum(o => o.TotalsForSenders.Sum(o => o.Value)),
                Senders = senders.ToDictionary(s => s.Id, s => SenderDTO.From(s)),
                TotalForSenders = senders.ToDictionary(s => s.Id, s => caches.Sum(o => o.TotalsForSenders[s.Id]))
            };
        }

        [HttpGet("getStatisticTotal/numberOfMessages")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfMessages([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfMessages(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfWords")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfWords([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfWords(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfCharacters")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfCharacters([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfCharacters(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfMedia")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfMedia([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfMedia(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfEmojis")]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfEmojis([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfEmojis(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatistic/numberOfMessages")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMessages([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfMessages(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfWords")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfWords([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfWords(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfCharacters")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfCharacters([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfCharacters(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfMedia")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfMedia([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfMedia(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfEmojis")]
        public ActionResult<Statistic<int>> GetStatisticNumberOfEmojis([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfEmojis(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfEmoji/{emojiCodePoints}")]
        public Statistic<int> GetStatisticSingleEmoji(string emojiCodePoints, [FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfEmoji(emojiCodePoints), filter);

            return result;
        }

        [HttpGet("getStatistic/custom/{id}")]
        public ActionResult<Statistic<int>> GetCustomStatistic(int id, [FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfCustom(id), filter);

            return result;
        }

        [HttpGet("getStatisticTotal/custom/{id}")]
        public StatisticTotal GetCustomStatisticTotal(int id, [FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfCustom(id), filter.FromDate, filter.ToDate);
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
        public CustomStatistic SaveCustomStatistics([FromBody] CustomStatistic customStatistic)
        {
            var result = customStatisticService.SaveCustomStatistic(customStatistic);
            statisticCacheService.ClearCacheFor(result.Name);
            return result;
        }
    }
}
