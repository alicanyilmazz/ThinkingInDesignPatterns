namespace Strategy.Strategy;

public sealed class PaymentService
{
    private readonly IPaymentStrategyResolver _strategyResolver;

    public PaymentService(
        IPaymentStrategyResolver strategyResolver)
    {
        _strategyResolver = strategyResolver;
    }

    public Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request.Amount),
                "Payment amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(
                request.ReferenceNumber))
        {
            throw new ArgumentException(
                "Reference number is required.");
        }

        IPaymentStrategy strategy =
            _strategyResolver.Resolve(request.Type);

        return strategy.PayAsync(
            request,
            cancellationToken);
    }
}