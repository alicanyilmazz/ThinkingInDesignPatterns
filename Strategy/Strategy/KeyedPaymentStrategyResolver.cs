
namespace Strategy.Strategy;

public sealed class KeyedPaymentStrategyResolver
    : IPaymentStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public KeyedPaymentStrategyResolver(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentStrategy Resolve(PaymentType paymentType)
    {
        return _serviceProvider
            .GetRequiredKeyedService<IPaymentStrategy>(
                paymentType);
    }
}
