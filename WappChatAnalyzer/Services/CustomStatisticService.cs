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

    }
}
