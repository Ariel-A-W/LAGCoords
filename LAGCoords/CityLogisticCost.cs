namespace LAGCoords;

public class CityLogisticCost
{
    public string CityName { get; set; } = string.Empty;
    public double DistanceFromPrevious { get; set; }
    public double AccumulatedDistance { get; set; }
    public double SegmentCost { get; set; }
    public double AccumulatedCost { get; set; }
    public bool IsStartCity { get; set; }
}
