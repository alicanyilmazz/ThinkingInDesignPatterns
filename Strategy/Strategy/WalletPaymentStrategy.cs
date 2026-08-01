namespace Strategy.Strategy;

public sealed class WalletPaymentStrategy
    : IPaymentStrategy
{
    private readonly ILogger<WalletPaymentStrategy> _logger;

    public WalletPaymentStrategy(
        ILogger<WalletPaymentStrategy> logger)
    {
        _logger = logger;
    }

    public PaymentType Type => PaymentType.Wallet;

    public async Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.WalletId))
        {
            throw new ArgumentException(
                "Wallet payment requires a wallet ID.");
        }

        _logger.LogInformation(
            "Wallet payment started. Reference: {Reference}",
            request.ReferenceNumber);

        // Gerçekte wallet servisi çağrılır.
        await Task.Delay(100, cancellationToken);

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString("N"),
            Commission: 0,
            Message: "Wallet payment completed.");
    }
}