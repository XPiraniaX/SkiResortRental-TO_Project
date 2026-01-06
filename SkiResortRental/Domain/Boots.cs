namespace SkiResortRental.Domain;

public class Boots : Equipment
{
    public int Size { get; }

    public Boots(string name, int size)
    {
        Name = name;
        Size = size;
    }

    public override decimal BaseDailyPrice => 40;
    public override decimal Deposit => 120;

}
