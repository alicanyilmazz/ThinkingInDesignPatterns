namespace CreationalPatterns.FactoryMethod.Strategy;

public interface ICommissionStrategy
{
    decimal Calculate(decimal amount);
}