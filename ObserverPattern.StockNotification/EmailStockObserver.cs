namespace ObserverPattern.StockNotification;

public class EmailStockObserver : IObserver
{
    public void Update(ProductStock stock)
    {
        Console.WriteLine($"EMAIL -> {stock.ProductName} stock changed to {stock.Stock}");
    }
}
