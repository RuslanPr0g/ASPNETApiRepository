using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Accum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Accumulation("fDSGdg"));
        }

        private static string Accumulation(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            StringBuilder stringBuilder = new();

            var i = 0;
            text.ToList().ForEach((c) =>
            {
                stringBuilder.Append(new string(c, i + 1));

                if (i < text.Length - 1)
                    stringBuilder.Append('-');

                i++;
            });

            return stringBuilder.ToString().ToTitleCase();
        }
    }

    static class Extentions
    {
        public static string ToTitleCase(this string s) =>
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
    }
}
