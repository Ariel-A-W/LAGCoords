namespace LAGCoords;

public static class Mutation
{
    public static void Swap(
   Individual individual, double rate, Random rnd)
    {
        if (rnd.NextDouble() < rate)
        {
            int a = rnd.Next(individual.Route.Length);
            int b = rnd.Next(individual.Route.Length);

            (individual.Route[a], individual.Route[b]) =
            (individual.Route[b], individual.Route[a]);
        }
    }
}
