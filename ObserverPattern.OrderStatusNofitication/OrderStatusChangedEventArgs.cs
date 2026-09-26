namespace ObserverPattern.OrderStatusNofitication;

public class OrderStatusChangedEventArgs : EventArgs
{
    public int OrderId { get; init; }
    public string OldStatus { get; init; } = null!;
    public string NewStatus { get; init; } = null!;
}