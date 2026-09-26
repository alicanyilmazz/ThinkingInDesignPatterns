namespace Behavioral.Patterns.Observer;

public class EmailStockObserver
{
    public void HandleStockChanged(object? sender, StockChangedEventArgs e)
    {
        Console.WriteLine($"EMAIL -> {e.ProductName}: {e.OldStock} -> {e.NewStock}");
    }
}