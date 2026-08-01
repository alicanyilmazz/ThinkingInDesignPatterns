namespace Strategy.Strategy;

public sealed class BankTransferPaymentStrategy
    : IPaymentStrategy
{
    private readonly ILogger<BankTransferPaymentStrategy> _logger;

    public BankTransferPaymentStrategy(
        ILogger<BankTransferPaymentStrategy> logger)
    {
        _logger = logger;
    }

    public PaymentType Type => PaymentType.BankTransfer;

    public async Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Iban))
        {
            throw new ArgumentException(
                "Bank transfer requires an IBAN.");
        }

        _logger.LogInformation(
            "Bank transfer started. Reference: {Reference}",
            request.ReferenceNumber);

        // Gerçekte burada transfer servisi çağrılır.
        await Task.Delay(100, cancellationToken);

        decimal commission = request.Amount * 0.005m;

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString("N"),
            Commission: commission,
            Message: "Bank transfer completed.");
    }
}