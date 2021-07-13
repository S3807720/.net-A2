﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MCBA_Web.Utilities
{
    public static class MiscellaneousExtensionUtilities
    {
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
        public static bool HasMoreThanTwoDecimalPlaces(this decimal value) => value.HasMoreThanNDecimalPlaces(2);

        public static bool IsAvailable(this SqlConnection conn)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to the database...");
                conn.Open();
                conn.Close();
                Console.Clear();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        public static DataTable GetDataTable(this SqlCommand command)
        {
            using var adapter = new SqlDataAdapter(command);

            var table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        //mask input
        public static string HideCharacter()
        {
            StringBuilder input = new StringBuilder();
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
