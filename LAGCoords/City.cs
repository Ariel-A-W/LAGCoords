namespace LAGCoords;

public class City
{
    public string Name { get; }
    public double Latitude { get; }
    public double Longitude { get; }

    public City(string name, double latitude, double longitude)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    // Sobrescribir Equals y GetHashCode para uso correcto en HashSet
    public override bool Equals(object? obj)
    {
        if (obj is City other)
           return Name == other.Name &&
                  Latitude == other.Latitude &&
                  Longitude == other.Longitude;        

        return false;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Name, Latitude, Longitude);
}