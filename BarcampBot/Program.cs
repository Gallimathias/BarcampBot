using System;
using System.Threading;
using BarcampBot.Runtime;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace BarcampBot
{
    internal class Program
    {
        private static ManualResetEventSlim resetEvent;
        private static Bot bot;
        private static Logger logger;

        private static void Main(string[] args)
        {
            resetEvent = new ManualResetEventSlim(false);
            var config = new LoggingConfiguration();
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new FileTarget("logfile") { FileName = "barcamp.log" });
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new ConsoleTarget("console"));
            LogManager.Configuration = config;
            logger = LogManager.GetCurrentClassLogger();
            logger.Info("Logger is initialized");

            bot = new Bot();

            Console.CancelKeyPress += ConsoleCancelKeyPress;
            logger.Info("Initialization complete....");
            logger.Info("Start bot....");

            bot.Start();
            resetEvent.Wait();
            logger.Info("Bot was terminated. Close program.");
            LogManager.Shutdown();
        }

        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            logger.Info("Stop bot....");
            bot.Stop();            
            resetEvent.Set();
        }
    }
}
