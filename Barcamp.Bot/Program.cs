using Barcamp.Bot.Core;
using System;
using System.Threading;

namespace Barcamp.Bot
{
    internal class Program
    {
        private static ManualResetEventSlim resetEvent;
        private static BarcampBot bot;

        private static void Main(string[] args)
        {
            resetEvent = new ManualResetEventSlim(false);
            bot = new BarcampBot();

            Console.CancelKeyPress += ConsoleCancelKeyPress;
            Console.WriteLine("Initialization complete....");
            Console.WriteLine("Start bot....");

            bot.Start();
            resetEvent.Wait();
        }

        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stop bot....");
            bot.Stop();
            resetEvent.Set();
        }
    }
}
