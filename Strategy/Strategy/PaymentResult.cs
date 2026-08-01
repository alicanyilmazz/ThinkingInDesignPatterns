namespace Strategy.Strategy;

public sealed record PaymentResult(
    bool IsSuccess,
    string TransactionId,
    decimal Commission,
    string Message);
