
IPaymentService paymentService = new LegacyBankAdapter(new LegacyBankApi());

paymentService.Pay(500);
public interface IPaymentService
{
    void Pay(decimal amount);
}

public class LegacyBankApi
{
    public void ExecutePayment(decimal amount)
    {
        Console.WriteLine($"Legacy payment : {amount}");
    }
}

public class LegacyBankAdapter : IPaymentService
{
    private readonly LegacyBankApi _legacyApi;

    public LegacyBankAdapter(LegacyBankApi legacyApi)
    {
        _legacyApi = legacyApi;
    }

    public void Pay(decimal amount)
    {
        _legacyApi.ExecutePayment(amount);
    }
}

//------------------------------------------------------------
 /*
 Bak burada sadece method ismini değil objeyi de çevirdi.
 Bu da Adapter'ın işi.
 */
public class SoapCustomerClient
{
    public SoapCustomer GetCustomer(int id)
    {
        return new SoapCustomer
        {
            Id = id,
            FullName = "John Doe"
        };
    }
}

public class SoapCustomerAdapter : ICustomerService
{
    private readonly SoapCustomerClient _client;

    public SoapCustomerAdapter(SoapCustomerClient client)
    {
        _client = client;
    }

    public CustomerDto Get(int id)
    {
        var soapCustomer = _client.GetCustomer(id);

        return new CustomerDto
        {
            Id = soapCustomer.Id,
            Name = soapCustomer.FullName
        };
    }
}