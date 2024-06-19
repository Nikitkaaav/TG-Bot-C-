using Telegram.Bot.Types.Enums;
using Telegram.Bot;

public partial class Program
{
    private static TelegramBotClient botClient;

    public static async Task Main() // точка входа 
    {
        botClient = new TelegramBotClient("7038593299:AAEkEuD54turzP9lr_xHzqqWSx8-yhAeLac");// токен телеграм бота 
        Console.WriteLine("Бот готов к работе");// печать сообщения о том, что бот запущен и готов к работе 

        int shift = 0;

        while (true)
        {
            var updates = await botClient.GetUpdatesAsync(shift);// получение обновлений 

            foreach (var update in updates)// обрабатываем каждое обновление 
            {
                var message = update.Message;// получаем сообщение от каждого обновления 

                if (message.Type == MessageType.Text)// проверяем, что тип являет текстовым 
                {
                    await SendingMessages.MessageProcessing(message, botClient); // вызываем метод из класса SendingMessage
                }

                shift = update.Id + 1;
            }
        }
    }
}