namespace LAGCoords;

public class LogisticConfig
{
    // Combustible
    public double FuelPricePerLiter { get; set; } = 950.0; // Precio en pesos argentinos
    public double KmPerLiter { get; set; } = 8.0; // Rendimiento del vehículo

    // Peajes
    public double AverageTollCost { get; set; } = 2500.0; // Costo promedio de peaje
    public double KmBetweenTolls { get; set; } = 150.0; // Distancia estimada entre peajes

    // Conductor
    public double DriverHourlyCost { get; set; } = 3500.0; // Salario + cargas sociales por hora
    public double AverageSpeedKmH { get; set; } = 70.0; // Velocidad promedio

    // Paradas
    public double CostPerStop { get; set; } = 5000.0; // Tiempo de carga/descarga + mano de obra

    // Vehículo
    public double DepreciationPerKm { get; set; } = 45.0; // Depreciación por km
    public double MaintenancePerKm { get; set; } = 35.0; // Mantenimiento por km

    // Costos fijos diarios
    public double DailyFixedCost { get; set; } = 15000.0; // Seguro, administración, etc.
}