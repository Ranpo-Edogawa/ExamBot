// using Newtonsoft.Json;
using System.Text.Json;
using System.Threading;
using Quizbot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;


public class Program
{
    public static int TestNumber = 0;
    public static int TestNumber1;
    public static int correctAnswer = 0;
    public static string PhoneNumber;
    public static string adminText;
    public static int questionNum;
    public static string question;
    public static string answer1;
    public static string answer2;
    public static string answer3;
    public static string answer4;
    public static string correctanswer;
    public static int Status = 0;
    public static List<string> list = new List<string>();
    public static int updater = 0;





    public static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClient("6606156173:AAGPkwS9dfLQqiRikce80dQ4KMw8DDX6a00");
        PdfDownload pdfDownload = new PdfDownload();
        ContactPdf contactPdf = new ContactPdf();
        Class1 class1 = new Class1();
        QuestionAnswerPdf questionAnswerPdf = new QuestionAnswerPdf();
        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        try
        {

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot

        cts.Cancel();

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {


            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
                UpdateType.CallbackQuery => HandleCallBackQueryAsync(botClient, update, cancellationToken)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error: {ex.Message}");
            }

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { new KeyboardButton("Admin"), new KeyboardButton("General") },
                new KeyboardButton[] { new KeyboardButton("Start") },
            });


            ReplyKeyboardMarkup replyKeyboardMarkup2 = new(new[]
            {
                new KeyboardButton[] { "Users List", new KeyboardButton("Subject CRUD")},
                new KeyboardButton[] { "Back ", new KeyboardButton("Back " ) }
            }

            );
            ReplyKeyboardMarkup replyKeyboardMarkup3 = new(new[]
            {
                new KeyboardButton[] { "Create", new KeyboardButton("Update") },
                new KeyboardButton[] { "Delete", new KeyboardButton("back") },
            }
            );

            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            if (messageText == "/start" || messageText.ToLower() == "start")
            {
                await SendStartMessageAsync(chatId, botClient, cancellationToken);
            }
            else if (messageText == "Users List")
            {
                await using Stream stream = System.IO.File.OpenRead("D:\\New folder (2)\\Quizbot 2\\Quizbot\\contact\\hello-from-contact.pdf");
                await botClient.SendDocumentAsync(
                    chatId: update.Message.Chat.Id,
                    document: InputFile.FromStream(stream: stream, fileName: $"All_users2.pdf"),
                    caption: "All user info"
                    );
                stream.Dispose();
            }
            else if (messageText.ToLower() == "share contact")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    // P/s: Faqat meni no'merimga ishledi
                    text: "Please send your code to be an admin!\n",
                    cancellationToken: cancellationToken);
            }


            else if (messageText == "Send")
            {
                //  class1.GetData(question,answer1,answer2,answer3,answer4,correctanswer);

            }
            else if (messageText == "User")
            {
                await UserMessageAsync(chatId, botClient, cancellationToken);
            }

            else if (messageText == "Back")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "choose",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "Admin")
            {
                await AdminRegisterationMessageAsync(chatId, botClient, cancellationToken);
            }
            else if (messageText == "Subject CRUD" || messageText == "CRUD")
            {
                await CRUDMessageAsync(chatId, botClient, cancellationToken);
            }
            else if (messageText.StartsWith("5755"))
            {
                adminText = messageText;
                await botClient.SendTextMessageAsync(
               chatId: chatId,
              text: "Choose",
               replyMarkup: replyKeyboardMarkup2,
              cancellationToken: cancellationToken);
            }
            else if (messageText == "back")
            {
                adminText = messageText;
                await botClient.SendTextMessageAsync(
               chatId: chatId,
              text: "Choose",
               replyMarkup: replyKeyboardMarkup2,
              cancellationToken: cancellationToken);
            }
            else if (messageText == "Statistics")
            {

                pdfDownload.GetMethod(correctAnswer);

                await using Stream stream = System.IO.File.OpenRead("D:\\New folder (2)\\Quizbot 2\\Quizbot\\pdfs\\hello-from-umarhon.pdf");
                await botClient.SendDocumentAsync(
                    chatId: update.Message.Chat.Id,
                    document: InputFile.FromStream(stream: stream, fileName: $"All_users2.pdf"),
                    caption: "Statistics of Answers"
                    );
                stream.Dispose();
                correctAnswer = 0;
            }
            if (update.Message.Text.ToLower() == "create")
            {
                Status = 1;
            }
            if (Status != 0 && Status < 16)
            {
                switch (Status)
                {
                    case 1:
                        await SendMessage(botClient, update, cancellationToken, "Please enter the question: ", 2);
                        break;
                    case 2:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 3, "Please enter option A:");
                        break;
                    case 4:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 5, "Please enter option B:");
                        break;
                    case 6:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 7, "Please enter option C:");
                        break;
                    case 8:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 9, "Please enter option D:");
                        break;
                    case 10:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 11, "Please enter the correct answer (A, B, C, D):");
                        break;
                    case 12:
                        await SendMessage(botClient, update, cancellationToken, "Succesfully !!!", 13);
                        break;
                    case 15:
                        updater = 0;
                        break;
                    default:
                        break;
                };
            }
            if (update.Message.Text.ToLower() == "update")
            {
                updater = 1;
            }
            if (updater != 0 && updater < 16)
            {
                switch (updater)
                {
                    case 1:
                        await SendMessage(botClient, update, cancellationToken, "Please enter the question: ", 2);
                        break;
                    case 2:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 3, "Please enter option A:");
                        break;
                    case 4:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 5, "Please enter option B:");
                        break;
                    case 6:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 7, "Please enter option C:");
                        break;
                    case 8:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 9, "Please enter option D:");
                        break;
                    case 10:
                        await ReceieveQuestion(botClient, update, cancellationToken, list, 11, "Please enter the correct answer (A, B, C, D):");
                        break;
                    case 12:
                        await SendMessage(botClient, update, cancellationToken, "Succesfully !!!", 13);
                        break;
                    case 15:
                        updater = 0;
                        break;
                    default:
                        break;
                };
            }

            else if (update.Message.Text.ToLower() == "delete")
            {
                DeleteAsync(botClient, update, cancellationToken);
            }

        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }

    private static async Task<int> GetPdfAllList(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string v1, int v2)
    {
        string pdfFilePath = Testlar.Get();

        // Faylni o'qish uchun FileStream yaratish
        using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read))
        {
            // MemoryStream yaratish va PDF faylini uni ichiga yozish
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);

                // MemoryStreamni boshiga olib kelish
                memoryStream.Position = 0;

                await botClient.SendDocumentAsync(
                    chatId: update.Message.Chat.Id,
                    document: InputFile.FromStream(stream: memoryStream, fileName: $"All_users.pdf"),
                    caption: v1
                );
            }
        }

        updater = v2;
        return v2;
    }

    private static async Task SendStartMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Admin"), new KeyboardButton("User") }
        });

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Welcome! Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static async Task AdminRegisterationMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Share contact"), new KeyboardButton("start") }
        });

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static async Task UserMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("General"), new KeyboardButton("Biology") },
            new KeyboardButton[] { new KeyboardButton("Statistics"), new KeyboardButton("Back") }
        });

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static async Task CRUDMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Create"), new KeyboardButton("Update") },
            new KeyboardButton[] { new KeyboardButton("Delete"), new KeyboardButton("Start") }
        });

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private async static Task<int> ReceieveQuestion(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, List<string> list, int i, string text)
    {
        await Console.Out.WriteLineAsync(update.Message.Text);
        if (update.Message.Text != null)
        {
            list.Add(update.Message.Text);
        }
        Status = i;
        await SendMessage(botClient, update, cancellationToken, text, i + 1);
        return i;
    }

    private async static Task<int> SendMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string v, int status)
    {
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: v,
            cancellationToken: cancellationToken);
        Status = status;
        return status;
    }


    private async static Task<int> Stop(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string v, int status)
    {
        return 11;
    }

    public static async void DeleteAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Pkease choose index what about you want removed",
            cancellationToken: cancellationToken);
            int index = int.Parse(update.Message.Text);
            Testlar.Delete(index);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }




    private static async Task HandleCallBackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var callBack = update.CallbackQuery;

        var tests = Testlar.GetTest();
        var tests1 = Testlar.GetBiologiya();

        var test = tests[TestNumber];

        //await CheckAnswerAsync(test,botClient, callBack, cancellationToken);
        TestNumber++;
        try
        {
            var nextTest = tests[TestNumber];
            //await SendNextQuestion(nextTest,botClient, update, cancellationToken);
        }
        catch (ArgumentOutOfRangeException e)
        {
            await botClient.SendTextMessageAsync(
                chatId: callBack.Message.Chat.Id,
                text: "This was the last question",
                cancellationToken: cancellationToken);
        }

    }

    private static async Task SendNextQuestion(Test nextTest, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
    new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.A}", callbackData: "A"),


    }, new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.B}", callbackData: "B"),

    }, new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.C}", callbackData: "C"),

    }, new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.D}", callbackData: "D"),

    }

});
        if (update.Message is null)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: $"{nextTest.Question}",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken
                );
        }
        else
        {
            await botClient.SendTextMessageAsync(
               chatId: update.Message.Chat.Id,
               text: $"{nextTest.Question}",
               replyMarkup: inlineKeyboard,
               cancellationToken: cancellationToken
               );
        }
    }

    private static async Task CheckAnswerAsync(Test test, ITelegramBotClient botClient, CallbackQuery? callBack, CancellationToken cancellationToken)
    {
        if (callBack.Data == test.CorrectAnswer)
        {
            correctAnswer++;
            await botClient.SendTextMessageAsync(
                 chatId: callBack.From.Id,
                text: $"Answer is correct. {correctAnswer}",
                cancellationToken: cancellationToken
                );
        }
        else
        {
            await botClient.SendTextMessageAsync(
               chatId: callBack.From.Id,
              text: $"Incorrect Answer ",
              cancellationToken: cancellationToken
              );
        }
    }

    private static async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;

        if (messageText == "General")
        {
            TestNumber = 0;
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Assalomu aleykum {update.Message.From.FirstName}",
                cancellationToken: cancellationToken
            );
            var tests = Testlar.GetTest();
            //await SendNextQuestion(tests[0], botClient, update, cancellationToken);
        }

        if (messageText == "Biology")
        {
            TestNumber = 0;
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Assalomu aleykum {update.Message.From.FirstName}",
                cancellationToken: cancellationToken
            );
            var tests2 = Testlar.GetBiologiya();
            await SendNextQuestion(tests2[0], botClient, update, cancellationToken);
        }
    }
}