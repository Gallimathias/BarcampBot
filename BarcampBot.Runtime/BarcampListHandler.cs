using BarcampBot.Database;
using BarcampBot.IoC.Locators;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.Runtime
{
    public class BarcampListHandler
    {
        private readonly Logger logger;
        private readonly BaseDatabaseService databaseService;
        private readonly Database.Database database;

        public BarcampListHandler()
        {
            logger = LogManager.GetCurrentClassLogger();
            databaseService = CoreServiceLocator.GetServiceInstance<BaseDatabaseService>();

            if (databaseService == null)
            {
                var error = new NullReferenceException("Can't find any database provider");

                logger.Fatal(error, "Can't initialize a database");
                throw error;
            }

            database = databaseService.GetEnsureDatabase(@"databases\barcamp.db");
        }
    }
}

