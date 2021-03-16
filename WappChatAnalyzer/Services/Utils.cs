using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WappChatAnalyzer.Services
{
    static class Utils
    {
        static char[] letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static char[] numbers = "01234567890".ToCharArray();

        public static bool IsNumber(this char c)
        {
            return numbers.Any(o => o == c);
        }

        public static bool IsLetter(this char c)
        {
            return letters.Any(o => o == c);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(var el in enumerable)
                action(el);
        }

        public static string ToISOString(this DateTime dateTime)
        {
            return $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
        }
    }
}
