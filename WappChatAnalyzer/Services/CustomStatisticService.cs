using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Services
{
    public interface ICustomStatisticService
    {
        public CustomStatistic GetCustomStatistic(int id);
        public List<CustomStatistic> GetCustomStatistics();
        public CustomStatistic SaveCustomStatistic(CustomStatistic customStatistic);
    }

    public class CustomStatisticService : ICustomStatisticService
    {
        private MainDbContext mainDbContext;

        public CustomStatisticService(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public CustomStatistic GetCustomStatistic(int id)
        {
            return mainDbContext.CustomStatistics.FirstOrDefault(o => o.Id == id);
        }

        public List<CustomStatistic> GetCustomStatistics()
        {
            return mainDbContext.CustomStatistics.ToList();
        }

        public CustomStatistic SaveCustomStatistic(CustomStatistic customStatistic)
        {
            if (customStatistic.Id == 0)
            {
                mainDbContext.CustomStatistics.Add(customStatistic);
                mainDbContext.SaveChanges();
                return customStatistic;
            }
            else
            {
                var stat = mainDbContext.CustomStatistics.FirstOrDefault(o => o.Id == customStatistic.Id);
                stat.Name = customStatistic.Name;
                stat.Regex = customStatistic.Regex;
                mainDbContext.SaveChanges();
                return stat;
            }
        }
    }
}
