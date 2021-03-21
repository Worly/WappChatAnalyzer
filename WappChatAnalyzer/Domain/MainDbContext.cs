using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WappChatAnalyzer.Domain
{
    public class MainDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=127.0.0.1;database=WappChatAnalyzer;user=root;password=#MozeSve123");
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
