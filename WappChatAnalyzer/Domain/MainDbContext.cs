using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace WappChatAnalyzer.Domain
{
    public class MainDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ImportHistory> ImportHistories { get; set; }
        public DbSet<CustomStatistic> CustomStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=127.0.0.1;database=WappChatAnalyzer;user=root;password=#MozeSve123";
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()))
                .EnableSensitiveDataLogging()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public void UpdateDatabase()
        {
            Console.WriteLine("Updating database!");
            Database.Migrate();
        }
    }
}
