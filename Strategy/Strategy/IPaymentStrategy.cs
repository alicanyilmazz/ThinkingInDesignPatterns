namespace Strategy.Strategy;

public interface IPaymentStrategy
{
    PaymentType Type { get; }

    Task<PaymentResult> PayAsync(PaymentRequest request,CancellationToken cancellationToken);
}