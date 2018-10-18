using BarcampBot.Database;
using BarcampBot.IoC.Locators;
using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BarcampBot.Runtime
{
    public class BarcampListHandler : IDisposable
    {
        private readonly Logger logger;
        private readonly BaseDatabaseService databaseService;
        private readonly Database.Database database;
        private readonly HttpClient client;

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

            var folder = new DirectoryInfo(@".\databases");

            if (!folder.Exists)
                folder.Create();

            database = databaseService.GetEnsureDatabase(Path.Combine(folder.FullName, "barcamp.db"));
            client = new HttpClient();
        }


        public void RefreshList()
        {
            var str = client
                .GetAsync("https://www.barcamp-liste.de/")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result;

            var doc = new HtmlDocument();
            doc.LoadHtml(str);

            var barcampList = new BarcampList(doc);

            database.Barcamps.RemoveRange(database.Barcamps);
            database.Barcamps.AddRange(barcampList);
            database.SaveChanges();
        }

        public void Dispose()
        {
            database.SaveChanges();
        }
    }
}

