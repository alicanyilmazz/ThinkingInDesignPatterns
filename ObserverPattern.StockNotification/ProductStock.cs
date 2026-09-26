namespace ObserverPattern.StockNotification;

public class ProductStock : ISubject
{
    private readonly List<IObserver> _observers = [];

    public string ProductName { get; }
    public int Stock { get; private set; }

    public ProductStock(string productName)
    {
        ProductName = productName;
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void UpdateStock(int stock)
    {
        Stock = stock;

        Notify();
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}