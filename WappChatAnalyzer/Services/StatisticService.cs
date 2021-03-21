﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IStatisticService
    {
        public Statistic<int> GetStatistic(IChat chat, Func<IEnumerable<Message>, int> statisticFunc, string statisticName);
    }

    public class StatisticService : IStatisticService
    {
        public Statistic<int> GetStatistic(IChat chat, Func<IEnumerable<Message>, int> statisticFunc, string statisticName)
        {
            var valuesBySendersOnDates = chat.Senders.ToDictionary(o => o, o =>
                chat.Messages.Filter(o).GroupBy(i => i.SentDateNormalized).AsParallel().AsOrdered().Select(i => statisticFunc(i))
                .ToList());
            var totalBySenders = valuesBySendersOnDates.ToDictionary(o => o.Key, o => o.Value.Sum());
            var total = totalBySenders.Values.Sum();

            var result = new Statistic<int>()
            {
                Total = total,
                TotalBySenders = totalBySenders,
                Dates = chat.Messages.Select(o => o.SentDateNormalized).Distinct().OrderBy(o => o).ToList(),
                ValuesBySendersOnDates = valuesBySendersOnDates,
                StatisticName = statisticName
            };

            return result;
        }
    }
}