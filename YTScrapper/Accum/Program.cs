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

        private static string Accumulation(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return string.Join("-",
                text
                    .ToLower()
                    .Select((x, i) => $"{char.ToUpper(x)}{new string(x, i)}"));
        }

    }
}
