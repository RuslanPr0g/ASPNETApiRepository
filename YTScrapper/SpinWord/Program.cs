using System;
using System.Linq;

namespace SpinWord
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run("emocleW");
            Run("olleH yadnoM");
            Run("This is a test");
            Run("You are tsomla to the last test");
            Run("Just gniddik ereht is llits one more");
        }

        static void Run(string toRun) =>
            Console.WriteLine($"{toRun} => ${SpinWord(toRun)}");

        static string SpinWord(string incoming)
        {
            return incoming.Split(" ")
              .Select(word => { if (word.Length > 5) { word.Reverse(); } return word; })
              .Pipe(x => String.Join(" ", x));
        }
    }

    public static class PipeExtensions
    {
        public static B Pipe<A, B>(this A obj, Func<A, B> func1) => func1(obj);
    }
}
