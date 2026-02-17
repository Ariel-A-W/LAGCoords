namespace LAGCoords;

public static class Crossover
{
    public static Individual OrderCrossover(
        Individual parent1, Individual parent2, Random rnd)
    {
        int size = parent1.Route.Length;
        int start = rnd.Next(size);
        int end = rnd.Next(start + 1, size + 1); // Asegura que end > start

        var child = new City[size];
        var filled = new HashSet<City>();

        // Copiar segmento del parent1
        for (int i = start; i < end; i++)
        {
            child[i] = parent1.Route[i];
            filled.Add(parent1.Route[i]);
        }

        // Llenar el resto con genes del parent2
        int current = end % size;
        foreach (var city in parent2.Route)
        {
            if (!filled.Contains(city))
            {
                child[current] = city;
                filled.Add(city);
                current = (current + 1) % size;
            }
        }

        return new Individual(child);
    }
}

