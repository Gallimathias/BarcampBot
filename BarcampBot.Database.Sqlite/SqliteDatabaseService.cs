using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.Database.Sqlite
{
    public class SqliteDatabaseService : BaseDatabaseService
    {
        public SqliteDatabaseService() : base()
        {
        }

        public override Database GetEnsureDatabase(string source)
        {
            RegisterDatabase(new SqliteDatabase().GetEnsureDatabase(source));
            return Database;
        }
    }
}
