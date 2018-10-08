using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barcamp.Bot.Database
{
    public abstract class Database : DbContext
    {
        public DbSet<Barcamp> Barcamps { get; set; }

        public abstract Database GetEnsureDatabase(string source);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
