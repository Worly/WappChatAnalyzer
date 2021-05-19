using DelegateDecompiler;
using System;
using System.Collections.Generic;
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
        public Statistic<int> GetStatistic(IMessageService messageService, Func<IEnumerable<Message>, int> statisticFunc, string statisticName, Filter filter);
    }

    public class StatisticService : IStatisticService
    {
        public Statistic<int> GetStatistic(IMessageService messageService, Func<IEnumerable<Message>, int> statisticFunc, string statisticName, Filter filter)
        {
            var filteredMessages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();


            if (filter.GroupingPeriod == null)
                filter.GroupingPeriod = "date";
            Func<Message, DateTime> groupingSelector = null;
            switch (filter.GroupingPeriod)
            {
                case "timeOfDay":
                    groupingSelector = o => new DateTime(2021, 1, 1, o.SentDateTime.Hour, 0, 0);
                    break;
                case "hour":
                    groupingSelector = o => new DateTime(o.SentDateTime.Year, o.SentDateTime.Month, o.SentDateTime.Day, o.SentDateTime.Hour, 0, 0);
                    break;
                case "date":
                    groupingSelector = o => o.NormalizedSentDate;
                    break;
                case "week":
                    groupingSelector = o => o.NormalizedSentDate.AddDays(-(((int)o.NormalizedSentDate.DayOfWeek + 6) % 7));
                    break;
                case "month":
                    groupingSelector = o => new DateTime(o.NormalizedSentDate.Year, o.NormalizedSentDate.Month, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Grouping period cannot be: " + filter.GroupingPeriod);
            }

            var timePeriods = filteredMessages.GroupBy(groupingSelector).Distinct().Select(o => o.Key).OrderBy(o => o).ToList();

            var valuesBySendersOnTimePeriods =
                messageService
                .GetAllSenders()
                .ToList()
                .ToDictionary(o => o, o => timePeriods
                    .Select(p => filteredMessages
                        .AsQueryable()
                        .Filter(o)
                        .Where(o => groupingSelector(o) == p))
                    .AsParallel()
                    .AsOrdered()
                    .Select(i => statisticFunc(i))
                .ToList());


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
                TotalBySenders = totalBySenders,
                TimePeriods = timePeriods,
                ValuesBySendersOnTimePeriods = valuesBySendersOnTimePeriods,
                StatisticName = statisticName
            };

            return result;
        }
    }
}
