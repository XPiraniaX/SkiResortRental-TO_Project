namespace SkiResortRental.Domain;


public class Helmet : Equipment
{
    public int Size { get; }
    
    public Helmet(string name, int size)
    {
        Name = name;
        Size = size;
    }

    public override decimal BaseDailyPrice => 20;
    public override decimal Deposit => 60;
}