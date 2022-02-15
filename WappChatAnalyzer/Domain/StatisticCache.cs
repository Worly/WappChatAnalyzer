using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WappChatAnalyzer.Domain
{
    [Index("StatisticName", "ForDate", IsUnique = true, Name = "IX_Name_Date")]
    public class StatisticCache
    {
        public int Id { get; set; }
        public string StatisticName { get; set; }
        public DateTime ForDate { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public List<StatisticCacheForSender> ForSenders { get; set; }
    }
}
