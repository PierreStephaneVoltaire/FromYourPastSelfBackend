using System;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MailContext:DbContext
    {
        public MailContext(DbContextOptions<MailContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MessageFromPastSelf>(entity => {
                entity.ToTable("messages");
            });
        }
        public DbSet<MessageFromPastSelf> messages { get; set; }
    }
}
