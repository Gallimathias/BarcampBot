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
using Telegram.Bot.Types.Enums;
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
            args.TelegramBot.SendChatActionAsync(args.Message.Chat.Id, ChatAction.Typing);

            if (args.Message.Chat.Type == ChatType.Group || args.Message.Chat.Type == ChatType.Supergroup)
                return false;

            var buttons = new List<InlineKeyboardButton[]>();
            var barcamps = databaseService.Database.Barcamps.Where(b => b.Time.To >= DateTime.Now).ToList();

            for (int i = 0; i < barcamps.Count; i++)
            {
                var camp = barcamps[i];
                buttons.Add(new[] {new InlineKeyboardButton()
                {
                    Text = camp.Titel,
                    CallbackData = $"!barcamp id:{camp.Id}"
                }});
            }
            var keyboard = new InlineKeyboardMarkup(buttons);

            args.TelegramBot.SendTextMessageAsync(args.Message.Chat.Id, "Here all Barcamps i have", replyMarkup: keyboard);

            return true;
        }

        [Command("barcamp")]
        public static bool Barcamp(BotCommandArgs args)
        {
            args.TelegramBot.SendChatActionAsync(args.Message.Chat.Id, ChatAction.Typing);
            Barcamp barcamp = null;

            if (args.IsQuery)
            {
                var id = int.Parse(args.QueryData.Split(':')[1]);
                barcamp = databaseService.Database.Barcamps.ToList().FirstOrDefault(b => b.Id == id);
            }
            else
            {
                //TODO: Search
            }

            if (barcamp == null)
                return false;

            var strBuilder = new StringBuilder($"# {barcamp.Titel}");
            strBuilder.AppendLine();
            strBuilder.AppendLine($"__Price:__ {barcamp.Cost.ToString()}");
            strBuilder.AppendLine($"__Location:__ {barcamp.Time.Location}");
            strBuilder.AppendLine();
            strBuilder.AppendLine($"__From:__ {barcamp.Time.From.ToString("d.MM.yyyy")}");

            if (barcamp.Time.To.HasValue)
                strBuilder.AppendLine($"__To:__ {barcamp.Time.To.Value.ToString("d.MM.yyyy")}");

            strBuilder.AppendLine();
            strBuilder.AppendLine(barcamp.Description);

            args.TelegramBot.SendTextMessageAsync(
                args.Message.Chat.Id, 
                strBuilder.ToString(), 
                parseMode: ParseMode.Markdown, 
                replyMarkup: new ReplyKeyboardRemove());

            return true;
        }
    }
}
