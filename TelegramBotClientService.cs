using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

public class TelegramBotClientService
{
    private TelegramBotClient botClient;

    public TelegramBotClientService(string apiKey) // создаем конструктор 
    {
        botClient = new TelegramBotClient(apiKey);
    }

    public async Task SendTextMessageAsync(ChatId chatId, string text, IReplyMarkup replyMarkup = null) // создаем метод 
    {
        await botClient.SendTextMessageAsync(chatId, text, replyMarkup: replyMarkup); // выполнение метода 
    }
}