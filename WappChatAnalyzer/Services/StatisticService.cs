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
        public Statistic GetStatistic(StatisticFunc statisticFunc, int workspaceId, Filter filter);
    }

    public class StatisticService : IStatisticService
    {
        private IStatisticCacheService statisticCacheService;
        private IMessageService messageService;
        private IStatisticFuncsService statisticFuncsService;

        public StatisticService(IStatisticCacheService statisticCacheService, IMessageService messageService, IStatisticFuncsService statisticFuncsService)
        {
            this.statisticCacheService = statisticCacheService;
            this.messageService = messageService;
            this.statisticFuncsService = statisticFuncsService;
        }

        public Statistic GetStatistic(StatisticFunc statisticFunc, int workspaceId, Filter filter)
        {
            if (filter.GroupingPeriod == null)
                filter.GroupingPeriod = "date";
            if (filter.Per == null)
                filter.Per = "none";

            var senders = messageService.GetAllSenders(workspaceId);

            var statistic = Get(statisticFunc, workspaceId, senders, filter.FromDate, filter.ToDate, filter.GroupingPeriod);

            if (filter.Per != "none")
            {
                Statistic perStatistic;
                switch (filter.Per)
                {
                    case "message":
                        perStatistic = this.Get(statisticFuncsService.TotalNumberOfMessages(), workspaceId, senders, filter.FromDate, filter.ToDate, filter.GroupingPeriod);
                        break;
                    case "word":
                        perStatistic = this.Get(statisticFuncsService.TotalNumberOfWords(), workspaceId, senders, filter.FromDate, filter.ToDate, filter.GroupingPeriod);
                        break;
                    case "character":
                        perStatistic = this.Get(statisticFuncsService.TotalNumberOfCharacters(), workspaceId, senders, filter.FromDate, filter.ToDate, filter.GroupingPeriod);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Per cannot be: " + filter.Per);
                }

                Func<float, float, float> calculatePer;
                if (filter.PerReciprocal)
                    calculatePer = (f, s) => f == 0 ? 0 : s / f;
                else
                    calculatePer = (f, s) => s == 0 ? 0 : f / s;

                statistic.Total = calculatePer(statistic.Total, perStatistic.Total);

                foreach (var senderId in statistic.TotalBySenders.Keys.ToList())
                {
                    if (perStatistic.TotalBySenders.TryGetValue(senderId, out float value))
                        statistic.TotalBySenders[senderId] = calculatePer(statistic.TotalBySenders[senderId], value);
                    else
                        statistic.TotalBySenders[senderId] = 0;
                }

                for (int i = 0; i < statistic.TimePeriods.Count; i++)
                {
                    var j = perStatistic.TimePeriods.IndexOf(statistic.TimePeriods[i]);
                    if (j == -1)
                    {
                        foreach (var senderId in statistic.ValuesBySendersOnTimePeriods.Keys)
                            statistic.ValuesBySendersOnTimePeriods[senderId][i] = 0;
                        statistic.TotalsOnTimePeriods[i] = 0;
                    }
                    else
                    {
                        foreach (var senderId in statistic.ValuesBySendersOnTimePeriods.Keys)
                        {
                            if (perStatistic.ValuesBySendersOnTimePeriods.TryGetValue(senderId, out List<float> forPeriods))
                            {
                                var value = forPeriods[j];
                                statistic.ValuesBySendersOnTimePeriods[senderId][i] = calculatePer(statistic.ValuesBySendersOnTimePeriods[senderId][i], value);
                            }
                            else
                                statistic.ValuesBySendersOnTimePeriods[senderId][i] = 0;
                        }

                        statistic.TotalsOnTimePeriods[i] = calculatePer(statistic.TotalsOnTimePeriods[i], perStatistic.TotalsOnTimePeriods[j]);
                    }
                }
            }

            statistic.Filter.GroupingPeriod = filter.GroupingPeriod;

            return statistic;
        }

        private Statistic Get(StatisticFunc statisticFunc, int workspaceId, List<Sender> senders, DateOnly? fromDate, DateOnly? toDate, string groupingPeriod)
        {
            List<DateTime> timePeriods;
            Dictionary<int, List<float>> valuesBySendersOnTimePeriods;

            if (groupingPeriod == "timeOfDay" || groupingPeriod == "hour")
            {
                var filteredMessages = messageService.GetAllMessages(workspaceId).FilterDateRange(fromDate, toDate).ToList();


                Func<Message, DateTime> groupingSelector = null;
                switch (groupingPeriod)
                {
                    case "timeOfDay":
                        groupingSelector = o => new DateTime(2021, 1, 1, o.SentTime.Hour, 0, 0);
                        break;
                    case "hour":
                        groupingSelector = o => new DateTime(o.SentDate.Year, o.SentDate.Month, o.SentDate.Day, o.SentTime.Hour, 0, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Grouping period cannot be: " + groupingPeriod);
                }

                var tp = timePeriods = filteredMessages.GroupBy(groupingSelector).Distinct().Select(o => o.Key).OrderBy(o => o).ToList();

                valuesBySendersOnTimePeriods =
                    senders
                    .ToDictionary(o => o.Id, o => tp
                        .Select(p => filteredMessages
                            .AsQueryable()
                            .Filter(o)
                            .Where(o => groupingSelector(o) == p))
                        .AsParallel()
                        .AsOrdered()
                        .Select(i => (float)statisticFunc.Func(i))
                    .ToList());
            }
            else
            {
                var caches = statisticCacheService.GetStatisticCaches(statisticFunc, workspaceId, fromDate, toDate);

                Func<StatisticCacheDTO, DateTime> groupingSelector = null;
                switch (groupingPeriod)
                {
                    case "date":
                        groupingSelector = o => o.ForDate.ToDateTime(new TimeOnly(0, 0));
                        break;
                    case "week":
                        groupingSelector = o => o.ForDate.AddDays(-(((int)o.ForDate.DayOfWeek + 6) % 7)).ToDateTime(new TimeOnly(0, 0));
                        break;
                    case "month":
                        groupingSelector = o => new DateTime(o.ForDate.Year, o.ForDate.Month, 1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Grouping period cannot be: " + groupingPeriod);
                }

                var tp = timePeriods = caches.AsEnumerable().GroupBy(groupingSelector).Distinct().Select(o => o.Key).OrderBy(o => o).ToList();

                valuesBySendersOnTimePeriods =
                    senders
                    .ToDictionary(o => o.Id, s => timePeriods
                        .Select(p => caches
                            .Where(o => groupingSelector(o) == p)
                            .Sum(o => (float)o.TotalsForSenders[s.Id]))
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
            List<float> totalsOnTimePeriods = new List<float>();
            for (int i = 0; i < timePeriods.Count; i++)
                totalsOnTimePeriods.Add(valuesBySendersOnTimePeriods.Sum(o => o.Value[i]));

            var result = new Statistic()
            {
                Total = total,
                Filter = new Filter()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    GroupingPeriod = groupingPeriod,
                    Per = "none"
                },
                Senders = senders.ToDictionary(s => s.Id, s => s.GetDTO()),
                TotalBySenders = totalBySenders,
                TimePeriods = timePeriods,
                ValuesBySendersOnTimePeriods = valuesBySendersOnTimePeriods,
                TotalsOnTimePeriods = totalsOnTimePeriods,
                StatisticName = statisticFunc.Name
            };

            return result;
        }
    }
}
