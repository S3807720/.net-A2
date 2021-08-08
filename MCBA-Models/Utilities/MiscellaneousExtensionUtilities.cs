using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;

namespace MCBA_Models.Utilities
{
    public static class MiscellaneousExtensionUtilities
    {
        public static char ParseEnum(this string value)
        {
            return value[0];
        }
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
        public static bool HasMoreThanTwoDecimalPlaces(this decimal value) => value.HasMoreThanNDecimalPlaces(2);
        //https://www.talkingdotnet.com/store-complex-objects-in-asp-net-core-session/
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        //mask input
        public static string HideCharacter()
        {
            StringBuilder input = new();
            while (true)
            {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    //remove *
                    Console.SetCursorPosition(x - 1, y);
                    Console.Write(" ");
                    Console.SetCursorPosition(x - 1, y);
                }
                else if (key.Key != ConsoleKey.Backspace)
                { //add key to string, display *
                    input.Append(key.KeyChar);
                    Console.Write("*");
                } //eat invalid characters
                else { }
            }
            return input.ToString();
        }
    }
}
