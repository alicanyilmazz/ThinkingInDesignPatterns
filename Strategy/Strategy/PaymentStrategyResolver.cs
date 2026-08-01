namespace Strategy.Strategy;

public sealed class PaymentStrategyResolver
    : IPaymentStrategyResolver
{
    private readonly IReadOnlyDictionary<
        PaymentType,
        IPaymentStrategy> _strategies;

    public PaymentStrategyResolver(
        IEnumerable<IPaymentStrategy> strategies)
    {
        IPaymentStrategy[] strategyArray = strategies.ToArray();

        var duplicateType = strategyArray
            .GroupBy(strategy => strategy.Type)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateType is not null)
        {
            throw new InvalidOperationException(
                $"More than one strategy is registered for " +
                $"{duplicateType.Key}.");
        }

        _strategies = strategyArray.ToDictionary(
            strategy => strategy.Type);
    }

    public IPaymentStrategy Resolve(PaymentType paymentType)
    {
        if (_strategies.TryGetValue(
                paymentType,
                out IPaymentStrategy? strategy))
        {
            return strategy;
        }

        throw new NotSupportedException(
            $"No payment strategy is registered for {paymentType}.");
    }
}