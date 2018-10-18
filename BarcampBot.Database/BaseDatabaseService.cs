using BarcampBot.IoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.Database
{
    public abstract class BaseDatabaseService : IService
    {
        public Database Database { get; protected set; }

        public BaseDatabaseService()
        {
        }

        public virtual void RegisterDatabase(Database database)
        {
            Database = database;
        }

        public abstract Database GetEnsureDatabase(string source);

    }
}
