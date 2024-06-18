using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

public class TelegramBotClientService
{
    private TelegramBotClient botClient;

    public TelegramBotClientService(string apiKey)
    {
        botClient = new TelegramBotClient(apiKey);
    }

    public async Task SendTextMessageAsync(ChatId chatId, string text, IReplyMarkup replyMarkup = null)
    {
        await botClient.SendTextMessageAsync(chatId, text, replyMarkup: replyMarkup);
    }
}