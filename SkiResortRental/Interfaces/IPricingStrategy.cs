namespace SkiResortRental.Interfaces;

public interface IPricingStrategy
{
    decimal CalculatePrice(int days, decimal baseDailyPrice);
}