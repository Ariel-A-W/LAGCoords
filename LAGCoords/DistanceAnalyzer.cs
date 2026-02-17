namespace LAGCoords;

public class DistanceAnalyzer
{
    /// <summary>
    /// Calcula la distancia de cada ciudad en la ruta con respecto al punto de partida
    /// </summary>
    public static Dictionary<string, DistanceInfo> CalculateDistancesFromStart(
        City[] route, City startCity)
    {
        var distances = new Dictionary<string, DistanceInfo>();

        foreach (var city in route)
        {
            var directDistance = GeoDistance.Between(startCity, city);
            var position = Array.IndexOf(route, city);

            distances[city.Name] = new DistanceInfo
            {
                CityName = city.Name,
                DirectDistanceFromStart = directDistance,
                PositionInRoute = position,
                IsStartCity = city.Equals(startCity)
            };
        }

        return distances;
    }

    /// <summary>
    /// Calcula la distancia acumulada hasta cada ciudad siguiendo la ruta
    /// </summary>
    public static Dictionary<string, double> CalculateAccumulatedDistances(City[] route)
    {
        var accumulated = new Dictionary<string, double>();
        double totalDistance = 0.0;

        accumulated[route[0].Name] = 0.0; // La primera ciudad tiene distancia 0

        for (int i = 1; i < route.Length; i++)
        {
            totalDistance += GeoDistance.Between(route[i - 1], route[i]);
            accumulated[route[i].Name] = totalDistance;
        }

        return accumulated;
    }

    /// <summary>
    /// Imprime un reporte detallado de distancias
    /// </summary>
    public static void PrintDistanceReport(
        City[] route,
        City startCity,
        Dictionary<string, DistanceInfo> distancesFromStart,
        Dictionary<string, double> accumulatedDistances)
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════");
        Console.WriteLine("         ANÁLISIS DE DISTANCIAS POR CIUDAD");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine($"Punto de partida: {startCity.Name}");
        Console.WriteLine("───────────────────────────────────────────────────────\n");

        Console.WriteLine("{0,-20} {1,-15} {2,-15} {3,-10}",
            "Ciudad", "Dist. Directa", "Dist. Acumulada", "Posición");
        Console.WriteLine(new string('─', 65));

        foreach (var city in route)
        {
            var info = distancesFromStart[city.Name];
            var accumulated = accumulatedDistances[city.Name];

            Console.WriteLine("{0,-20} {1,-15:F2} {2,-15:F2} {3,-10}",
                city.Name,
                info.DirectDistanceFromStart,
                accumulated,
                info.PositionInRoute + 1);
        }

        Console.WriteLine("═══════════════════════════════════════════════════════\n");
    }
}
