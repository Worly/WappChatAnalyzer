using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        private StatisticTotal GetStatisticTotal(StatisticFunc statisticFunc, int workspaceId, DateOnly? from, DateOnly? to)
        {
            var caches = statisticCacheService.GetStatisticCaches(statisticFunc, workspaceId, from, to);
            var senders = messageService.GetAllSenders(workspaceId);
            return new StatisticTotal()
            {
                Total = caches.Sum(o => o.TotalsForSenders.Sum(o => o.Value)),
                Senders = senders.ToDictionary(s => s.Id, s => s.GetDTO()),
                TotalForSenders = senders.ToDictionary(s => s.Id, s => caches.Sum(o => o.TotalsForSenders[s.Id]))
            };
        }

        [HttpGet("getStatisticTotal/numberOfMessages")]
        [SelectedWorkspace]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfMessages([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfMessages(), HttpContext.SelectedWorkspace(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfWords")]
        [SelectedWorkspace]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfWords([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfWords(), HttpContext.SelectedWorkspace(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfCharacters")]
        [SelectedWorkspace]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfCharacters([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfCharacters(), HttpContext.SelectedWorkspace(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfMedia")]
        [SelectedWorkspace]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfMedia([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfMedia(), HttpContext.SelectedWorkspace(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatisticTotal/numberOfEmojis")]
        [SelectedWorkspace]
        public ActionResult<StatisticTotal> GetStatisticTotalNumberOfEmojis([FromQuery] Filter filter)
        {
            return GetStatisticTotal(statisticFuncsService.TotalNumberOfEmojis(), HttpContext.SelectedWorkspace(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getStatistic/numberOfMessages")]
        [SelectedWorkspace]
        public ActionResult<Statistic> GetStatisticNumberOfMessages([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfMessages(), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfWords")]
        [SelectedWorkspace]
        public ActionResult<Statistic> GetStatisticNumberOfWords([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfWords(), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfCharacters")]
        [SelectedWorkspace]
        public ActionResult<Statistic> GetStatisticNumberOfCharacters([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfCharacters(), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfMedia")]
        [SelectedWorkspace]
        public ActionResult<Statistic> GetStatisticNumberOfMedia([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfMedia(), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfEmojis")]
        [SelectedWorkspace]
        public ActionResult<Statistic> GetStatisticNumberOfEmojis([FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfEmojis(), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatistic/numberOfEmoji/{emojiCodePoints}")]
        [SelectedWorkspace]
        public Statistic GetStatisticSingleEmoji(string emojiCodePoints, [FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfEmoji(emojiCodePoints), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatistic/custom/{id}")]
        [SelectedWorkspace]
        public ActionResult<Statistic> GetCustomStatistic(int id, [FromQuery] Filter filter)
        {
            var customStatistic = customStatisticService.GetCustomStatistic(id, HttpContext.SelectedWorkspace());
            if (customStatistic == null)
                return NotFound();

            var result = statisticService.GetStatistic(statisticFuncsService.TotalNumberOfCustom(customStatistic), HttpContext.SelectedWorkspace(), filter);

            return result;
        }

        [HttpGet("getStatisticTotal/custom/{id}")]
        [SelectedWorkspace]
        public ActionResult<StatisticTotal> GetCustomStatisticTotal(int id, [FromQuery] Filter filter)
        {
            var customStatistic = customStatisticService.GetCustomStatistic(id, HttpContext.SelectedWorkspace());
            if (customStatistic == null)
                return NotFound();

            return GetStatisticTotal(statisticFuncsService.TotalNumberOfCustom(customStatistic), HttpContext.SelectedWorkspace(), filter.FromDate, filter.ToDate);
        }

        [HttpGet("getCustomStatistics")]
        [SelectedWorkspace]
        public List<CustomStatisticDTO> GetCustomStatistics()
        {
            return customStatisticService
                .GetCustomStatistics(HttpContext.SelectedWorkspace())
                .Select(o => o.GetDTO())
                .ToList();
        }

        [HttpGet("getCustomStatistic/{id}")]
        [SelectedWorkspace]
        public ActionResult<CustomStatisticDTO> GetCustomStatistics(int id)
        {
            var r = customStatisticService.GetCustomStatistic(id, HttpContext.SelectedWorkspace());
            if (r == null)
                return NotFound();

            return r.GetDTO();
        }

        [HttpPost("saveCustomStatistic")]
        [SelectedWorkspace]
        public ActionResult<CustomStatisticDTO> SaveCustomStatistics([FromBody] CustomStatisticDTO customStatistic)
        {
            var result = customStatisticService.SaveCustomStatistic(customStatistic, HttpContext.SelectedWorkspace());
            if (result == null)
                return NotFound();

            statisticCacheService.ClearCacheFor(result.Name, HttpContext.SelectedWorkspace());

            return result.GetDTO();
        }
    }
}
