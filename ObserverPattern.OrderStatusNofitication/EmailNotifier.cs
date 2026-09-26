namespace ObserverPattern.OrderStatusNofitication;

public class EmailNotifier
{
    public void OnStatusChanged(object? sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"Email -> Order {e.OrderId}: {e.OldStatus} -> {e.NewStatus}");
    }
}