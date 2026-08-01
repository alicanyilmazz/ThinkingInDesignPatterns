namespace Strategy.Strategy;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(PaymentType paymentType);
}
