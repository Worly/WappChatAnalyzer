using DelegateDecompiler;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IStatisticCacheService
    {
        List<StatisticCacheDTO> GetStatisticCaches(StatisticFunc statisticFunc, int workspaceId, DateTime? from, DateTime? to);
        void ClearCacheAfter(DateTime afterInclusive, int workspaceId);
        void ClearCacheFor(string statisticName, int workspaceId);
    }

    public class StatisticCacheService : IStatisticCacheService
    {
        private MainDbContext mainDbContext;
        private IMessageService messageService;

        public StatisticCacheService(MainDbContext mainDbContext, IMessageService messageService)
        {
            this.mainDbContext = mainDbContext;
            this.messageService = messageService;
        }

        public List<StatisticCacheDTO> GetStatisticCaches(StatisticFunc statisticFunc, int workspaceId, DateTime? from, DateTime? to)
        {
            var allMessages = messageService.GetAllMessages(workspaceId).OrderBy(o => o.Id).Decompile();

            DateTime firstMessageDate;
            DateTime lastMessageDate;

            if (from != null)
            {
                var firstMessage = allMessages.FirstOrDefault(o => o.NormalizedSentDate >= from.Value);
                if (firstMessage == null)
                    return new List<StatisticCacheDTO>();

                firstMessageDate = firstMessage.NormalizedSentDate;
            }
            else if (allMessages.Any())
                firstMessageDate = allMessages.FirstOrDefault().NormalizedSentDate;
            else
                return new List<StatisticCacheDTO>();

            if (to != null)
            {
                var lastMessage = allMessages.LastOrDefault(o => o.NormalizedSentDate <= to.Value);
                if (lastMessage == null)
                    return new List<StatisticCacheDTO>();

                lastMessageDate = lastMessage.NormalizedSentDate;
            }
            else
                lastMessageDate = allMessages.LastOrDefault().NormalizedSentDate;

            if (firstMessageDate > lastMessageDate)
                return new List<StatisticCacheDTO>();

            var caches = mainDbContext.StatisticCaches
                .Include(o => o.ForSenders)
                .Where(o => o.WorkspaceId == workspaceId)
                .Where(o => o.StatisticName == statisticFunc.Name)
                .Where(o => o.ForDate >= firstMessageDate && o.ForDate <= lastMessageDate).ToList();

            var senders = messageService.GetAllSenders(workspaceId);

            var days = Math.Round((lastMessageDate - firstMessageDate).TotalDays) + 1;

            if (caches.Count < days)
            {
                var messages = messageService.GetAllMessages(workspaceId).FilterDateRange(firstMessageDate, lastMessageDate).ToList();
                for (int i = 0; i < days; i++)
                {
                    var date = firstMessageDate.AddDays(i);

                    if (caches.Find(o => o.ForDate == date) == null)
                    {
                        var newCache = new StatisticCache()
                        {
                            StatisticName = statisticFunc.Name,
                            ForDate = date,
                            WorkspaceId = workspaceId,
                            ForSenders = senders.Select(sender => new StatisticCacheForSender()
                            {
                                SenderId = sender.Id,
                                Total = statisticFunc.Func(messages.AsQueryable().Filter(sender, date))
                            }).ToList()
                        };
                        mainDbContext.StatisticCaches.Add(newCache);
                        caches.Add(newCache);
                    }
                }

                mainDbContext.SaveChanges();
            }

            var result = caches.Select(o => new StatisticCacheDTO()
            {
                Id = o.Id,
                ForDate = o.ForDate,
                StatisticName = o.StatisticName,
                TotalsForSenders = senders.ToDictionary(s => s.Id, s =>
                {
                    var forSender = o.ForSenders.FirstOrDefault(f => f.Sender == s);
                    if (forSender != null)
                        return forSender.Total;
                    else
                        return 0;
                })
            }).ToList();

            return result;
        }

        public void ClearCacheAfter(DateTime afterInclusive, int workspaceId)
        {
            mainDbContext.StatisticCaches
                .RemoveRange(mainDbContext.StatisticCaches
                    .Where(o => o.WorkspaceId == workspaceId)
                    .Where(o => o.ForDate >= afterInclusive)
                );
            mainDbContext.SaveChanges();
        }

        public void ClearCacheFor(string statisticName, int workspaceId)
        {
            mainDbContext.StatisticCaches
                .RemoveRange(mainDbContext.StatisticCaches
                    .Where(o => o.WorkspaceId == workspaceId)
                    .Where(o => o.StatisticName == statisticName)
                );
            mainDbContext.SaveChanges();
        }
    }
}
