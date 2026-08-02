Console.WriteLine("Hello, World!");

// Observer olmadan problem

/*
 
public sealed class OrderService
{
    private readonly EmailService _emailService;
    private readonly StockService _stockService;
    private readonly InvoiceService _invoiceService;

    public OrderService(
        EmailService emailService,
        StockService stockService,
        InvoiceService invoiceService)
    {
        _emailService = emailService;
        _stockService = stockService;
        _invoiceService = invoiceService;
    }

    public void CreateOrder(Order order)
    {
        Console.WriteLine("Sipariş oluşturuldu.");

        _emailService.Send(order);
        _stockService.Reduce(order);
        _invoiceService.Create(order);
    }
}

*/