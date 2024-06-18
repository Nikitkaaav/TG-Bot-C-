using Telegram.Bot.Types.Enums;
using Telegram.Bot;

public partial class Program
{
    private static TelegramBotClient botClient;

    public static async Task Main()
    {
        botClient = new TelegramBotClient("7038593299:AAEkEuD54turzP9lr_xHzqqWSx8-yhAeLac");
        Console.WriteLine("Бот готов к работе");

        int shift = 0;

        while (true)
        {
            var updates = await botClient.GetUpdatesAsync(shift);

            foreach (var update in updates)
            {
                var message = update.Message;

                if (message.Type == MessageType.Text)
                {
                    await SendingMessages.MessageProcessing(message, botClient);
                }

                shift = update.Id + 1;
            }
        }
    }
}