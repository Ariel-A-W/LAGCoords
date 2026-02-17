namespace LAGCoords;

public class Individual
{
    public City[] Route { get; set; }
    public double Fitness { get; set; }

    // Constructor para población inicial
    public Individual(List<City> cities, Random rnd)
    {
        Route = cities
            .OrderBy(_ => rnd.Next())
            .ToArray();
    }

    // Constructor para crossover / mutation
    public Individual(City[] route)
    {
        Route = route;
    }
}
