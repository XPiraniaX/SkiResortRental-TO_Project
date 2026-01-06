namespace SkiResortRental.Domain;

public class Googles : Equipment
{
    
    public Googles(string name)
    {
        Name = name;
    }

    public override decimal BaseDailyPrice => 10;
    public override decimal Deposit => 30;

}