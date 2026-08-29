namespace Behavioral.Patterns.State.States;

public class ShippedOrderState : IOrderState
{
    public string Name => "Shipped";

    public void Pay(Order order)
    {
        Console.WriteLine("Siparişin ödemesi zaten yapılmış.");
    }

    public void Ship(Order order)
    {
        Console.WriteLine("Sipariş zaten kargoda.");
    }

    public void Deliver(Order order)
    {
        Console.WriteLine("Sipariş müşteriye teslim edildi.");

        order.ChangeState(new DeliveredOrderState());
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("Kargoya verilmiş sipariş iptal edilemez.");
    }
}