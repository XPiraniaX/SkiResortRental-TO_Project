using SkiResortRental.Domain;

namespace SkiResortRental.Interfaces;

public interface IRentalObserver
{
    void OnItemRented(Rental rental);
    void OnItemReturned(Rental rental);
}
