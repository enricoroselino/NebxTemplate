using Shared.Models.Exceptions;

namespace Shared.Models.ValueObjects;

public record GeoCoordinate
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double Altitude { get; init; }

    public GeoCoordinate(double longitude, double latitude)
    {
        EnsureLongitude(longitude);
        EnsureLatitude(latitude);
        
        Latitude = latitude;
        Longitude = longitude;
        Altitude = 0;
    }

    public GeoCoordinate(double longitude, double latitude, double altitude)
    {
        EnsureLongitude(longitude);
        EnsureLatitude(latitude);
        
        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
    }

    private static void EnsureLongitude(double longitude)
    {
        if (longitude is < -180 or > 180) throw new DomainException($"Longitude {longitude} is out of range");
    }

    private static void EnsureLatitude(double latitude)
    {
        if (latitude is < -90 or > 90) throw new DomainException($"Latitude {latitude} is out of range");
    }
}