using Microsoft.EntityFrameworkCore;
using System;
namespace Monitor.Models
{
    public class AgentDataContext : DbContext
    {

        public AgentDataContext(DbContextOptions<AgentDataContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentData>()
            .Property(e => e.Issues)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
        public DbSet<AgentData> AgentDataEntities { get; set; }
    }
}
