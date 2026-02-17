namespace LAGCoords;

public class DistanceInfo
{
    public string CityName { get; set; } = string.Empty;
    public double DirectDistanceFromStart { get; set; }
    public int PositionInRoute { get; set; }
    public bool IsStartCity { get; set; }
}
