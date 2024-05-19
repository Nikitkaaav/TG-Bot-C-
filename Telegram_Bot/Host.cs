using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Bot
{
    public class Host
    {
        public Action<ITelegramBotClient, Update>? OnMessage;
        private TelegramBotClient client;

        public Host(string token)
        {
            client = new TelegramBotClient(token);
        }
        
        public void Start()
        {
            client.StartReceiving(UpdateHandler, ErrorHadler);
            Console.WriteLine("Бот хуйня");
        }

        private async Task ErrorHadler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine("Ошибка: " + exception.Message);
            await Task.CompletedTask;
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Console.WriteLine($"{update.Message?.Text ?? "[не текст]"}");
            OnMessage?.Invoke(client, update);
            await Task.CompletedTask;
        }
    }
}
