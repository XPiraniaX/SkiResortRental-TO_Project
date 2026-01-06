using SkiResortRental.Domain;
using SkiResortRental.Interfaces;

namespace SkiResortRental.Services;

public class FinanceService : IRentalObserver
{
    public decimal Earned { get; private set; }
    public decimal Deposits { get; private set; }

    public void OnItemRented(Rental rental)
    {
        Earned += rental.PriceAmount;
        Deposits += rental.DepositAmount;
    }

    public void OnItemReturned(Rental rental)
    {
        Deposits -= rental.DepositAmount;
    }
}
