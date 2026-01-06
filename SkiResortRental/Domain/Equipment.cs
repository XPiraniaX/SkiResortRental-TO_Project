namespace SkiResortRental.Domain;

public abstract class Equipment
{
    public string Name { get; protected set; }

    public abstract decimal BaseDailyPrice { get; }
    public abstract decimal Deposit { get; }
}