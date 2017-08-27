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

        public DbSet<WebCore.Models.Culture> Culture { get; set; }

        public DbSet<WebCore.Models.Translation> Translation { get; set; }
    }
}
