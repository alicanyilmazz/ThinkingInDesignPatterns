namespace CreationalPatterns.FactoryMethod.Strategy;

public sealed class MasterCardCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.03m;
    }
}