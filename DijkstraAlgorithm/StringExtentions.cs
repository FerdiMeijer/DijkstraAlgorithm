namespace DijkstraAlgorithm;

public static class StringExtensions
{
    public static string ToCsv<T>(this IEnumerable<T> list, string separator = ", ")
    {
        return list?.Aggregate(string.Empty, (a, b) =>
        {
            if (string.IsNullOrEmpty(a))
            {
                return b?.ToString() ?? string.Empty;
            }

            return string.Concat(a, separator, b);
        }) ?? string.Empty;
    }
}
