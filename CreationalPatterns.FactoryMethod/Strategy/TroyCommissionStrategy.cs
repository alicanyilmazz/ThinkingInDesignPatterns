namespace CreationalPatterns.FactoryMethod.Strategy;

public sealed class TroyCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.01m;
    }
}