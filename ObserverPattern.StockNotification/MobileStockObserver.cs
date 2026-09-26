namespace ObserverPattern.StockNotification;

public class MobileStockObserver : IObserver
{
    public void Update(ProductStock stock)
    {
        Console.WriteLine($"PUSH -> {stock.ProductName} stock changed to {stock.Stock}");
    }
}