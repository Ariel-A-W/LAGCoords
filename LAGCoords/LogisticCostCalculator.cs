namespace LAGCoords;

public class LogisticCostCalculator
{
    // Configuración de costos por defecto
    private readonly LogisticConfig _config;

    public LogisticCostCalculator(LogisticConfig? config = null)
    {
        _config = config ?? new LogisticConfig();
    }

    /// <summary>
    /// Calcula el costo total de la ruta considerando todos los factores logísticos
    /// </summary>
    public LogisticCostBreakdown CalculateTotalCost(
        City[] route,
        double totalDistance,
        int numberOfStops = 0)
    {
        if (numberOfStops == 0)
            numberOfStops = route.Length;

        var breakdown = new LogisticCostBreakdown();

        // 1. Costo de combustible
        breakdown.FuelCost = CalculateFuelCost(totalDistance);

        // 2. Costo de peajes (estimado por distancia)
        breakdown.TollCost = CalculateTollCost(totalDistance);

        // 3. Costo de tiempo de conductor
        breakdown.DriverCost = CalculateDriverCost(totalDistance);

        // 4. Costo de paradas/descarga
        breakdown.StopCost = CalculateStopCost(numberOfStops);

        // 5. Depreciación del vehículo
        breakdown.VehicleDepreciation = CalculateDepreciation(totalDistance);

        // 6. Mantenimiento
        breakdown.MaintenanceCost = CalculateMaintenanceCost(totalDistance);

        // 7. Costos fijos (seguro, administración, etc.)
        breakdown.FixedCosts = _config.DailyFixedCost;

        // 8. Total
        breakdown.TotalCost = breakdown.FuelCost +
                              breakdown.TollCost +
                              breakdown.DriverCost +
                              breakdown.StopCost +
                              breakdown.VehicleDepreciation +
                              breakdown.MaintenanceCost +
                              breakdown.FixedCosts;

        breakdown.TotalDistance = totalDistance;
        breakdown.NumberOfStops = numberOfStops;
        breakdown.CostPerKm = breakdown.TotalCost / totalDistance;

        return breakdown;
    }

    /// <summary>
    /// Calcula el costo de combustible basado en la distancia
    /// </summary>
    private double CalculateFuelCost(double distanceKm)
    {
        // Consumo de combustible (litros/km) * distancia * precio por litro
        return (distanceKm / _config.KmPerLiter) * _config.FuelPricePerLiter;
    }

    /// <summary>
    /// Calcula el costo estimado de peajes
    /// </summary>
    private double CalculateTollCost(double distanceKm)
    {
        // Estimación: un peaje cada X kilómetros
        int estimatedTolls = (int)(distanceKm / _config.KmBetweenTolls);
        return estimatedTolls * _config.AverageTollCost;
    }

    /// <summary>
    /// Calcula el costo del tiempo del conductor
    /// </summary>
    private double CalculateDriverCost(double distanceKm)
    {
        // Tiempo de viaje en horas * salario por hora
        double travelHours = distanceKm / _config.AverageSpeedKmH;
        return travelHours * _config.DriverHourlyCost;
    }

    /// <summary>
    /// Calcula el costo de las paradas (carga/descarga)
    /// </summary>
    private double CalculateStopCost(int numberOfStops)
    {
        // Costo de tiempo de parada + mano de obra
        return numberOfStops * _config.CostPerStop;
    }

    /// <summary>
    /// Calcula la depreciación del vehículo
    /// </summary>
    private double CalculateDepreciation(double distanceKm)
    {
        return distanceKm * _config.DepreciationPerKm;
    }

    /// <summary>
    /// Calcula el costo de mantenimiento
    /// </summary>
    private double CalculateMaintenanceCost(double distanceKm)
    {
        return distanceKm * _config.MaintenancePerKm;
    }

