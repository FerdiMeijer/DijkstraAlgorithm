namespace DijkstraAlgorithm
{
    public static class DoubleExtentions
    {
        public static double Add(this double value, double add)
        {
            // value + add = max
            if (double.MaxValue - value <= add)
            {
                return double.MaxValue;
            }

            return value + add;
        }
    }
}