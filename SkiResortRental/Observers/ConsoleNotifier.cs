using SkiResortRental.Domain;
using SkiResortRental.Interfaces;

namespace SkiResortRental.Observers;

public class ConsoleNotifier : IRentalObserver
{
    public void OnItemRented(Rental rental)
    {
        Console.WriteLine(
            $"[INFO] Wypożyczono: {rental.Item.Equipment.Name} dla {rental.Client.Name}"
        );
    }

    public void OnItemReturned(Rental rental)
    {
        Console.WriteLine(
            $"[INFO] Zwrócono: {rental.Item.Equipment.Name} od {rental.Client.Name}"
        );
    }
}
