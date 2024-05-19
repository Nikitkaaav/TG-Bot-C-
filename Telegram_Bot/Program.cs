using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Host bot = new Host("7038593299:AAEkEuD54turzP9lr_xHzqqWSx8-yhAeLac");
            bot.Start();
            bot.OnMessage += OnMessage;
            Console.ReadLine();
        }

        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 0, update.Message?.Text ?? "[не текст]");
        }
    }
}