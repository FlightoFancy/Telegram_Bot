using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace Telegram_Bot
{
    class Program
    {

        private static string TMDbtoken { get; set; } = "ca4d611fc80bafe0234d2cf6e71ebb52";
        private static string token { get; set; } = "1830176663:AAGyCLq5RbLk7nNADA8R43INGGrPaWW24mQ";
        private static TelegramBotClient client;
        private static TMDbClient movieClient;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text != null)
            {
                Console.WriteLine($"Пришло сообщение с текстом: { msg.Text}");
                switch (msg.Text)
                {
                    case "Стикер":
                        await client.SendStickerAsync(
                        chatId: msg.Chat.Id,
                        sticker: "https://tlgrm.ru/_/stickers/cbe/e09/cbee092b-2911-4290-b015-f8eb4f6c7ec4/256/49.webp",
                        replyMarkup: GetButtons());
                        break;
                    case "Картинка":
                        await client.SendPhotoAsync(
                        chatId: msg.Chat.Id,
                        photo: "https://pbs.twimg.com/media/EgRlYClUEAAdJwA.jpg",
                        replyMarkup: GetButtons());
                        break;
                    case "Фильм дня":
                        await client.SendTextMessageAsync(
                        chatId: msg.Chat.Id,
                        text: GetMovies(),
                        replyMarkup: GetButtons());
                        break;
                    case "Цитата":
                        await client.SendTextMessageAsync(
                        chatId: msg.Chat.Id,
                        text: "Что вершит судьбу человечества в этом мире ? Некое незримое существо или закон, подобно Длани Господней парящей над миром ? По крайне мере истинно то, что человек не властен даже над своей волей.",
                        replyMarkup: GetButtons());
                        break;

                    default:
                        await client.SendTextMessageAsync(msg.Chat.Id, "Выберите команду: ", replyMarkup: GetButtons());
                        break;
                }
            }
        }

        private static string GetMovies()
        {
            movieClient = new TMDbClient(TMDbtoken);
            Movie movie = movieClient.GetMovieAsync(491584, "ru").Result;         
            string film = $"{movie.Title}, {movie.ReleaseDate.Value.Year}\n Рейтинг: {movie.VoteAverage}";
            return film;
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Цитата"}, new KeyboardButton { Text = "Фильм дня" } },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Картинка" }, new KeyboardButton { Text = "Стикер" } }
                }
            };
        }
    }
}
