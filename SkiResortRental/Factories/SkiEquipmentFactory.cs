using SkiResortRental.Domain;

namespace SkiResortRental.Factories;

public static class SkiEquipmentFactory
{
    public static Ski CreateSki(string name, int length)
        => new Ski(name, length);
    
    public static Snowboard CreateSnowboard(string name, int length)
        => new Snowboard(name, length);

    public static Boots CreateBoots(string name, int size)
        => new Boots(name, size);

    public static Helmet CreateHelmet(string name, int size)
        => new Helmet(name, size);
    
    public static Googles CreateGoogles(string name)
        => new Googles(name);
}