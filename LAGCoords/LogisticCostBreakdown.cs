namespace LAGCoords;

public class LogisticCostBreakdown
{
    public double FuelCost { get; set; }
    public double TollCost { get; set; }
    public double DriverCost { get; set; }
    public double StopCost { get; set; }
    public double VehicleDepreciation { get; set; }
    public double MaintenanceCost { get; set; }
    public double FixedCosts { get; set; }
    public double TotalCost { get; set; }
    public double TotalDistance { get; set; }
    public int NumberOfStops { get; set; }
    public double CostPerKm { get; set; }
}
