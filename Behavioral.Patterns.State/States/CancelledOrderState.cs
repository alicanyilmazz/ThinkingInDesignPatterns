namespace Behavioral.Patterns.State.States;

public class CancelledOrderState : IOrderState
{
    public string Name => "Cancelled";

    public void Pay(Order order)
    {
        Console.WriteLine("İptal edilmiş sipariş için ödeme yapılamaz.");
    }

    public void Ship(Order order)
    {
        Console.WriteLine("İptal edilmiş sipariş kargoya verilemez.");
    }

    public void Deliver(Order order)
    {
        Console.WriteLine("İptal edilmiş sipariş teslim edilemez.");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("Sipariş zaten iptal edilmiş.");
    }
}