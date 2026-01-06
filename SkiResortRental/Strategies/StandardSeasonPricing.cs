using SkiResortRental.Interfaces;

namespace SkiResortRental.Strategies;

public class StandardSeasonPricing : IPricingStrategy
{
    public decimal CalculatePrice(int days, decimal baseDailyPrice)
    {
        return days * baseDailyPrice;
    }
}