public interface IPaymentGateway
{
    void Pay(decimal amount);
}
public interface IFraudClient
{
    bool Check(decimal amount);
}
public interface INotificationClient
{
    void Send(string message);
}
public interface IFinancialProviderFactory
{
    IPaymentGateway CreatePaymentGateway();

    IFraudClient CreateFraudClient();

    INotificationClient CreateNotificationClient();
}

public sealed class ProviderAFactory : IFinancialProviderFactory
{
    public IPaymentGateway CreatePaymentGateway()
    {
        return new ProviderAPaymentGateway();
    }

    public IFraudClient CreateFraudClient()
    {
        return new ProviderAFraudClient();
    }

    public INotificationClient CreateNotificationClient()
    {
        return new ProviderANotificationClient();
    }
}

public sealed class ProviderBFactory : IFinancialProviderFactory
{
    public IPaymentGateway CreatePaymentGateway()
    {
        return new ProviderBPaymentGateway();
    }

    public IFraudClient CreateFraudClient()
    {
        return new ProviderBFraudClient();
    }

    public INotificationClient CreateNotificationClient()
    {
        return new ProviderBNotificationClient();
    }
}

// Implementations for Provider A
IFinancialProviderFactory factory = new ProviderAFactory();

IPaymentGateway paymentGateway = factory.CreatePaymentGateway();

IFraudClient fraudClient = factory.CreateFraudClient();

INotificationClient notificationClient = factory.CreateNotificationClient();

decimal amount = 1000;

bool isFraud = fraudClient.Check(amount);

if (!isFraud)
{
    paymentGateway.Pay(amount);

    notificationClient.Send("Payment completed successfully.");
}
else
{
    notificationClient.Send("Payment blocked because of fraud detection.");
}