using SkiResortRental.Interfaces;

namespace SkiResortRental.Strategies;

public class HighSeasonPricing : IPricingStrategy
{
    public decimal CalculatePrice(int days, decimal baseDailyPrice)
    {
        return days * baseDailyPrice * 1.3m;
    }
}