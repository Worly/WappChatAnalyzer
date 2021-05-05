using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IStatisticService
    {
        public Statistic<int> GetStatistic(IChat chat, Func<IEnumerable<Message>, int> statisticFunc, string statisticName, Filter filter);
    }

    public class StatisticService : IStatisticService
    {
        public Statistic<int> GetStatistic(IChat chat, Func<IEnumerable<Message>, int> statisticFunc, string statisticName, Filter filter)
        {
            var filteredMessages = chat.Messages.FilterDateRange(filter.FromDate, filter.ToDate);


            if (filter.GroupingPeriod == null)
                filter.GroupingPeriod = "date";
            Func<Message, DateTime> groupingSelector = null;
            switch (filter.GroupingPeriod)
            {
                case "hour":
                    groupingSelector = o => new DateTime(o.SentDateTime.Year, o.SentDateTime.Month, o.SentDateTime.Day, o.SentDateTime.Hour, 0, 0);
                    break;
                case "date":
                    groupingSelector = o => o.SentDateNormalized;
                    break;
                case "week":
                    groupingSelector = o => o.SentDateNormalized.AddDays(-(((int)o.SentDateNormalized.DayOfWeek + 6) % 7));
                    break;
                case "month":
                    groupingSelector = o => new DateTime(o.SentDateNormalized.Year, o.SentDateNormalized.Month, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Grouping period cannot be: " + filter.GroupingPeriod);
            }

            var timePeriods = filteredMessages.GroupBy(groupingSelector).Distinct().Select(o => o.Key).ToList();

            var valuesBySendersOnTimePeriods = chat.Senders.ToDictionary(o => o, o =>
                timePeriods.Select(p => filteredMessages.Filter(o).Where(o => groupingSelector(o) == p)).AsParallel().AsOrdered().Select(i => statisticFunc(i))
                .ToList());


            // izbaci sve dane/sate/tjedne/mjesece u kojima ima totalno 0 poruka/rijeci/znakova
            for(int i = 0; i < timePeriods.Count; i++)
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
