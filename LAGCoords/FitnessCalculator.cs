namespace LAGCoords;

public static class FitnessCalculator
{
    public static double Calculate(Individual individual)
    {
        double totalDistance = 0.0;
        var route = individual.Route;

        for (int i = 0; i < route.Length - 1; i++)
        {
            totalDistance += GeoDistance.Between(route[i], route[i + 1]);
        }

        // Cierre del circuito (TSP)
        totalDistance += GeoDistance.Between(route[^1], route[0]);

        return 1.0 / totalDistance;
    }
}
