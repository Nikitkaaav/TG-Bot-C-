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
        string selectedDistrict = "";
        string selectedRestaurantType = "";
        string selectedKitchen = "";
        string selectedPriceRange = "";


        List<string> districts = new List<string> { "Мотовилихинский", "Свердловский", "Индустриальный", "Ленинский", "Орджоникидзевский", "Дзержинский", "Кировский" };
        List<string> restaurants = new List<string> {"Ресторан", "Кафе","Бар/Паб" };
        List<string> prices = new List<string> { "До 500 руб.", "500-1000 руб.", "1000-1500 руб.","От 1500 руб." };
        List<string> kitchens = new List<string> { "Шашлычная", "Восточная", "Европейская" , "Фастфуд", "Веганская", "Грузинская", "Азиатская" , "Американская", "Итальянская", "Японская" };

        var chatId = message.Chat.Id;

        if (message.Text == "/start")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Ресторан"), new KeyboardButton("Кафе") },
            new[] { new KeyboardButton("Бар/Паб")}
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, "В какое заведение вы хотели бы пойти:", replyMarkup: replyMarkup);
        }

        
        else if (restaurants.Contains(message.Text))
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
            new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
            new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
            new[] { new KeyboardButton("Кировский") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);
        }
        else if (districts.Contains(message.Text))
        {

            selectedDistrict = message.Text;
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
             new[] { new KeyboardButton("Шашлычная"), new KeyboardButton("Восточная") },
            new[] { new KeyboardButton("Европейская"), new KeyboardButton("Фастфуд") },
            new[] { new KeyboardButton("Веганская"), new KeyboardButton("Грузинская")},
            new[] { new KeyboardButton("Азиатская"), new KeyboardButton("Американская")},
            new[] { new KeyboardButton("Итальянская"), new KeyboardButton("Японская")}
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали район: {message.Text}. Теперь выберите тип кухни:", replyMarkup: replyMarkup);
        }
        else if (kitchens.Contains(message.Text))
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("До 500 руб."), new KeyboardButton("500-1000 руб.") },
            new[] { new KeyboardButton("1000-1500 руб."), new KeyboardButton("От 1500 руб.") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип кухни: {message.Text}. Теперь выберите ценовую категорию:", replyMarkup: replyMarkup);
        }

        else if (message.Text == "Кировский")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("1"), new KeyboardButton("2") },
            new[] { new KeyboardButton("3"), new KeyboardButton("4") }
        }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип кухни: {message.Text}. Теперь выберите ценовую категорию:", replyMarkup: replyMarkup);
        }

    }
}