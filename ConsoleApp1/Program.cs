using System;
using MihaZupan;
using Telegram.Bot;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net;
using ConsoleApp1;

namespace ConsoleApp1
{
    class Program : Tasks
    {
        private static ITelegramBotClient botClient;
        private static Tasks[] array  = new Tasks[100];
        private static int size = 0;
        private static int ChatState = 0;
        private static int first = 0;
        private static int second = 0;
        private static int TaskIndex = 0;
        private static string version = "e";
        public Program(int i) : base(i)
        {}

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            if (version == "T")
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new Tasks(100);
                }
                // Добавляем Прокси для работы самого телеграмм
                var proxy = new HttpToSocks5Proxy("192.169.196.50", 23372);

                // Регестрируем бота в сети, используя наш API ключ. Так же указывает период ожидания - 10 секунд, после чего отключаемся
                botClient = new TelegramBotClient("1068239979:AAE4jLGq70Q3h73l9azagUg5zLmxaSUbuFg", proxy) { Timeout = TimeSpan.FromSeconds(100) };

                // Проверка на работоспособность. Вылетит ошибка в случае не подключения. Мы получим null
                var me = botClient.GetMeAsync().Result;
                Console.WriteLine($"Bot id: {me.Id} Bot Name: {me.FirstName}");

                // Добавление всех наших функций к самому телеграмм боту, чтобы он мог их сам вызывать
                botClient.OnMessage += Bot_OnMessage;
                botClient.OnCallbackQuery += BotOnCallbackQueryReveived;
                botClient.OnInlineQuery += BotOnInlineQueryReceived;
                botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
                botClient.OnReceiveError += BotOnReceiveError;

