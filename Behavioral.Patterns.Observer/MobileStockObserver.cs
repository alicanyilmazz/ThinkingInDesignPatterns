namespace Behavioral.Patterns.Observer;

public class MobileStockObserver
{
    public void HandleStockChanged(object? sender, StockChangedEventArgs e)
    {
        Console.WriteLine($"PUSH -> {e.ProductName}: {e.OldStock} -> {e.NewStock}");
    }
}