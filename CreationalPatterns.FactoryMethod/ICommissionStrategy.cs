namespace CreationalPatterns.FactoryMethod;

public interface ICommissionStrategy
{
    decimal Calculate(decimal amount);
}