    /// <summary>
    /// Calcula el costo por ciudad visitada
    /// </summary>
    public Dictionary<string, CityLogisticCost> CalculateCostPerCity(
        City[] route,
        City startCity)
    {
        var costs = new Dictionary<string, CityLogisticCost>();
        double accumulatedDistance = 0.0;
        double accumulatedCost = 0.0;

        costs[route[0].Name] = new CityLogisticCost
        {
            CityName = route[0].Name,
            DistanceFromPrevious = 0,
            AccumulatedDistance = 0,
            SegmentCost = 0,
            AccumulatedCost = _config.DailyFixedCost / route.Length, // Prorrateo de costos fijos
            IsStartCity = route[0].Equals(startCity)
        };

        for (int i = 1; i < route.Length; i++)
        {
            double segmentDistance = GeoDistance.Between(route[i - 1], route[i]);
            accumulatedDistance += segmentDistance;

            // Costo de este segmento
            double segmentCost = CalculateFuelCost(segmentDistance) +
                                CalculateTollCost(segmentDistance) / route.Length +
                                CalculateDriverCost(segmentDistance) +
                                _config.CostPerStop +
                                CalculateDepreciation(segmentDistance) +
                                CalculateMaintenanceCost(segmentDistance);

            accumulatedCost += segmentCost;

            costs[route[i].Name] = new CityLogisticCost
            {
                CityName = route[i].Name,
                DistanceFromPrevious = segmentDistance,
                AccumulatedDistance = accumulatedDistance,
                SegmentCost = segmentCost,
                AccumulatedCost = accumulatedCost,
                IsStartCity = route[i].Equals(startCity)
            };
        }

        return costs;
    }

    /// <summary>
    /// Imprime un reporte detallado de costos
    /// </summary>
    public static void PrintCostReport(
        LogisticCostBreakdown breakdown,
        Dictionary<string, CityLogisticCost> costPerCity)
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════");
        Console.WriteLine("           ANÁLISIS DE COSTOS LOGÍSTICOS");
        Console.WriteLine("═══════════════════════════════════════════════════════\n");

        Console.WriteLine("DESGLOSE DE COSTOS:");
        Console.WriteLine("───────────────────────────────────────────────────────");
        Console.WriteLine($"{"Combustible:",-30} ${breakdown.FuelCost,10:F2}");
        Console.WriteLine($"{"Peajes:",-30} ${breakdown.TollCost,10:F2}");
        Console.WriteLine($"{"Conductor (tiempo):",-30} ${breakdown.DriverCost,10:F2}");
        Console.WriteLine($"{"Paradas/Descarga:",-30} ${breakdown.StopCost,10:F2}");
        Console.WriteLine($"{"Depreciación vehículo:",-30} ${breakdown.VehicleDepreciation,10:F2}");
        Console.WriteLine($"{"Mantenimiento:",-30} ${breakdown.MaintenanceCost,10:F2}");
        Console.WriteLine($"{"Costos fijos:",-30} ${breakdown.FixedCosts,10:F2}");
        Console.WriteLine(new string('─', 55));
        Console.WriteLine($"{"TOTAL:",-30} ${breakdown.TotalCost,10:F2}");
        Console.WriteLine($"{"Costo por km:",-30} ${breakdown.CostPerKm,10:F2}");
        Console.WriteLine($"{"Distancia total:",-30} {breakdown.TotalDistance,10:F2} km");
        Console.WriteLine($"{"Número de paradas:",-30} {breakdown.NumberOfStops,10}");

        Console.WriteLine("\n\nCOSTOS POR CIUDAD:");
        Console.WriteLine("───────────────────────────────────────────────────────");
        Console.WriteLine("{0,-20} {1,-12} {2,-15} {3,-15}",
            "Ciudad", "Dist. Segm.", "Costo Segm.", "Costo Acum.");
        Console.WriteLine(new string('─', 70));

        foreach (var kvp in costPerCity)
        {
            var cost = kvp.Value;
            Console.WriteLine("{0,-20} {1,-12:F2} ${2,-14:F2} ${3,-14:F2}",
                cost.CityName,
                cost.DistanceFromPrevious,
                cost.SegmentCost,
                cost.AccumulatedCost);
        }

        Console.WriteLine("═══════════════════════════════════════════════════════\n");
    }
}
