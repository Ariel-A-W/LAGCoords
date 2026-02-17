namespace LAGCoords;

public static class GeoDistance
{
    private const double EarthRadiusKm = 6371.0;

    public static double Between(City a, City b)
    {
        double lat1 = ToRad(a.Latitude);
        double lon1 = ToRad(a.Longitude);
        double lat2 = ToRad(b.Latitude);
        double lon2 = ToRad(b.Longitude);

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double h = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dLon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(h));
        return EarthRadiusKm * c;
    }

    private static double ToRad(double deg)
        => deg * Math.PI / 180.0;
}