namespace SkiResortRental.Domain;

public class Rental
{
    public Client Client { get; }
    public InventoryItem Item { get; }
    public DateTime StartDate { get; } = DateTime.Now;

    public decimal DepositAmount { get; }
    public decimal PriceAmount { get; }
    public Rental(Client client, InventoryItem item, decimal priceAmount)
    {
        Client = client;
        Item = item;
        PriceAmount = priceAmount;
        DepositAmount = item.Equipment.Deposit;
        item.Rent();
    }

    public decimal Finish()
    {
        Item.Return();
        return 0;
    }

}