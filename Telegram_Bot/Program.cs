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

            var message = update.Message;
            if (message.Text != null)
            {
                if (message.Text.ToLower().Contains("/start"))
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Чем дальше лес...... скибидидоб ес ес");
                    return;

                }

            }
        }


        private static async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                if (message.Text.ToLower().Contains("/start"))
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Чем дальше лес...... скибидидоб ес ес");
                    return;

                }

            }


        }







    }
}