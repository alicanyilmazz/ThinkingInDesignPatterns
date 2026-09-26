namespace ObserverPattern.OrderStatusNofitication;

public class Order
{
    public int Id { get; }

    public string Status { get; private set; }

    public event EventHandler<OrderStatusChangedEventArgs>? StatusChanged;

    public Order(int id)
    {
        Id = id;
        Status = "Pending";
    }

    public void ChangeStatus(string newStatus)
    {
        var oldStatus = Status;

        Status = newStatus;

        StatusChanged?.Invoke(this, new OrderStatusChangedEventArgs { OrderId = Id, OldStatus = oldStatus, NewStatus = newStatus });
    }
}