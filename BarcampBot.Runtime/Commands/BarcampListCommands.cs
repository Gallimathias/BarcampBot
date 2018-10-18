using BarcampBot.Database;
using BarcampBot.IoC.Locators;
using CommandManagementSystem.Attributes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using Telegram.Bot.Types.ReplyMarkups;

namespace BarcampBot.Runtime.Commands
{
    public static class BarcampListCommands
    {
        private static readonly BaseDatabaseService databaseService;

        static BarcampListCommands()
        {
            databaseService = CoreServiceLocator.GetServiceInstance<BaseDatabaseService>();
        }

        [Command("all")]
        public static bool All(BotCommandArgs args)
        {
            var buttons = new List<InlineKeyboardButton[]>();
            var barcamps = databaseService.Database.Barcamps.Where(b => b.Time.To >= DateTime.Now).ToList();

            for (int i = 0; i < barcamps.Count; i++)
            {
                var camp = barcamps[i];
                buttons.Add(new[] {new InlineKeyboardButton()
                {
                    Text = camp.Titel,
                    CallbackData = $"!barcamp id:{i}"
                }});
            }
            var keyboard = new InlineKeyboardMarkup(buttons);

            args.TelegramBot.SendTextMessageAsync(args.Message.Chat.Id, "Here all Barcamps i have", replyMarkup: keyboard);

            return true;
        }

        [Command("barcamp")]
        public static bool Barcamp(BotCommandArgs args)
        {
            if (args.IsQuery)
            {
                var id = int.Parse(args.QueryData.Split(':')[1]);
            }

            return true;
        }
    }
}
