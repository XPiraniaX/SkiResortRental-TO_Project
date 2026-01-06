using SkiResortRental.Domain;

namespace SkiResortRental.Services;
    
public class InventoryStatisticsService
{
    public (int total, int available, int rented) GetGlobalStats(
        IEnumerable<InventoryItem> inventory)
    {
        int total = inventory.Sum(i => i.TotalQuantity);
        int rented = inventory.Sum(i => i.RentedQuantity);

        return (total, total - rented, rented);
    }
}