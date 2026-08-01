namespace BehavioralPatterns.Strategy.Strategies;

public interface ICommissionStrategy
{
    decimal Calculate(decimal amount);
}

