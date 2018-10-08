using CommandManagementSystem.Attributes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using Telegram.Bot.Types.ReplyMarkups;

namespace Barcamp.Bot.Runtime.Commands
{
    public static class BarcampListCommands
    {
        private static readonly BarcampList barcampList;

        static BarcampListCommands()
        {
            var client = new HttpClient();
            var str = client
                .GetAsync("https://www.barcamp-liste.de/")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result;

            var doc = new HtmlDocument();
            doc.LoadHtml(str);

            barcampList = new BarcampList(doc);
        }

        [Command("all")]
        public static bool All(BotCommandArgs args)
        {
            var buttons = new List<InlineKeyboardButton[]>();

            for (int i = 0; i < barcampList.Count; i++)
            {
                var camp = barcampList[i];
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
