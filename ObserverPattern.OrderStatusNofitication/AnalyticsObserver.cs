namespace ObserverPattern.OrderStatusNofitication;

public class AnalyticsObserver
{
    public void OnStatusChanged(object? sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"Analytics -> Order {e.OrderId} changed to {e.NewStatus}");
    }
}