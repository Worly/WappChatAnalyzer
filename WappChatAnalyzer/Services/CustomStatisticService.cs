using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface ICustomStatisticService
    {
        public CustomStatistic GetCustomStatistic(int id, int workspaceId);
        public List<CustomStatistic> GetCustomStatistics(int workspaceId);
        public CustomStatistic SaveCustomStatistic(CustomStatisticDTO customStatistic, int workspaceId);
    }

    public class CustomStatisticService : ICustomStatisticService
    {
        private MainDbContext mainDbContext;

        public CustomStatisticService(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public CustomStatistic GetCustomStatistic(int id, int workspaceId)
        {
            return mainDbContext.CustomStatistics
                .Where(o => o.WorkspaceId == workspaceId)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<CustomStatistic> GetCustomStatistics(int workspaceId)
        {
            return mainDbContext.CustomStatistics.Where(o => o.WorkspaceId == workspaceId).ToList();
        }

        public CustomStatistic SaveCustomStatistic(CustomStatisticDTO customStatistic, int workspaceId)
        {
            if (customStatistic.Id == 0)
            {
                var newStat = new CustomStatistic()
                {
                    Name = customStatistic.Name,
                    Regex = customStatistic.Regex,
                    WorkspaceId = workspaceId
                };

                mainDbContext.CustomStatistics.Add(newStat);
                mainDbContext.SaveChanges();
                return newStat;
            }
            else
            {
                var stat = mainDbContext.CustomStatistics.Where(o => o.WorkspaceId == workspaceId).FirstOrDefault(o => o.Id == customStatistic.Id);
                if (stat == null)
                    return null;

                stat.Name = customStatistic.Name;
                stat.Regex = customStatistic.Regex;
                mainDbContext.SaveChanges();
                return stat;
            }
        }
    }
}
