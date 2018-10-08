using System;
using System.Threading;
using BarcampBot.Runtime;

namespace BarcampBot
{
    internal class Program
    {
        private static ManualResetEventSlim resetEvent;
        private static Bot bot;

        private static void Main(string[] args)
        {
            resetEvent = new ManualResetEventSlim(false);
            bot = new Bot();

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
