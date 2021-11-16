using System;
using System.Globalization;
using System.Linq;

namespace Accum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"fDSGdg: {Accumulation("fDSGdg")}");
            Console.WriteLine($"Fffffasdfg: {Accumulation("Fffffasdfg")}");
            Console.WriteLine($"fffFDFGASfds: {Accumulation("fffFDFGASfds")}");
            Console.WriteLine($"dfaFFFfasd: {Accumulation("dfaFFFfasd")}");
            Console.WriteLine($"aa: {Accumulation("aa")}");
            Console.WriteLine($"AA: {Accumulation("AA")}");
            Console.WriteLine($"aA: {Accumulation("aA")}");
            Console.WriteLine($"Aa: {Accumulation("Aa")}");
            Console.WriteLine($"Empty: {Accumulation(string.Empty)}");
            Console.WriteLine($"Null: {Accumulation(null)}");
        }

        private static string Accumulation(string text) =>
            text
              ?.ToLower()
              .ToCharArray()
              .Select((x, i) => $"{ x.Repeat(i + 1) }").ToArray()
              .JoinToString("-")
              .ToTitleCase()
              ?? string.Empty;
    }

    static class Extentions
    {
        public static string ToTitleCase(this string s) =>
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s);

        public static string Repeat(this char c, int count) =>
            new(c, count);

        public static string JoinToString(this string[] s, string separator) =>
            string.Join(separator, s);
    }
}