                // Его непосредственное начало работы. Мы запускаем прослушивание сообщений
                botClient.StartReceiving();
            }
            else
            {
                string task = Console.ReadLine();

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new Tasks(100);
                }
                while (task != "q")
                {
                    task = Console.ReadLine();
                    Bot_OnMessage1(task);
                    /*
                    // Пример работы
                    Bot_OnMessage1("/add topic D: Example M: task1 1 task2 0 T: 2000-01-21 11:11:11");
                    Bot_OnMessage1("/add topic D: Example M: task1 1 task2 0 T: 2001-01-21 11:11:11");

                    Bot_OnMessage1("/edit");
                    Bot_OnMessage1("-1");
                    Bot_OnMessage1("2");
                    Bot_OnMessage1("0");
                    Bot_OnMessage1("1");
                    Bot_OnMessage1("TOPICNAME");
                    Bot_OnMessage1("/show");

                    Bot_OnMessage1("/edit");
                    Bot_OnMessage1("0");
                    Bot_OnMessage1("2.0");
                    Bot_OnMessage1("TASKNAME");

                    Bot_OnMessage1("TASKNAME 0");
                    Bot_OnMessage1("/show");
                    */
                }
            }
            Console.ReadKey();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            // Обработчик ошибок, которые идут со стороны телеграмма
            Console.WriteLine("Received error: {0} — {1}",
               receiveErrorEventArgs.ApiRequestException.ErrorCode,
               receiveErrorEventArgs.ApiRequestException.Message);
        }

        private static void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                new InlineQueryResultLocation(
                    id: "1",
                    latitude: 40.7058316f,
                    longitude: -74.2581888f,
                    title: "New York")   // Показанный результат
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 40.7058316f,
                            longitude: -74.2581888f)    // Ответ, если результат выбран
                    },

                new InlineQueryResultLocation(
                    id: "2",
                    latitude: 13.1449577f,
                    longitude: 52.507629f,
                    title: "Berlin") // Показанный результат
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 13.1449577f,
                            longitude: 52.507629f)  //Ответ, если результат выбран
                    }
            };
        }


        private static async void BotOnCallbackQueryReveived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            // Действие бота в случае пришедшего сообщения
            // Индекс нужен для соотнесения чата с пользователем и его списка дел.
            var text = e?.Message?.Text;

            // Индекс нужен для соотнесения чата с пользователем и его списка дел.
            var ChatIndex = e.Message.Chat.Id;

            // Разделем все пришедшие слова для выделение ключевых и заполнения полей
            string[] allText = text.Split(' ');

            // Если оно пустое, то ничего не делаем
            if (text == null)
                return;

            ChatIndex = ChatIndex%100;
            // Просто проверка
            Console.WriteLine($"revices text message '{text}' in chat '{1}'");


            if (ChatState == 1)
            {
                string answer = "Введите:" +
                        "0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date";

                //TaskIndex = Int32.Parse(allText[0]);
                try
                {
                    TaskIndex = Convert.ToInt16(allText[0]);
                }
                catch
                {
                    return;
                }


                Console.WriteLine("Trying  {0}", array[ChatIndex].size1);
                int i = TaskIndex;

                if (TaskIndex > array[ChatIndex].array.Length)
                {
                    Console.WriteLine("Добавьте задачу");
                    return;
                }

                if (i < 0 || i >= array[ChatIndex].size1)
                {
                    Console.WriteLine("Вышли за пределы массива, повторите ввод");
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Вышли за пределы массива, повторите ввод"
                        ).ConfigureAwait(false);
                    return;
                }
                ChatState = 2;


                Console.WriteLine(answer);
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: answer
                        ).ConfigureAwait(false);
                return;
            }

            if (ChatState == 2)
            {
                // 0 - Topic
                // 1 - Description
                // 2.n - MarkDowns
                // 3 - Date

                int index = 1;
                // index = int.Parse(allText[0]);
                try
                {
                    index = Convert.ToInt16(allText[0]);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите:" +
                        "0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date"
                        ).ConfigureAwait(false);
                }

                if (allText[0].Length > 1 && allText[0][1] == '.')
                {
                    var Indexes = allText[0].Split('.');
                    Console.WriteLine("All Text is {0}", allText[0]);
                    try
                    {
                        first = Convert.ToInt32(Indexes[0]);
                        second = Convert.ToInt32(Indexes[1]);
                        ChatState = 3;
                        return;
                    }
                    catch
                    {
                        Console.WriteLine("Введите в формате 2.n где n - индекс Подзадачи");
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите в формате 2.n где n - индекс Подзадачи"
                        ).ConfigureAwait(false);
                        return;
                    }
                }
                if (index == 0)
                {
                    Console.WriteLine("Вы собираетесь изменить {0} ?", array[ChatIndex].array[TaskIndex].topic);
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите новое название"
                        ).ConfigureAwait(false);
                    ChatState = 4;
                    Console.WriteLine("Введите новое название");
                    return;
                }

                if (index == 1)
                {
                    ChatState = 5;
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите новое название"
                        ).ConfigureAwait(false);
                    Console.WriteLine("Введите новое название"
                        );
                    return;
                }
                if (index == 3)
                {
                    ChatState = 6;
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите новое название"
                        ).ConfigureAwait(false);
                    Console.WriteLine("Введите новое название");
                    return;
                }
            }
            if (ChatState == 3)
            {
                try
                {
                    array[ChatIndex].array[TaskIndex].Tasks_[second].text = allText[0];
                    array[ChatIndex].array[TaskIndex].Tasks_[second].done = allText[1] == "1" ? true : false;

                    ChatState = 0;
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Измененно"
                        ).ConfigureAwait(false);
                    Console.WriteLine("Измененно");
                }
                catch
                {

                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите в формате TASKNAME DONE"
                        ).ConfigureAwait(false);
                    Console.WriteLine("Введите в формате TASKNAME DONE");
                }
                return;
            }

            if (ChatState == 4)
            {
                array[ChatIndex].array[TaskIndex].topic = allText[0];
                ChatState = 0;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text: "Измененно"
                    ).ConfigureAwait(false);
                Console.WriteLine("Измененно");
                return;
            }

            if (ChatState == 5)
            {
                array[ChatIndex].array[TaskIndex].text = allText[0];
                ChatState = 0;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text: "Измененно"
                    ).ConfigureAwait(false);
                Console.WriteLine("Измененно");

                return;
            }

            if (ChatState == 6)
            {
                try
                {
                    array[ChatIndex].array[TaskIndex].time = DateTime.ParseExact(text, "yyyy-MM-dd HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture);

                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Измененно"
                        ).ConfigureAwait(false);
                    Console.WriteLine( "Измененно");
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Введите дату в формате yyyy-MM-dd HH:mm:ss"
                        ).ConfigureAwait(false);
                    Console.WriteLine("Введите дату в формате yyyy-MM-dd HH:mm:ss");
                    return;
                }
            }

            // Проверяем пришедшую команду
            switch (allText[0])
            {
                case "/add":

                    ChatState = 0;
                    // Выделяем имя
                    string TopicName = " ";
                    try
                    {
                        TopicName = allText[1];
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Напишите в требуемом формате: /add topic D: Example M: task1 1 task2 0 T: 2000-01-21 11:11:11"
                        ).ConfigureAwait(false);
                    }

                    // Выделяем Индексы всех значений. Если они не найдены, то придет -1
                    // Мы ищем второй параметр среди массива слов, пришедших нам
                    int DesriptionNameIndex = FindStr(allText, "D:");
                    int MarkDownIndex = FindStr(allText, "M:");
                    int DateIndex = FindStr(allText, "T:");
                    Console.WriteLine(@"Got indexes {0} {1} {2}", DesriptionNameIndex, MarkDownIndex, DateIndex);
                    // Проверяем описание. В случае отсутствия Дел и времени, заполняем до конца массива.
                    // Unspit просто соедияет слова в одно предложение
                    string DesriptionName = DesriptionNameIndex > 0 && MarkDownIndex > 0 ? Unsplit(allText, DesriptionNameIndex+1, MarkDownIndex) : "-";
                    if (DesriptionName == "-")
                    {
                        Console.WriteLine("{0}   {1}", DesriptionNameIndex + 1, allText.Length);
                        DesriptionName = DesriptionNameIndex > 0  ? Unsplit(allText, DesriptionNameIndex+1, allText.Length-1) : " ";
                    }
                    Console.WriteLine(@" Got Description {0}", DesriptionName);
                    // Берем все наши дела, если они есть
                    int indexOfAll = 1;
                    indexOfAll = DateIndex - MarkDownIndex <= 0 ? indexOfAll : DateIndex - MarkDownIndex;
                    Markdown[] marks = new Markdown[1] { new Markdown("", false) };
                    Console.WriteLine(@" Index OF Marks {0}", indexOfAll);
                    marks = GetMarkdowns(allText, MarkDownIndex+2, DateIndex);
                    Console.WriteLine("Got Marks");
                    // Парсим дату, в учетом строгого порядка. Иначе пихаем текущее время
                    DateTime Date = DateTime.Now;
                    Console.WriteLine("Init Time");
                    try
                    {
                        Console.WriteLine(allText[DateIndex + 1] + ' ' + allText[DateIndex + 2]);
                        Date = DateIndex > 0 && DateIndex < allText.Length ? DateTime.ParseExact(allText[DateIndex+1]+ ' ' + allText[DateIndex + 2], "yyyy-MM-dd HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now;
                        Console.WriteLine("Got Time");
                    }
                    catch
                    {
                        Console.WriteLine("Not Got Time");
                    }

                    // Если выходим на границы массива. то тупо увеличиваем его на 1
                    if (ChatIndex >= array.Length)
                    {
                        Console.WriteLine("Tasks len {0} {1}", ChatIndex, array.Length);

                        Tasks[] array_ = new Tasks[ChatIndex + array.Length];
                        for (int i = 0; i < ChatIndex; i++)
                        {
                            array_[i] = array[i];
                        }
                        array = array_;
                    }
                    Console.WriteLine("Add");
                    // Добавляем всех поля сразу в списрк дел для нашего пользователя

                    array[ChatIndex].Add(TopicName, DesriptionName, marks, Date);

                    // Пишем ответ, что выполнили
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Сделано. Задача добавлена"
                        ).ConfigureAwait(false);
                    Console.WriteLine("Done. Task Added");
                    break;
                case "/edit":
                    ChatState = 1;

                    string output = Show(ChatIndex, array);

                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: output + "\nВведите в отдельном сообщении номер задачи и 0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date для изменения"
                        ).ConfigureAwait(false);
                    Console.WriteLine(output);
                    Console.WriteLine("Введите  номер задачи ");
                    break;
                case "/show":
                    ChatState = 0;

                    output = Show(ChatIndex, array);
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: output + " ^_^"
                        ).ConfigureAwait(false);
                    Console.WriteLine(output);
                    break;
                default:

                    ChatState = 0;
                    // просто стандартный ответ на всякий бред
                    const string answer1 = @"
                        I can't unserstad u
                        Please use this flags:
                            /add
                            /edit
                            /show";

                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: answer1
                        ).ConfigureAwait(false);
                    Console.WriteLine(answer1);
                    break;
            }

            // Пример ответа
            //await botClient.SendTextMessageAsync(
            //    chatId: e.Message.Chat.Id,
            //    text: $"You said '{text}'"
            //    ).ConfigureAwait(false);
        }
        private static async void Bot_OnMessage1(string text)
        {
            // Действие бота в случае пришедшего сообщения
            // Индекс нужен для соотнесения чата с пользователем и его списка дел.
            var ChatIndex = 0;

            // Разделем все пришедшие слова для выделение ключевых и заполнения полей
            string[] allText = text.Split(' ');

            // Если оно пустое, то ничего не делаем
            if (text == null)
                return;

            ChatIndex = ChatIndex % 100;

            if (ChatState == 1)
            {
                string answer = "Введите:" +
                        "0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date";

                //TaskIndex = Int32.Parse(allText[0]);
                try
                {
                    TaskIndex = Convert.ToInt16(allText[0]);
                }
                catch
                {
                    Console.WriteLine("Добавьте задачу");
                    return;
                }


                int i = TaskIndex;

                if (TaskIndex > array[ChatIndex].array.Length)
                {
                    Console.WriteLine("Добавьте задачу");
                    return;
                }

                if (i < 0 || i >= array[ChatIndex].size1)
                {
                    Console.WriteLine("Вышли за пределы массива, повторите ввод");

                    return;
                }
                ChatState = 2;


                Console.WriteLine(answer);
                return;
            }

            if (ChatState == 2)
            {
                // 0 - Topic
                // 1 - Description
                // 2.n - MarkDowns
                // 3 - Date

                int index = 1;
                // index = int.Parse(allText[0]);
                try
                {
                    index = Convert.ToInt16(allText[0]);
                }
                catch
                {
                    Console.WriteLine( "Введите:" +
                        "0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date");
                }

                if (allText[0].Length > 1 && allText[0][1] == '.')
                {
                    var Indexes = allText[0].Split('.');
                    try
                    {
                        first = Convert.ToInt32(Indexes[0]);
                        second = Convert.ToInt32(Indexes[1]);
                        ChatState = 3;
                        Console.WriteLine("Выбранно " +
                         "0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date");
                        return;
                    }
                    catch
                    {
                        Console.WriteLine("Введите в формате 2.n где n - индекс Подзадачи");

                        return;
                    }
                }
                if (index == 0)
                {
                    Console.WriteLine("Вы собираетесь изменить {0} ?", array[ChatIndex].array[TaskIndex].topic);

                    ChatState = 4;
                    Console.WriteLine("Введите новое название");
                    return;
                }

                if (index == 1)
                {
                    ChatState = 5;
                    Console.WriteLine("Введите новое название"
                        );
                    return;
                }
                if (index == 3)
                {
                    ChatState = 6;
                    Console.WriteLine("Введите новое название");
                    return;
                }
            }
            if (ChatState == 3)
            {
                try
                {
                    array[ChatIndex].array[TaskIndex].Tasks_[second].text = allText[0];
                    array[ChatIndex].array[TaskIndex].Tasks_[second].done = allText[1] == "1" ? true : false;

                    ChatState = 0;
                    Console.WriteLine("Измененно");
                }
                catch
                {

                    Console.WriteLine("Введите в формате TASKNAME DONE");
                }
                return;
            }

            if (ChatState == 4)
            {
                array[ChatIndex].array[TaskIndex].topic = allText[0];
                ChatState = 0;

                Console.WriteLine("Измененно");
                return;
            }

            if (ChatState == 5)
            {
                array[ChatIndex].array[TaskIndex].text = allText[0];
                ChatState = 0;

                Console.WriteLine("Измененно");

                return;
            }

            if (ChatState == 6)
            {
                try
                {
                    array[ChatIndex].array[TaskIndex].time = DateTime.ParseExact(text, "yyyy-MM-dd HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture);

                    Console.WriteLine("Измененно");
                }
                catch
                {
                    Console.WriteLine("Введите дату в формате yyyy-MM-dd HH:mm:ss");
                    return;
                }
            }

            // Проверяем пришедшую команду
            switch (allText[0])
            {
                case "/add":

                    ChatState = 0;
                    // Выделяем имя
                    string TopicName = " ";
                    try
                    {
                        TopicName = allText[1];
                    }
                    catch
                    {
                       Console.WriteLine("Напишите в требуемом формате: /add topic D: Example M: task1 1 task2 0 T: 2000-01-21 11:11:11");
                    }

                    // Выделяем Индексы всех значений. Если они не найдены, то придет -1
                    // Мы ищем второй параметр среди массива слов, пришедших нам
                    int DesriptionNameIndex = FindStr(allText, "D:");
                    int MarkDownIndex = FindStr(allText, "M:");
                    int DateIndex = FindStr(allText, "T:");
                    // Проверяем описание. В случае отсутствия Дел и времени, заполняем до конца массива.
                    // Unspit просто соедияет слова в одно предложение
                    string DesriptionName = DesriptionNameIndex > 0 && MarkDownIndex > 0 ? Unsplit(allText, DesriptionNameIndex + 1, MarkDownIndex) : "-";
                    if (DesriptionName == "-")
                    {
                        DesriptionName = DesriptionNameIndex > 0 ? Unsplit(allText, DesriptionNameIndex + 1, allText.Length - 1) : " ";
                    }
                    // Берем все наши дела, если они есть
                    int indexOfAll = 1;
                    indexOfAll = DateIndex - MarkDownIndex <= 0 ? indexOfAll : DateIndex - MarkDownIndex;
                    Markdown[] marks = new Markdown[1] { new Markdown("", false) };
                    marks = GetMarkdowns(allText, MarkDownIndex + 2, DateIndex);
                    // Парсим дату, в учетом строгого порядка. Иначе пихаем текущее время
                    DateTime Date = DateTime.Now;
                    try
                    {
                        Date = DateIndex > 0 && DateIndex < allText.Length ? DateTime.ParseExact(allText[DateIndex + 1] + ' ' + allText[DateIndex + 2], "yyyy-MM-dd HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now;
                    }
                    catch
                    {
                        Console.WriteLine("Not Got Time");
                    }

                    // Если выходим на границы массива. то тупо увеличиваем его на 1
                    if (ChatIndex >= array.Length)
                    {
                        Tasks[] array_ = new Tasks[ChatIndex + array.Length + 1];
                        for (int i = 0; i < ChatIndex; i++)
                        {
                            array_[i] = array[i];
                        }
                        array = array_;
                    }
                    // Добавляем всех поля сразу в списрк дел для нашего пользователя
                   array[ChatIndex].Add(TopicName, DesriptionName, marks, Date);

                    // Пишем ответ, что выполнили
                    Console.WriteLine("Сделано. Задача добавлена");
                    break;
                case "/edit":
                    ChatState = 1;

                    string output = Show(ChatIndex, array);

                    Console.WriteLine(output + "\nВведите в отдельном сообщении номер задачи и 0 - Topic, / 1 - Description, / 2.n - MarkDowns, 3 - Date для изменения");
                    Console.WriteLine("Введите  номер задачи ");
                    break;
                case "/show":
                    ChatState = 0;

                    output = Show(ChatIndex, array);
                    Console.WriteLine(output + " ^_^");
                    break;
                default:

                    ChatState = 0;
                    // просто стандартный ответ на всякий бред
                    const string answer1 = @"
                        I can't unserstad u
                        Please use this flags:
                            /add
                            /edit
                            /show";

                    Console.WriteLine(answer1);
                    break;
            }

            // Пример ответа
            //await botClient.SendTextMessageAsync(
            //    chatId: e.Message.Chat.Id,
            //    text: $"You said '{text}'"
            //    ).ConfigureAwait(false);
        }
        // Нахождение параметра  s внутри массива st и возвращение его индекса
        private static int FindStr(string[] st, string s)
        {
            int Index = 0;
            foreach(string s_ in st)
            {
                if (s_ == s)
                {
                    return Index;
                }
                Index++;
            }
            return -1;
        }
        private static string Show(long ChatIndex, Tasks[] array)
        {
            string answer = "";
            int IndexFromStart = 1;
            try
            {
                for (int i = 0; i < array[ChatIndex].size_; i++)
                {
                    if (array[ChatIndex].array[i] is null)
                        continue;
                    if (array[ChatIndex].array[i].topic is null || array[ChatIndex].array[i].text is null)
                        continue;
                    answer += i.ToString() + ' ';
                    answer += array[ChatIndex].array[i].topic + '\n';
                    answer += "\t" + array[ChatIndex].array[i].text + '\n';
                    for (int j = 0; j < array[ChatIndex].array[i].Tasks_.Length; j++)
                    {
                        if (array[ChatIndex].array[i].Tasks_[j] is null || array[ChatIndex].array[i].Tasks_[j].text is null)
                            continue;
                        if (array[ChatIndex].array[i].Tasks_[j].text == "")
                            continue;
                        answer += "\t \t" + (j + 1) + ' ' + array[ChatIndex].array[i].Tasks_[j].text + " " + array[ChatIndex].array[i].Tasks_[j].done + '\n';
                    }

                    answer += '\t' + array[ChatIndex].array[i].time.ToString() + "\n\n";
                    IndexFromStart++;
                }
            }
            catch
            {
                answer = "Добавьте задачу";
            }

            return answer;
        }
        // Соединение всех слов в одно большое
        private static string Unsplit(string[] st, int start, int end)
        {
            string output = "";

            for (int i = start; i < end ; i++)
            {
                output += st[i] + " ";
            }
            return output;
        }

        // Выделяем все подзадачи, с учетом индексов
        private static Markdown[] GetMarkdowns(string[] Text, int start, int end)
        {
            if (start <= 2)
            {
                // Если индекс - 1, то значит польхователь не указал их
                return new Markdown[1] { new Markdown("", false) };
            }
            // Делаем проверку на указание даты
            end = end == -1 ? Text.Length : end;
            // Создаем нужное количество подзадач согласно условию : Подзадача Статус(1 - Выполнено, 0 - Нет)
            int ind = ((end - start));
            Markdown[] m = new Markdown[ind + 1];
           int index = 0;
            // Проверяем на четность
            int net = start % 2 == 0 ? 1 : 0;

            for (int i = start; i < end; i++)
            {

                // Если стартовый индекс нечетный, то сначала заполняем текст,
                // При следующем проходе цикла уже будет обратное ему значение, значит заполнение второго параметра
                if (i % 2 == net)
                {
                    m[index] = new Markdown(" ");
                    m[index].text = Text[i];
                }
                else
                {
                    if (m[index] is null)
                    {
                        m[index] = new Markdown(" ");
                        m[index].text = Text[i-1];
                    }
                    m[index].done = Text[i] == "1" ? true : false;
                    index++;
                }
            }
            return m;
        }

        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            throw new NotImplementedException();
        }
    }
}
