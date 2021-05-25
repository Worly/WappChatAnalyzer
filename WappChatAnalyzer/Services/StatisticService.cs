using DelegateDecompiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IStatisticService
    {
        public Statistic<int> GetStatistic(StatisticFunc statisticFunc, Filter filter);
    }

    public class StatisticService : IStatisticService
    {
        private IStatisticCacheService statisticCacheService;
        private IMessageService messageService;

        public StatisticService(IStatisticCacheService statisticCacheService, IMessageService messageService)
        {
            this.statisticCacheService = statisticCacheService;
            this.messageService = messageService;
        }

        public Statistic<int> GetStatistic(StatisticFunc statisticFunc, Filter filter)
        {
            if (filter.GroupingPeriod == null)
                filter.GroupingPeriod = "date";

            List<DateTime> timePeriods;
            Dictionary<int, List<int>> valuesBySendersOnTimePeriods;

            var senders = messageService.GetAllSenders();

            if (filter.GroupingPeriod == "timeOfDay" || filter.GroupingPeriod == "hour")
            {
                var filteredMessages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();


                Func<Message, DateTime> groupingSelector = null;
                switch (filter.GroupingPeriod)
                {
                    case "timeOfDay":
                        groupingSelector = o => new DateTime(2021, 1, 1, o.SentDateTime.Hour, 0, 0);
                        break;
                    case "hour":
                        groupingSelector = o => new DateTime(o.SentDateTime.Year, o.SentDateTime.Month, o.SentDateTime.Day, o.SentDateTime.Hour, 0, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Grouping period cannot be: " + filter.GroupingPeriod);
                }

                timePeriods = filteredMessages.GroupBy(groupingSelector).Distinct().Select(o => o.Key).OrderBy(o => o).ToList();

                valuesBySendersOnTimePeriods =
                    senders
                    .ToDictionary(o => o.Id, o => timePeriods
                        .Select(p => filteredMessages
                            .AsQueryable()
                            .Filter(o)
                            .Where(o => groupingSelector(o) == p))
                        .AsParallel()
                        .AsOrdered()
                        .Select(i => statisticFunc.Func(i))
                    .ToList());
            }
            else
            {
                var caches = statisticCacheService.GetStatisticCaches(statisticFunc, filter.FromDate, filter.ToDate);

                Func<StatisticCacheDTO, DateTime> groupingSelector = null;
                switch (filter.GroupingPeriod)
                {
                    case "date":
                        groupingSelector = o => o.ForDate;
                        break;
                    case "week":
                        groupingSelector = o => o.ForDate.AddDays(-(((int)o.ForDate.DayOfWeek + 6) % 7));
                        break;
                    case "month":
                        groupingSelector = o => new DateTime(o.ForDate.Year, o.ForDate.Month, 1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Grouping period cannot be: " + filter.GroupingPeriod);
                }

                timePeriods = caches.AsEnumerable().GroupBy(groupingSelector).Distinct().Select(o => o.Key).OrderBy(o => o).ToList();

                valuesBySendersOnTimePeriods =
                    senders
                    .ToDictionary(o => o.Id, s => timePeriods
                        .Select(p => caches
                            .Where(o => groupingSelector(o) == p)
                            .Sum(o => o.TotalsForSenders[s.Id]))
                        .ToList());
            }

            // izbaci sve dane/sate/tjedne/mjesece u kojima ima totalno 0 poruka/rijeci/znakova
            for (int i = 0; i < timePeriods.Count; i++)
            {
                if (valuesBySendersOnTimePeriods.Sum(o => o.Value[i]) == 0)
                {
                    timePeriods.RemoveAt(i);
                    valuesBySendersOnTimePeriods.ForEach(o => o.Value.RemoveAt(i));
                    i--;
                }
            }

            var totalBySenders = valuesBySendersOnTimePeriods.ToDictionary(o => o.Key, o => o.Value.Sum());
            var total = totalBySenders.Values.Sum();

            var result = new Statistic<int>()
            {
                Total = total,
                Filter = filter,
                Senders = senders.ToDictionary(s => s.Id, s => SenderDTO.From(s)),
                TotalBySenders = totalBySenders,
                TimePeriods = timePeriods,
                ValuesBySendersOnTimePeriods = valuesBySendersOnTimePeriods,
                StatisticName = statisticFunc.Name
            };

            return result;
        }
    }
}
