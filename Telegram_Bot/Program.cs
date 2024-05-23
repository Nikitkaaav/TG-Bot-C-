using System;
using Telegram.Bot;
using Telegram.Bot.Types;

using Telegram.Bot.Types.ReplyMarkups;
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
            

            
                if (message.Text.ToLower().Contains("/start"))
                {

                var keyboard = new InlineKeyboardMarkup(new[]
                {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("1", "button_click"),
                    

                }
            }) ;

                // Действие при нажатии на кнопку "1"
               

                await client.SendTextMessageAsync(message.Chat.Id, "Какое действие хотите выбрать ?", replyMarkup: keyboard);
            }
            /*client.OnСallbackQuery += async (sender, callbackQueryEventArgs) =>
            {
                var callbackQuery = callbackQueryEventArgs.CallbackQuery;

                if (callbackQuery.Data == "button_click")
                {
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Вы выбрали действие 1! 🎉");
                }
            };*/



        }




    }


}


        







    
