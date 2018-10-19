using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BarcampBot.Runtime
{
    public class BotCommandArgs
    {
        public TelegramBotClient TelegramBot { get; }
        public Message Message { get; }
        public string QueryData { get; internal set; }
        public bool IsQuery { get; internal set; }
        public CallbackQuery CallbackQuery { get; internal set; }

        public BotCommandArgs(TelegramBotClient telegramBot, Message message)
        {
            TelegramBot = telegramBot;
            Message = message;
        }
    }
}
