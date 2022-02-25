using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WappChatAnalyzer.Domain
{
    [Index("StatisticName", "ForDate", "WorkspaceId", IsUnique = true, Name = "IX_Name_Date_Workspace")]
    public class StatisticCache
    {
        public int Id { get; set; }
        public string StatisticName { get; set; }
        public DateOnly ForDate { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public List<StatisticCacheForSender> ForSenders { get; set; }
    }
}
