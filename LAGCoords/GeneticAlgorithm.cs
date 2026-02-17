namespace LAGCoords;

public class GeneticAlgorithm
{
    private readonly List<City> _cities;
    private readonly Random _rnd = new();

    public GeneticAlgorithm(List<City> cities)
    {
        _cities = cities;
    }

    public Individual Run(
        int populationSize,
        int generations,
        double mutationRate)
    {
        // 🔹 Población inicial con rutas reales (City[])
        var population = Enumerable.Range(0, populationSize)
            .Select(_ => new Individual(_cities, _rnd))
            .ToList();

        // 🔹 Fitness inicial
        foreach (var ind in population)
            ind.Fitness = FitnessCalculator.Calculate(ind);

        for (int g = 0; g < generations; g++)
        {
            var newPopulation = new List<Individual>();

            while (newPopulation.Count < populationSize)
            {
                var parent1 = Selection.Tournament(population, 3, _rnd);
                var parent2 = Selection.Tournament(population, 3, _rnd);

                var child = Crossover.OrderCrossover(parent1, parent2, _rnd);
                Mutation.Swap(child, mutationRate, _rnd);

                child.Fitness = FitnessCalculator.Calculate(child);
                newPopulation.Add(child);
            }

            population = newPopulation;
        }

        // 🔹 Menor distancia = mayor fitness (1 / distancia)
        return population.OrderByDescending(i => i.Fitness).First();
    }
}
