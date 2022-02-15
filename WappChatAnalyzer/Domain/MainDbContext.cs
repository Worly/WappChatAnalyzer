﻿using Microsoft.EntityFrameworkCore;
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
            var connectionString = "server=127.0.0.1;database=WappChatAnalyzer;user=root;password=#MozeSve123";
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()))
                .EnableSensitiveDataLogging()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
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
