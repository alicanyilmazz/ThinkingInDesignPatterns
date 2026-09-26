namespace ObserverPattern.StockNotification;

public interface IObserver
{
    void Update(ProductStock stock);
}
