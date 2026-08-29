namespace Behavioral.Patterns.State.States;

public class PaidOrderState : IOrderState
{
    public string Name => "Paid";

    public void Pay(Order order)
    {
        Console.WriteLine("Siparişin ödemesi zaten yapılmış.");
    }

    public void Ship(Order order)
    {
        Console.WriteLine("Sipariş kargoya verildi.");

        order.ChangeState(new ShippedOrderState());
    }

    public void Deliver(Order order)
    {
        Console.WriteLine("Sipariş önce kargoya verilmelidir.");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("Sipariş iptal edildi.");

        Console.WriteLine("Ödeme iade süreci başlatılabilir.");

        order.ChangeState(new CancelledOrderState());
    }
}