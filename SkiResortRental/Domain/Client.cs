namespace SkiResortRental.Domain;

public class Client
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }

    public Client(string name)
    {
        Name = name;
    }
}