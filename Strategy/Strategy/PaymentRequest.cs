namespace Strategy.Strategy;

public sealed record PaymentRequest(
    PaymentType Type,
    decimal Amount,
    string ReferenceNumber,
    string? CardToken,
    string? Iban,
    string? WalletId);

