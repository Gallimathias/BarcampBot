using System;
using System.IO;
using System.Threading;
using BarcampBot.Database.Sqlite;
using BarcampBot.IoC.Locators;
using BarcampBot.Runtime;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace BarcampBot
{
    internal class Program
    {
        private static ManualResetEventSlim resetEvent;
        private static BarcampListHandler barcampHandler;
        private static Bot bot;
        private static Logger logger;

        private static void Main(string[] args)
        {
            resetEvent = new ManualResetEventSlim(false);
            InitializeLogger();
            LoadPlugins();

            //Default SQLlite
            CoreServiceLocator.RegisterService<SqliteDatabaseService>();

            //Initialize Bot
            barcampHandler = new BarcampListHandler();
            bot = new Bot();

            Console.CancelKeyPress += ConsoleCancelKeyPress;
            logger.Info("Initialization complete....");
            logger.Info("Start bot....");

            //Run
            bot.Start();
            resetEvent.Wait();
            logger.Info("Bot was terminated. Close program.");
            LogManager.Shutdown();
        }

        private static void LoadPlugins()
        {
            var folder = new DirectoryInfo(@".\plugins");
            if (!folder.Exists)
                folder.Create();

            CoreServiceLocator.RegisterAssemblys(folder.FullName);
        }

        private static void InitializeLogger()
        {
            var config = new LoggingConfiguration();
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new FileTarget("logfile") { FileName = "barcamp.log" });
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new ColoredConsoleTarget("console"));
            LogManager.Configuration = config;
            logger = LogManager.GetCurrentClassLogger();
            logger.Info("Logger is initialized");
        }

        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            logger.Info("Stop bot....");
            bot.Stop();            
            resetEvent.Set();
        }
    }
}
