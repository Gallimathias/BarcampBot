using BarcampBot.Database;
using BarcampBot.IoC.Locators;
using CommandManagementSystem;
using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace BarcampBot.Runtime
{
    public class Bot
    {
        private readonly Logger logger;
        private readonly DefaultCommandManager<string, BotCommandArgs, bool> manager;
        private readonly TelegramBotClient telegramBot;

        public Bot()
        {
            logger = LogManager.GetCurrentClassLogger();
            manager = new DefaultCommandManager<string, BotCommandArgs, bool>(GetType().Namespace + ".Commands");
            var fileInfo = new FileInfo("Telegram_Token");
            if (!fileInfo.Exists)
            {
                var error = new FileNotFoundException("Can't find telegram token file. Bot cannot be initialized.");
                error.Data.Add("Path", fileInfo.FullName);

                logger.Fatal(error, "Can't find telegram token file. Bot cannot be initialized.");
                throw error;
            }

            telegramBot = new TelegramBotClient(File.ReadAllText(fileInfo.FullName).Trim());

            telegramBot.OnMessage += TelegramBotOnMessage;
            telegramBot.OnInlineQuery += TelegramBotOnInlineQuery;
            telegramBot.OnCallbackQuery += TelegramBotOnCallbackQuery;

        }

        private void TelegramBotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var data = e.CallbackQuery.Data;
            var index = data.IndexOf(' ');
            var command = data.Substring(0, index).TrimStart('!');
            var args = data.Substring(index, data.Length - index).Trim();

            manager.DispatchAsync(command, new BotCommandArgs(telegramBot, e.CallbackQuery.Message)
            {
                IsQuery = true,
                QueryData = args
            });
        }

        private void TelegramBotOnInlineQuery(object sender, InlineQueryEventArgs e)
        {
            var error = new NotSupportedException("InlineQuery is currently not supported");
            error.Data.Add("From", e.InlineQuery.From.Username);
            error.Data.Add("Query", e.InlineQuery.Query);
            logger.Fatal(error, "InlineQuery is currently not supported");
            throw error;
        }

        public void Start()
        {
            telegramBot.StartReceiving();
        }

        public void Stop()
        {
            telegramBot.StopReceiving();
        }

        private void TelegramBotOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type != MessageType.Text)
                return;

            var command = e.Message
                .Text
                .Split()
                .FirstOrDefault(s => s.StartsWith("/"))?
                .Trim()
                .TrimStart('/')
                .ToLower();

            if (string.IsNullOrWhiteSpace(command))
                return;

            manager.DispatchAsync(command, new BotCommandArgs(telegramBot, e.Message));
        }
    }
}
