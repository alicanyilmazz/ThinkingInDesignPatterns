namespace Behavioral.Patterns.State.States;

public class DeliveredOrderState : IOrderState
{
    public string Name => "Delivered";

    public void Pay(Order order)
    {
        Console.WriteLine("Teslim edilmiş sipariş için tekrar ödeme yapılamaz.");
    }

    public void Ship(Order order)
    {
        Console.WriteLine("Sipariş zaten teslim edilmiş.");
    }

    public void Deliver(Order order)
    {
        Console.WriteLine("Sipariş zaten teslim edilmiş.");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("Teslim edilmiş sipariş iptal edilemez.");
    }
}