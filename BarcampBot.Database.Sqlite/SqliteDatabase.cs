using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.Database.Sqlite
{
    public class SqliteDatabase : Database
    {
        static SqliteDatabase() => SQLitePCL.Batteries.Init();

        public SqliteDatabase(): base()
        {

        }
        public SqliteDatabase(DbContextOptions options) : base(options)
        {
        }

        public override Database GetEnsureDatabase(string source)
        {
            var db = GetDatabase(source);
            db.Database.EnsureCreated();
            return db;
        }

        public static SqliteDatabase GetDatabase(string source)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlite($"Data Source={source}");
            return new SqliteDatabase(builder.Options);
        }
    }
}
