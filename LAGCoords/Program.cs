namespace LAGCoords;

public class Program{
    public static void Main(string[] args)
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║     OPTIMIZADOR DE RUTAS LOGÍSTICAS - TSP             ║");
        Console.WriteLine("║          Algoritmo Genético con Análisis              ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

        // Definir ciudades
        var cities = new List<City>
        {
            new City("Buenos Aires", -34.6037, -58.3816),
            new City("Córdoba", -31.4201, -64.1888),
            new City("Rosario", -32.9468, -60.6393),
            new City("Mendoza", -32.8895, -68.8458),
            new City("La Plata", -34.9214, -57.9544),
            new City("San Miguel de Tucumán", -26.8083, -65.2176),
            new City("Mar del Plata", -38.0055, -57.5426),
            new City("Salta", -24.7859, -65.4117)
        };

        // Definir punto de partida
        var startCity = cities[0]; // Buenos Aires
        Console.WriteLine($"📍 Punto de partida: {startCity.Name}\n");

        // Ejecutar algoritmo genético
        Console.WriteLine("🔄 Ejecutando algoritmo genético.\n Por favor, espere...");
        var ga = new GeneticAlgorithm(cities);
        var best = ga.Run(
            populationSize: 200,
            generations: 500,
            mutationRate: 0.02
        );

        double totalDistance = 1 / best.Fitness;

        // ═══════════════════════════════════════════════════════
        // 1. MOSTRAR RUTA ÓPTIMA
        // ═══════════════════════════════════════════════════════
        Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║              RUTA ÓPTIMA ENCONTRADA                   ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

        for (int i = 0; i < best.Route.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {best.Route[i].Name}");
        }
        Console.WriteLine($"\n✓ Distancia total: {totalDistance:F2} km");

        // ═══════════════════════════════════════════════════════
        // 2. ANÁLISIS DE DISTANCIAS DESDE PUNTO DE PARTIDA
        // ═══════════════════════════════════════════════════════
        var distancesFromStart = DistanceAnalyzer.CalculateDistancesFromStart(
            best.Route, startCity);
        var accumulatedDistances = DistanceAnalyzer.CalculateAccumulatedDistances(
            best.Route);

        DistanceAnalyzer.PrintDistanceReport(
            best.Route,
            startCity,
            distancesFromStart,
            accumulatedDistances);

        // ═══════════════════════════════════════════════════════
        // 3. ANÁLISIS DE COSTOS LOGÍSTICOS
        // ═══════════════════════════════════════════════════════

        // Configurar parámetros de costos (puedes ajustar según necesites)
        var logisticConfig = new LogisticConfig
        {
            FuelPricePerLiter = 950.0,      // Precio combustible (ARS)
            KmPerLiter = 8.0,               // Rendimiento del vehículo
            AverageTollCost = 2500.0,       // Costo promedio peaje
            KmBetweenTolls = 150.0,         // Distancia entre peajes
            DriverHourlyCost = 3500.0,      // Costo conductor/hora
            AverageSpeedKmH = 70.0,         // Velocidad promedio
            CostPerStop = 5000.0,           // Costo por parada
            DepreciationPerKm = 45.0,       // Depreciación/km
            MaintenancePerKm = 35.0,        // Mantenimiento/km
            DailyFixedCost = 15000.0        // Costos fijos diarios
        };

        var costCalculator = new LogisticCostCalculator(logisticConfig);

        // Calcular costos totales
        var costBreakdown = costCalculator.CalculateTotalCost(
            best.Route,
            totalDistance,
            best.Route.Length);

        // Calcular costos por ciudad
        var costPerCity = costCalculator.CalculateCostPerCity(
            best.Route,
            startCity);

        // Imprimir reporte de costos
        LogisticCostCalculator.PrintCostReport(costBreakdown, costPerCity);

        // ═══════════════════════════════════════════════════════
        // 4. RESUMEN EJECUTIVO
        // ═══════════════════════════════════════════════════════
        Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  RESUMEN EJECUTIVO                    ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

        Console.WriteLine($"📊 Ciudades visitadas: {best.Route.Length}");
        Console.WriteLine($"📏 Distancia total: {totalDistance:F2} km");
        Console.WriteLine($"💰 Costo total: ${costBreakdown.TotalCost:F2}");
        Console.WriteLine($"💵 Costo por kilómetro: ${costBreakdown.CostPerKm:F2}");
        Console.WriteLine($"⏱️  Tiempo estimado de viaje: {totalDistance / logisticConfig.AverageSpeedKmH:F1} horas");

        double efficiency = totalDistance / costBreakdown.TotalCost * 1000;
        Console.WriteLine($"📈 Eficiencia: {efficiency:F2} km/$1000");

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║            OPTIMIZACIÓN COMPLETADA ✓                  ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");
    }
}