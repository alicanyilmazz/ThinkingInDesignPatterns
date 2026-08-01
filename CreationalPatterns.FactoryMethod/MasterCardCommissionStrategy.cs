namespace CreationalPatterns.FactoryMethod;

public sealed class MasterCardCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.03m;
    }
}