namespace Strategy.Strategy;

public sealed class CreditCardPaymentStrategy
    : IPaymentStrategy
{
    private readonly ILogger<CreditCardPaymentStrategy> _logger;

    public CreditCardPaymentStrategy(
        ILogger<CreditCardPaymentStrategy> logger)
    {
        _logger = logger;
    }

    public PaymentType Type => PaymentType.CreditCard;

    public async Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CardToken))
        {
            throw new ArgumentException(
                "Credit card payment requires a card token.");
        }

        _logger.LogInformation(
            "Credit card payment started. Reference: {Reference}",
            request.ReferenceNumber);

        // Gerçekte burada kart/provizyon servisi çağrılır.
        await Task.Delay(100, cancellationToken);

        decimal commission = request.Amount * 0.02m;

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString("N"),
            Commission: commission,
            Message: "Credit card payment completed.");
    }
}