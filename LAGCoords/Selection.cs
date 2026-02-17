namespace LAGCoords;

public static class Selection
{
    public static Individual Tournament(
        List<Individual> population, int size, Random rnd)
    {
        return population
            .OrderBy(_ => rnd.Next())
            .Take(size)
            .OrderByDescending(i => i.Fitness) // ✅ Mayor fitness es mejor
            .First();
    }
}