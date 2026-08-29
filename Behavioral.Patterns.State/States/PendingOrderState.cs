namespace Behavioral.Patterns.State.States;

public class PendingOrderState : IOrderState
{
    public string Name => "Pending";


    public void Pay(Order order)
    {
        Console.WriteLine("Ödeme alındı.");

        order.ChangeState(new PaidOrderState());
    }

    public void Ship(Order order)
    {
        Console.WriteLine("Sipariş ödeme yapılmadan kargoya verilemez.");
    }

    public void Deliver(Order order)
    {
        Console.WriteLine("Pending sipariş teslim edilemez.");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("Sipariş iptal edildi.");

        order.ChangeState(new CancelledOrderState());
    }
}