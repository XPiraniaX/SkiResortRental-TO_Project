namespace SkiResortRental.Domain;

public class Ski : Equipment
{
    public int Length { get; }

    public Ski(string name, int length)
    {
        Name = name;
        Length = length;
    }

    public override decimal BaseDailyPrice => 60;
    public override decimal Deposit => 180;

}