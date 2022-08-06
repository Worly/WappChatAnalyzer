using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace WappChatAnalyzer.Domain
{
    public class MainDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Sender> Senders { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ImportHistory> ImportHistories { get; set; }
        public DbSet<CustomStatistic> CustomStatistics { get; set; }
        public DbSet<StatisticCache> StatisticCaches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=127.0.0.1;Port=5433;Database=WappChatAnalyzer;Userid=WappChatAnalyzerUser;Password=Mg4gDb9mhWr8HRd92pBT";
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()))
                .EnableSensitiveDataLogging()
                .UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Message.OnModelCreating(modelBuilder);
        }

        public void UpdateDatabase()
        {
            Console.WriteLine("Updating database!");
            Database.Migrate();
        }
    }
}
