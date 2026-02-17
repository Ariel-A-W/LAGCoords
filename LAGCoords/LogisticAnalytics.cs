namespace LAGCoords;

public static class LogisticAnalytics
{
    public static City FindNearest(City origin, IEnumerable<City> cities)
        => cities.OrderBy(c => GeoDistance.Between(origin, c)).First();

    public static (City A, City B, double Distance) ClosestPair(List<City> cities)
    {
        double min = double.MaxValue;
        City? a = null, b = null;

        for (int i = 0; i < cities.Count; i++)
        {
            for (int j = i + 1; j < cities.Count; j++)
            {
                double d = GeoDistance.Between(cities[i], cities[j]);
                if (d < min)
                {
                    min = d;
                    a = cities[i];
                    b = cities[j];
                }
            }
        }
        return (a, b, min)!;
    }
}