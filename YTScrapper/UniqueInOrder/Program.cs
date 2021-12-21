using System.Linq;

var res = UniqueInOrder("AAAABBBCCDAABBB");

Console.WriteLine();
foreach (var item in res)
{
    Console.WriteLine(item);
}
Console.WriteLine();

static IEnumerable<T> UniqueInOrder<T>(IEnumerable<T> input)
{
    var prev = default(T);

    foreach (var elem in input)
    {
        if (prev is not null && prev.Equals(elem))
            continue;

        prev = elem;

        yield return elem;
    }
}