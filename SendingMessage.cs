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
using System.Net;
using static System.Net.Mime.MediaTypeNames;
partial class SendingMessages // класс, отвечающий за обработку и отправку сообщений, а так же за создание кнопок
{
    
    private static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly }; // обьявляем массив строк,который будет содержать область доступа к гугл таблице(Google Sheets)
    private static SheetsService service; // поле, которое создает сервис для работы с гугл таблицей(Google Sheets API)

    public static async Task MessageProcessing(Message message, TelegramBotClient botClient)// метод, который обрабатывает запросы, отправляемые пользователем из Телеграм бота,а так же создает кнопки в Телеграм бота
    {
        var GoogleFileJson = "google_key.json"; // файл JSON для работы с гугл таблицей

        GoogleCredential credential;


        using (var stream = new FileStream(GoogleFileJson, FileMode.Open, FileAccess.Read)) // чтение JSON файла
        {
            credential = GoogleCredential.FromStream(stream);

            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(Scopes);
            }
        }

        service = new SheetsService(new BaseClientService.Initializer()// созданием инициализатор http ддя работы с API гугл таблицы
        {
            HttpClientInitializer = credential
        });
        string range = "Лист2!A1:Z300"; // задаем диапазон в гугл таблице из которого будет браться информация
        string spreadsheetId = "1Bm6xziI9e4HptwMd3xU0PuixlWTC323RQDSQP9qsD-g";

        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);// создаем запрос для получения данных из таблицы по данному id
        var chatId = message.Chat.Id; // получением идентификатор телеграм чата 
        string savedFile = "data.txt"; // задаем имя файла в который будет записываться информация из таблицы      

        // Типы заведений
        if (message.Text == "Кафе") // ПроВерка на Кафе, если введенное сообщение - Кафе
        {
           try { 
                var answer = request.Execute(); // делаем запрос к гугл таблице
                var values = answer.Values;// получаем значения из гугл таблицы

                if (values != null && values.Count > 0)
                {
                    System.IO.File.WriteAllText(savedFile, string.Empty); // очищаем файл перед записью

                    foreach (var line in values) // перебираем все значения, полученные из гугл таблицы
                    {
                        if (line.Contains("Тип заведения: Кафе.")) // проверяем содержит ли данную информацию
                        {
                          
                            bool NewLine = true; // создаем переменную для отступов 
                            string answerText = string.Join("\t", line); // создаем строки с разделением
                            string[] sentences = answerText.Split('.'); // разбиваем  на строки после каждой точки
                            string answerTextFinal = string.Join(".\r\n", sentences); // все обьеденяем разделяя точкой 

                            foreach (var value in line) // поиск по точкам, для того чтобы при выводе сообщений это не выглядело как сплощной текст 
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

                            System.IO.File.AppendAllText(savedFile, answerTextFinal + "\r\n\r\n"); // записываем все в файл 
                        }
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данного заведения не найдено."); // добавляем исколючение если нет такого заведения 
                }
        }

            catch (Exception ex) // обрабатываем исключения 
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
    }
        if (message.Text == "Бар/Паб") // ПроВерка на Бар/Паб, работает так же как и проверка на Кафе
        {
            try
            {
                var answer = request.Execute(); // делаем запрос к гугл таблице
                var values = answer.Values;// получаем значения из гугл таблицы

                if (values != null && values.Count > 0)
                {
                    System.IO.File.WriteAllText(savedFile, string.Empty); // очищаем файл перед записью

                    foreach (var line in values) // перебираем все значения, полученные из гугл таблицы
                    {
                        if (line.Contains("Тип заведения: Бар/Паб.")) // проверяем содержит ли данную информацию
                        {

                            bool NewLine = true; // создаем переменную для отступов 
                            string answerText = string.Join("\t", line); // создаем строки с разделением
                            string[] sentences = answerText.Split('.'); // разбиваем  на строки после каждой точки
                            string answerTextFinal = string.Join(".\r\n", sentences); // все обьеденяем разделяя точкой 

                            foreach (var value in line) // поиск по точкам, для того чтобы при выводе сообщений это не выглядело как сплощной текст 
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

                            System.IO.File.AppendAllText(savedFile, answerTextFinal + "\r\n\r\n"); // записываем все в файл 
                        }
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данного заведения не найдено."); // добавляем исколючение если нет такого заведения 
                }
            }

            catch (Exception ex) // обрабатываем исключения 
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        if (message.Text == "Ресторан") // ПроВерка на Ресторан , работает так же как и проверка на Кафе
        {
            try
            {
                var answer = request.Execute(); // делаем запрос к гугл таблице
                var values = answer.Values;// получаем значения из гугл таблицы

                if (values != null && values.Count > 0)
                {
                    System.IO.File.WriteAllText(savedFile, string.Empty); // очищаем файл перед записью

                    foreach (var line in values) // перебираем все значения, полученные из гугл таблицы
                    {
                        if (line.Contains("Тип заведения: Ресторан.")) // проверяем содержит ли данную информацию
                        {

                            bool NewLine = true; // создаем переменную для отступов 
                            string answerText = string.Join("\t", line); // создаем строки с разделением
                            string[] sentences = answerText.Split('.'); // разбиваем  на строки после каждой точки
                            string answerTextFinal = string.Join(".\r\n", sentences); // все обьеденяем разделяя точкой 

                            foreach (var value in line) // поиск по точкам, для того чтобы при выводе сообщений это не выглядело как сплощной текст 
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

                            System.IO.File.AppendAllText(savedFile, answerTextFinal + "\r\n\r\n"); // записываем все в файл 
                        }
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данного заведения не найдено."); // добавляем исколючение если нет такого заведения 
                }
            }

            catch (Exception ex) // обрабатываем исключения 
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        
        //Кухни
        
        else if (message.Text == "Быстрое питание") // ПроВерка на Быстрое питание
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Быстрое питание.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Восточная")        // ПроВерка на Восточная
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Восточная.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Итальянская") // ПроВерка на Итальянская
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Итальянская.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Азиатская") // ПроВерка на Азиатская
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Азиатская.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Русская") // ПроВерка на Русская
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Русская.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Европейская") // ПроВерка на Европейская
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Европейская.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Грузинская") // ПроВерка на Грузинская
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Кухня: Грузинская.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        
        //Районы
        
        else if (message.Text == "Свердловский")     // ПроВерка на Свердловский 
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Свердловский.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Ленинский") // ПроВерка на Ленинский
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Ленинский.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Мотовилихинский") // ПроВерка на Мотовилихинский
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Мотовилихинский.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Индустриальный") // ПроВерка на Индустриальный
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Индустриальный.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Орджоникидзевский") // ПроВерка на Орджоникидзевский
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Орджоникидзевский.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Дзержинский") // ПроВерка на Дзержинский
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Дзержинский.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        else if (message.Text == "Кировский") // ПроВерка на Кировский
        {
            try
            {
                if (System.IO.File.Exists(savedFile)) // проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile); // читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>(); // создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false; // добавляем переменную для отследивания процесса добавления записей 

                    foreach (var record in records)
                    {
                        if (record.Contains("Район: Кировский.")) // проверяем содержится ли данная информация 
                        {
                            adding = true;

                            filteredRecords.Add(record); // добавляем в отпильтрованную запись 
                        }
                    }
                    string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords); // все обьединяем и разделяем 
                    System.IO.File.WriteAllText(savedFile, filteredRecordsFinal); // записываем все в файл 
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Данная информация отсутствует."); // если отсутствует информация 
                }
            }
            catch (Exception ex) // обработка исключений
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");// Обработка исключения
            }
        }
        
        // Цены
        
        else if (message.Text == "До 500 руб.") // ПроВерка на до 500р
        {
            try
            {
                if (System.IO.File.Exists(savedFile))// проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);// читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>();// создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false;// добавляем переменную для отследивания процесса добавления записей 
                    bool existence = false; // добавляем переменную чтобы в дальнейшем выдать информацию, если данной ценовой категории нет при заданных фильтрах
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: До 500 руб."))// проверяем содержится ли данная информация 
                        {
                            existence = true;
                            adding = true;
                            filteredRecords.Add(record);// добавляем в отпильтрованную запись 
                        }
                    }

                    if (!existence) // выдаем сообщение если при данных фильтрах ничего нет 
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else  // если при данных фильтрах что-то есть 
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);// все обьединяем и разделяем
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);// записываем все в файл 

                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal); // выводим информацию о заведениях
                    }
                }
                
            }
            catch (Exception ex)// Обработка исключения
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        else if (message.Text == "500-1000 руб.") // ПроВерка на 500-1000 руб.
        {
            try
            {
                if (System.IO.File.Exists(savedFile))// проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);// читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>();// создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false;// добавляем переменную для отследивания процесса добавления записей 
                    bool existence = false; // добавляем переменную чтобы в дальнейшем выдать информацию, если данной ценовой категории нет при заданных фильтрах
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: 500-1000 руб."))// проверяем содержится ли данная информация 
                        {
                            existence = true;
                            adding = true;
                            filteredRecords.Add(record);// добавляем в отпильтрованную запись 
                        }
                    }

                    if (!existence) // выдаем сообщение если при данных фильтрах ничего нет 
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else  // если при данных фильтрах что-то есть 
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);// все обьединяем и разделяем
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);// записываем все в файл 

                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal); // выводим информацию о заведениях
                    }
                }

            }
            catch (Exception ex)// Обработка исключения
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        else if (message.Text == "1000-1500 руб.") // ПроВерка на 1000-1500 руб.
        {
            try
            {
                if (System.IO.File.Exists(savedFile))// проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);// читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>();// создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false;// добавляем переменную для отследивания процесса добавления записей 
                    bool existence = false; // добавляем переменную чтобы в дальнейшем выдать информацию, если данной ценовой категории нет при заданных фильтрах
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: 1000-1500 руб."))// проверяем содержится ли данная информация 
                        {
                            existence = true;
                            adding = true;
                            filteredRecords.Add(record);// добавляем в отпильтрованную запись 
                        }
                    }

                    if (!existence) // выдаем сообщение если при данных фильтрах ничего нет 
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else  // если при данных фильтрах что-то есть 
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);// все обьединяем и разделяем
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);// записываем все в файл 

                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal); // выводим информацию о заведениях
                    }
                }

            }
            catch (Exception ex)// Обработка исключения
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }
        else if (message.Text == "От 1500 руб.") // ПроВерка на от 1500 руб.
        {
            try
            {
                if (System.IO.File.Exists(savedFile))// проверяем есть ли файл, в который мы записывали информацию после выбора типа заведений
                {
                    string savedData = System.IO.File.ReadAllText(savedFile);// читаем то, что содержится в файле 
                    var records = savedData.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries); // разбиваем записи и сохраняем 

                    List<string> filteredRecords = new List<string>();// создаем список для дальнейшей работы с отфильтрованными записями 
                    bool adding = false;// добавляем переменную для отследивания процесса добавления записей 
                    bool existence = false; // добавляем переменную чтобы в дальнейшем выдать информацию, если данной ценовой категории нет при заданных фильтрах
                    foreach (var record in records)
                    {
                        if (record.Contains("Средний чек: От 1500 руб."))// проверяем содержится ли данная информация 
                        {
                            existence = true;
                            adding = true;
                            filteredRecords.Add(record);// добавляем в отпильтрованную запись 
                        }
                    }

                    if (!existence) // выдаем сообщение если при данных фильтрах ничего нет 
                    {
                        await botClient.SendTextMessageAsync(chatId, "При ваших фильтрах отсутствует заведение в городе Перми с данным средним чеком.");
                    }

                    else  // если при данных фильтрах что-то есть 
                    {
                        string filteredRecordsFinal = string.Join("\r\n\r\n", filteredRecords);// все обьединяем и разделяем
                        System.IO.File.WriteAllText(savedFile, filteredRecordsFinal);// записываем все в файл 

                        await botClient.SendTextMessageAsync(chatId, filteredRecordsFinal); // выводим информацию о заведениях
                    }
                }

            }
            catch (Exception ex)// Обработка исключения
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}");
            }
        }

        if (message.Text == "✔Поддержка✔") // ПроВерка на Поддержка
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
             new[] { InlineKeyboardButton.WithUrl("Обратная связь", "https://forms.gle/RN9s4z8i1ohjYxHeA") }, // создание кнопки для перессылки на гугл-форму если пользователь нажимает нажимает на "поддержка"
            });
            await botClient.SendTextMessageAsync(chatId, "Нажмите, чтобы заполнить форму:", replyMarkup: inlineKeyboard);// отправка сообщений пользователю
        }

        List<string> districts = new List<string> { "Мотовилихинский", "Свердловский", "Индустриальный", "Ленинский", "Орджоникидзевский", "Дзержинский", "Кировский" };// списки для дальнейшей работы
        List<string> restaurants = new List<string> { "Ресторан", "Кафе", "Бар/Паб" };
        List<string> prices = new List<string> { "До 500 руб.", "500-1000 руб.", "1000-1500 руб.", "От 1500 руб." };
        List<string> kitchens = new List<string> { "Восточная", "Европейская", "Быстрое питание", "Грузинская", "Азиатская", "Итальянская", "Русская" };

        if (message.Text == "/start" || message.Text == "⟲Начать заново⟲") // если пользователем отправлены данные сообщения/сообщение
        {

            var replyMarkup = new ReplyKeyboardMarkup(new[] // создаем кнопки 
           {
             new[] { new KeyboardButton("Ресторан"), new KeyboardButton("Кафе") },
             new[] { new KeyboardButton("Бар/Паб")},
             new[] { new KeyboardButton("✔Поддержка✔") },
             new[] { new KeyboardButton("⟲Начать заново⟲") },
         }, resizeKeyboard: true);           
            await botClient.SendTextMessageAsync(chatId, "В какое заведение вы хотели бы пойти:", replyMarkup: replyMarkup);// отправка сообщений пользователю
        }
        else if (message.Text == "Ресторан")// если пользователем отправлены данные сообщения/сообщение
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]// создаем кнопки
            {new[] { new KeyboardButton("Русская"), new KeyboardButton("Восточная") },
             new[] { new KeyboardButton("Европейская"), new KeyboardButton("Быстрое питание") },
             new[] { new KeyboardButton("Азиатская"),new KeyboardButton("Итальянская")},
             new[] { new KeyboardButton("Грузинская") },
             new[] { new KeyboardButton("✔Поддержка✔") },
             new[] { new KeyboardButton("⟲Начать заново⟲") },
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип кухни: {message.Text}. Теперь выберите тип кухни:", replyMarkup: replyMarkup);// отправка сообщений пользователю
        }

        else if (message.Text == "Бар/Паб")// если пользователем отправлены данные сообщения/сообщение
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]// создаем кнопки
           {
             new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
             new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
             new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
             new[] { new KeyboardButton("Кировский") },
             new[] { new KeyboardButton("✔Поддержка✔") },
             new[] { new KeyboardButton("⟲Начать заново⟲") },
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);// отправка сообщений пользователю
        }

        else if (message.Text == "Кафе")// если пользователем отправлены данные сообщения/сообщение
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]// создаем кнопки
            {
             new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
             new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
             new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
             new[] { new KeyboardButton("Кировский") },
             new[] { new KeyboardButton("✔Поддержка✔") },
             new[] { new KeyboardButton("⟲Начать заново⟲") },
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);// отправка сообщений пользователю
        }

        else if (kitchens.Contains(message.Text))// если пользователем отправлены данные сообщения/сообщение
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]// создаем кнопки
            {
             new[] { new KeyboardButton("Мотовилихинский"), new KeyboardButton("Свердловский") },
             new[] { new KeyboardButton("Индустриальный"), new KeyboardButton("Ленинский") },
             new[] { new KeyboardButton("Орджоникидзевский"), new KeyboardButton("Дзержинский") },
             new[] { new KeyboardButton("Кировский") },
             new[] { new KeyboardButton("✔Поддержка✔") },
             new[] { new KeyboardButton("⟲Начать заново⟲") },
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали тип заведения : {message.Text}. Теперь выберите район:", replyMarkup: replyMarkup);// отправка сообщений пользователю
        }
        else if (districts.Contains(message.Text))// если пользователем отправлены данные сообщения/сообщение
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]// создаем кнопки
            {
              new[] { new KeyboardButton("До 500 руб."), new KeyboardButton("500-1000 руб.") },
             new[] { new KeyboardButton("1000-1500 руб."), new KeyboardButton("От 1500 руб.") },
             new[] { new KeyboardButton("✔Поддержка✔") },
             new[] { new KeyboardButton("⟲Начать заново⟲") },
         }, resizeKeyboard: true);

            await botClient.SendTextMessageAsync(chatId, $"Вы выбрали район: {message.Text}. Теперь выберите ценовую категорию:", replyMarkup: replyMarkup);// отправка сообщений пользователю
        }
    }
}