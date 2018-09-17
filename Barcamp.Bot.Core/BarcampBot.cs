using CommandManagementSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Barcamp.Bot.Core
{
    public class BarcampBot
    {
        private readonly DefaultCommandManager<string, BotCommandArgs, bool> manager;
        private readonly TelegramBotClient telegramBot;

        public BarcampBot()
        {
            manager = new DefaultCommandManager<string, BotCommandArgs, bool>(GetType().Namespace + ".Commands");
            telegramBot = new TelegramBotClient(File.ReadAllText("Telegram_Token").Trim());

            telegramBot.OnMessage += TelegramBotOnMessage;
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

            var command = e.Message.Text.Split().FirstOrDefault(s => s.StartsWith("/"))?.Trim().TrimStart('/');

            if (string.IsNullOrWhiteSpace(command))
                return;

            manager.DispatchAsync(command, new BotCommandArgs(telegramBot, e.Message));
        }
    }
}
