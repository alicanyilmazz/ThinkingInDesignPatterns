namespace BehavioralPatterns.Strategy.Strategies;

public sealed class VisaCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.02m;
    }
}