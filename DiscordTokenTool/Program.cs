using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTokenTool
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string title = @"
______ _                       _ _____     _            _____           _ 
|  _  (_)                     | |_   _|   | |          |_   _|         | |
| | | |_ ___  ___ ___  _ __ __| | | | ___ | | _____ _ __ | | ___   ___ | |
| | | | / __|/ __/ _ \| '__/ _` | | |/ _ \| |/ / _ \ '_ \| |/ _ \ / _ \| |
| |/ /| \__ \ (_| (_) | | | (_| | | | (_) |   <  __/ | | | | (_) | (_) | |
|___/ |_|___/\___\___/|_|  \__,_| \_/\___/|_|\_\___|_| |_\_/\___/ \___/|_|
                                                                          ";
            Console.WriteLine(title);
            Console.ResetColor();

            Console.Write("\nEmail or Phone Number: ");
            string email = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            await getToken(email, password);
        }

        async static Task getToken(string email, string password)
        {
            string url = "https://discord.com/api/v9/auth/login";

            var payload = new Dictionary<string, string>()
            {
                { "login", email}, { "password", password }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            HttpClient client = new HttpClient();
            StringContent stringContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, stringContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\nLogin Successful!");
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseContent);
                Console.WriteLine($"User ID: {json["user_id"]}");
                Console.Write($"Token: {json["ticket"]}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nLogin Failed!");
                Console.Write($"Error: {response.ReasonPhrase}");
                Console.ReadKey();
            }
        }
    }
}