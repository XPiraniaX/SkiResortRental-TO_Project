namespace SkiResortRental.Domain;

public class Snowboard : Equipment
{
    public int Length { get; }

    public Snowboard(string name, int length)
    {
        Name = name;
        Length = length;
    }

    public override decimal BaseDailyPrice => 50;
    public override decimal Deposit => 150;

}