using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.Database
{
    public abstract class Database : DbContext
    {
        public DbSet<Barcamp> Barcamps { get; set; }

        public Database(DbContextOptions options) : base(options)
        {

        }

        public abstract Database GetEnsureDatabase(string source);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
