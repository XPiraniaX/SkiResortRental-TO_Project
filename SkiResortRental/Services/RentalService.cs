using SkiResortRental.Domain;
using SkiResortRental.Interfaces;

namespace SkiResortRental.Services;

public class RentalService
{
    private readonly List<IRentalObserver> _observers = new();
    private readonly List<Rental> _activeRentals = new();
    private readonly IPricingStrategy _pricingStrategy;
    
    public IReadOnlyList<Rental> ActiveRentals => _activeRentals;

    public RentalService(IPricingStrategy pricingStrategy)
    {
        _pricingStrategy = pricingStrategy;
    }

    
    public void AddObserver(IRentalObserver observer)
    {
        _observers.Add(observer);
    }

    public Rental Rent(Client client, InventoryItem item)
    {
        var price = _pricingStrategy.CalculatePrice(1,item.Equipment.BaseDailyPrice);
        
        var rental = new Rental(client, item, price);
        _activeRentals.Add(rental);

        NotifyRented(rental);
        return rental;
    }

    public void Return(Rental rental)
    {
        rental.Finish();
        _activeRentals.Remove(rental);

        NotifyReturned(rental);
    }

    private void NotifyRented(Rental rental)
    {
        foreach (var observer in _observers)
            observer.OnItemRented(rental);
    }

    private void NotifyReturned(Rental rental)
    {
        foreach (var observer in _observers)
            observer.OnItemReturned(rental);
    }
    
    public decimal GetDailyPrice(Equipment equipment)
    {
        return _pricingStrategy.CalculatePrice(1, equipment.BaseDailyPrice);
    }
}
