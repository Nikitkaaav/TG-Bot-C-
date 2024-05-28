using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    private static TelegramBotClient botClient;

    static async Task Main()
    {
        botClient = new TelegramBotClient("7038593299:AAEkEuD54turzP9lr_xHzqqWSx8-yhAeLac");

        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Hello, my name is {me.FirstName}");

        int offset = 0;

        while (true)
        {
            var updates = await botClient.GetUpdatesAsync(offset);

            foreach (var update in updates)
            {
                var message = update.Message;

                if (message.Type == MessageType.Text)
                {
                    await BotOnMessageReceived(message);
                }

                offset = update.Id + 1;
            }
        }
    }

    private static async Task BotOnMessageReceived(Message message)
    {
        var chatId = message.Chat.Id;

        if (message.Text == "/start")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("До 500 руб."), new KeyboardButton("500-1000 руб.") },
            new[] { new KeyboardButton("1000-1500 руб."), new KeyboardButton("От 1500 руб.") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, "Выберите ценовую категорию заведения:", replyMarkup: replyMarkup);
        }
        else if (message.Text == "До 500 руб." || message.Text == "500-1000 руб." || message.Text == "1000-1500 руб." || message.Text == "От 1500 руб.")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
            new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
            new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
            new[] { new KeyboardButton("Кировский") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали ценовую категорию: {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);
        }
        else if (message.Text == "Мотовилихинский" || message.Text == "Свердловский" || message.Text == "Индустриальный" || message.Text == "Ленинский" || message.Text == "Орджоникидзевский" || message.Text == "Дзержинский" || message.Text == "Кировский")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Ресторан"), new KeyboardButton("Кафе") },
            new[] { new KeyboardButton("Бар") , new KeyboardButton("Кафе") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали район: {message.Text}. Теперь выберите тип заведения:", replyMarkup: replyMarkup);
        }
        else
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Итальянская"), new KeyboardButton("Японская") },
            new[] { new KeyboardButton("Французская"), new KeyboardButton("Русская") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения: {message.Text}. Теперь выберите тип кухни:", replyMarkup: replyMarkup);
        }
    }
}