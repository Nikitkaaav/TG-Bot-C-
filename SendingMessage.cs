using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
partial class SendingMessages
{
    private static TelegramBotClient botClient;
    private static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    private static SheetsService service;

    public static async Task MessageProcessing(Message message, TelegramBotClient botClient)
    {
        var GoogleFileJson = "google_key.json";

        GoogleCredential credential;


        using (var stream = new FileStream(GoogleFileJson, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream);

            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(Scopes);
            }
        }

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });
        string range = "Лист2!A1:Z300";
        string spreadsheetId = "1Bm6xziI9e4HptwMd3xU0PuixlWTC323RQDSQP9qsD-g";

        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);// запрос для получения данных из таблицы по данному id
        var chatId = message.Chat.Id; // получение идентификатора telegram чата 
        string savedFile = "saved_data.txt";
        // Типы заведений
        if (message.Text == "Бар/Паб")
        {
            try
            {
                var answer = request.Execute();
                var values = answer.Values;

                if (values != null && values.Count > 0)
                {


                    System.IO.File.WriteAllText(savedFile, string.Empty); // очистить файл перед записью

                    foreach (var line in values)
                    {
                        if (line.Contains("Тип заведения: Бар/Паб."))
                        {
                            bool NewLine = true;
                            string answerText = string.Join("\t", line);
                            string[] sentences = answerText.Split('.'); // Разбиваем текст на строки после каждой точки
                            string answerTextFinal = string.Join(".\r\n", sentences);
                            foreach (var value in line) // поиск по точкам 
                            {
                                if (NewLine && value == ".")
                                {
                                    answerText += "\r\n";
                                }

                                answerText += value + "\t";

                                if (value == ".")
                                {
                                    NewLine = true;
                                }
                                else
                                {
                                    NewLine = false;
                                }
                            }

                            System.IO.File.AppendAllText(savedFile, answerTextFinal + "\r\n\r\n");

                        }


                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данного заведения не найдено.");
                }
            }

            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        if (message.Text == "Кафе")
        {
            try
            {
                var answer = request.Execute();
                var values = answer.Values;

                if (values != null && values.Count > 0)
                {


                    System.IO.File.WriteAllText(savedFile, string.Empty); // Очистить файл перед записью

                    foreach (var line in values)
                    {
                        if (line.Contains("Тип заведения: Кафе."))
                        {
                            bool NewLine = true;
                            string answerText = string.Join("\t", line);
                            string[] sentences = answerText.Split('.'); // Разбиваем текст на строки после каждой точки
                            string answerTextFinal = string.Join(".\r\n", sentences);
                            foreach (var value in line) // поиск по точкам 
                            {
                                if (NewLine && value == ".")
                                {
                                    answerText += "\r\n";
                                }

                                answerText += value + "\t";

                                if (value == ".")
                                {
                                    NewLine = true;
                                }
                                else
                                {
                                    NewLine = false;
                                }
                            }
                            System.IO.File.AppendAllText(savedFile, answerTextFinal + "\r\n\r\n");
                        }
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данного заведения не найдено.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        if (message.Text == "Ресторан")
        {
            try
            {
                var answer = request.Execute();
                var values = answer.Values;

                if (values != null && values.Count > 0)
                {


                    System.IO.File.WriteAllText(savedFile, string.Empty); // Очистить файл перед записью

                    foreach (var line in values)
                    {
                        if (line.Contains("Тип заведения: Ресторан."))
                        {
                            bool NewLine = true;
                            string answerText = string.Join("\t", line);
                            string[] sentences = answerText.Split('.'); // Разбиваем текст на строки после каждой точки
                            string answerTextFinal = string.Join(".\r\n", sentences);
                            foreach (var value in line) // поиск по точкам 
                            {
                                if (NewLine && value == ".")
                                {
                                    answerText += "\r\n";
                                }

                                answerText += value + "\t";

                                if (value == ".")
                                {
                                    NewLine = true;
                                }
                                else
                                {
                                    NewLine = false;
                                }
                            }
                            System.IO.File.AppendAllText(savedFile, answerTextFinal + "\r\n\r\n");
                        }
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данного заведения не найдено.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        //Кухни
        else if (message.Text == "Быстрое питание") // ПроВерка на Быстрое питание
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Быстрое питание."))
                        {
                            adding = true;

                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Восточная")        // ПроВерка на Восточная
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Восточная."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }
                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Итальянская") // ПроВерка на Итальянская
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Итальянская."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}"); // Обработка исключения
            }
        }
        else if (message.Text == "Азиатская") // ПроВерка на Азиатская
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Азиатская."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Русская") // ПроВерка на Европейская
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Русская."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Европейская") // ПроВерка на Европейская
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Европейская."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}"); // Обработка исключения
            }
        }
        else if (message.Text == "Грузинская") // ПроВерка на Грузинская
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Грузинская."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        //Районы
        else if (message.Text == "Свердловский")     // ПроВерка на Свердловский 
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Свердловский."))
                        {

                            adding = true;
                            filteredRecords.Add(record);
                        }
                    }


                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Ленинский") // ПроВерка на Ленинский
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Ленинский."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Мотовилихинский") // ПроВерка на Мотовилиха
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Мотовилихинский."))
                        {

                            adding = true;
                            filteredRecords.Add(record);
                        }
                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);




                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Индустриальный") // ПроВерка на Индустриальный
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Индустриальный."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}"); // Обработка исключения
            }
        }
        else if (message.Text == "Орджоникидзевский") // ПроВерка на Орджоникидзевский
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Орджоникидзевский."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}"); // Обработка исключения
            }
        }
        else if (message.Text == "Дзержинский") // ПроВерка на Дзержинский
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Дзержинский."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Кировский") // ПроВерка на Кировский
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Кировский."))
                        {
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        // Цены
        else if (message.Text == "До 500 руб.") // ПроВерка на до 500р
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;
                    bool existence = false;
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: До 500 руб."))
                        {
                            existence = true;
                            adding = true;
                            filteredRecords.Add(record);
                        }



                    }

                    if (!existence)
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                        // это вывод, того, что нашел бот в таблице
                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal);
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "500-1000 руб.") // ПроВерка на 500-1000 руб.
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;
                    bool existence = false;
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: 500-1000 руб."))
                        {
                            adding = true;
                            existence = true;
                            filteredRecords.Add(record);
                        }
                    }


                    if (!existence)
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                        // это вывод, того, что нашел бот в таблице
                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal);
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "1000-1500 руб.") // ПроВерка на 1000-1500 руб.
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;
                    bool existence = false;
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: 1000-1500 руб."))
                        {
                            adding = true;
                            existence = true;
                            filteredRecords.Add(record);
                        }
                    }
                    if (!existence)
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                        // это вывод, того, что нашел бот в таблице
                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal);
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "От 1500 руб.") // ПроВерка на от 1500 руб.
        {
            try
            {
                if (System.IO.File.Exists(savedFile))
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> filteredRecords = new List<string>();
                    bool adding = false;
                    bool existence = false;
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: От 1500 руб."))
                        {
                            adding = true;
                            existence = true;
                            filteredRecords.Add(record);
                        }


                    }

                    if (!existence)
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);
                        // это вывод, того, что нашел бот в таблице
                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal);
                    }


                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данные для фильтрации отсутствуют.");
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }

        List<string> districts = new List<string> { "Мотовилихинский", "Свердловский", "Индустриальный", "Ленинский", "Орджоникидзевский", "Дзержинский", "Кировский" };
        List<string> restaurants = new List<string> { "Ресторан", "Кафе", "Бар/Паб" };
        List<string> prices = new List<string> { "До 500 руб.", "500-1000 руб.", "1000-1500 руб.", "От 1500 руб." };
        List<string> kitchens = new List<string> { "Восточная", "Европейская", "Быстрое питание", "Грузинская", "Азиатская", "Итальянская", "Русская" };

        if (message.Text == "/start" || message.Text == "Начать заново")
        {

            var replyMarkup = new ReplyKeyboardMarkup(new[]
           {
             new[] { new KeyboardButton("Ресторан"), new KeyboardButton("Кафе") },
             new[] { new KeyboardButton("Бар/Паб")},
             new[] { new KeyboardButton("Начать заново")},
         }, resizeKeyboard: true);
            await botClient.SendTextMessageAsync(chatId, "Привет,уважаемый пользователь я бот,который поможет тебе найти заведение по твоим интересам в городе Перми.Тебе будут придложены кнопки, благодаря которым ты сможешь выбрать завидение", replyMarkup: replyMarkup);
            await botClient.SendTextMessageAsync(chatId, "В какое заведение вы хотели бы пойти:", replyMarkup: replyMarkup);
        }
        else if (message.Text == "Ресторан")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {new[] { new KeyboardButton("Русская"), new KeyboardButton("Восточная") },
             new[] { new KeyboardButton("Европейская"), new KeyboardButton("Быстрое питание") },
             new[] { new KeyboardButton("Азиатская"),new KeyboardButton("Итальянская")},
             new[] { new KeyboardButton("Грузинская") },
             new[] { new KeyboardButton("Начать заново")},

         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип кухни: {message.Text}. Теперь выберите тип кухни:", replyMarkup: replyMarkup);
        }

        else if (message.Text == "Бар/Паб")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
           {
             new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
             new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
             new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
             new[] { new KeyboardButton("Кировский") },
             new[] { new KeyboardButton("Начать заново")},
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);

        }

        else if (message.Text == "Кафе")
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
             new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
             new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
             new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
             new[] { new KeyboardButton("Кировский") },
             new[] { new KeyboardButton("Начать заново")},
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);
        }

        else if (kitchens.Contains(message.Text))
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
             new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
             new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
             new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
             new[] { new KeyboardButton("Кировский") },
             new[] { new KeyboardButton("Начать заново")},
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);
        }
        else if (districts.Contains(message.Text))
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
              new[] { new KeyboardButton("До 500 руб."), new KeyboardButton("500-1000 руб.") },
             new[] { new KeyboardButton("1000-1500 руб."), new KeyboardButton("От 1500 руб.") },
             new[] { new KeyboardButton("Начать заново")},
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали район: {message.Text}. Теперь выберите ценовую категорию:", replyMarkup: replyMarkup);
        }
    }
}