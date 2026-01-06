namespace SkiResortRental.Domain;

public class InventoryItem
{
    public Equipment Equipment { get; }
    public int TotalQuantity { get; private set; }
    public int RentedQuantity { get; private set; }

    public int AvailableQuantity => TotalQuantity - RentedQuantity;

    public InventoryItem(Equipment equipment, int quantity)
    {
        Equipment = equipment;
        TotalQuantity = quantity;
    }

    public void Rent()
    {
        if (AvailableQuantity <= 0)
            throw new InvalidOperationException("Brak dostępnych sztuk.");
        RentedQuantity++;
    }

    public void Return()
    {
        if (RentedQuantity <= 0)
            throw new InvalidOperationException("Brak wypożyczonych sztuk.");
        RentedQuantity--;
    }

    public void AddQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException();
        TotalQuantity += amount;
    }

    public void RemoveQuantity(int amount)
    {
        if (TotalQuantity - amount < RentedQuantity)
            throw new InvalidOperationException("Nie można usunąć wypożyczonego sprzętu.");
        TotalQuantity -= amount;
    }
}