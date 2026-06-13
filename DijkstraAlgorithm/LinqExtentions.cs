namespace DijkstraAlgorithm;

public static class LinqExtensions
{
    public static T? MinOrDefault<T>(this IEnumerable<T> sequence)
    {
        return sequence.Any() ? sequence.Min() : default;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable?.Any() != true;
    }
}
