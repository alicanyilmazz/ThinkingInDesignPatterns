namespace Behavioral.Patterns.Observer;

public class ProductStock
{
    public string ProductName { get; }

    public int Stock { get; private set; }

    public event EventHandler<StockChangedEventArgs>? StockChanged;

    public ProductStock(string productName, int stock = 0)
    {
        ProductName = productName;
        Stock = stock;
    }

    public void UpdateStock(int newStock)
    {
        var oldStock = Stock;

        Stock = newStock;

        OnStockChanged(oldStock, newStock);
    }

    protected virtual void OnStockChanged(int oldStock, int newStock)
    {
        StockChanged?.Invoke(
            this,
            new StockChangedEventArgs
            {
                ProductName = ProductName,
                OldStock = oldStock,
                NewStock = newStock
            });
    }
}