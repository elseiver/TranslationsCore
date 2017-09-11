using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCore.Models;

namespace WebCore.Models
{
    public class WebCoreContext : DbContext
    {
        public WebCoreContext (DbContextOptions<WebCoreContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Translation>()
                .Property(b => b.Modify_DT)
                .HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Culture>()
                .Property(b => b.Active)
                .HasDefaultValueSql("(1)");
            modelBuilder.Entity<Culture>()
                .Property(b => b.Default)
                .HasDefaultValueSql("(0)");
        }

        public DbSet<WebCore.Models.Culture> Culture { get; set; }

        public DbSet<WebCore.Models.Translation> Translation { get; set; }
    }
}
