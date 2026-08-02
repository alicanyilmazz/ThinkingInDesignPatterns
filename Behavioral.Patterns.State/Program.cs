using System;

namespace StatePatternDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var order = new OrderContext(new PendingState());

            order.Pay();
            Console.WriteLine();

            order.Ship();
            Console.WriteLine();

            order.Deliver();
            Console.WriteLine();

            order.Cancel();
            Console.WriteLine();

            Console.ReadLine();
        }
    }

    #region Context

    public class OrderContext
    {
        public IOrderState State { get; set; }

        public OrderContext(IOrderState state)
        {
            State = state;
        }

        public void Pay()
        {
            State.Pay(this);
        }

        public void Ship()
        {
            State.Ship(this);
        }

        public void Deliver()
        {
            State.Deliver(this);
        }

        public void Cancel()
        {
            State.Cancel(this);
        }
    }

    #endregion

    #region State Interface

    public interface IOrderState
    {
        void Pay(OrderContext context);

        void Ship(OrderContext context);

        void Deliver(OrderContext context);

        void Cancel(OrderContext context);
    }

    #endregion

    #region Pending

    public class PendingState : IOrderState
    {
        public void Pay(OrderContext context)
        {
            Console.WriteLine("Ödeme alındı.");
            Console.WriteLine("State : Pending -> Paid");

            context.State = new PaidState();
        }

        public void Ship(OrderContext context)
        {
            Console.WriteLine("Ödeme alınmadan kargoya verilemez.");
        }

        public void Deliver(OrderContext context)
        {
            Console.WriteLine("Henüz kargoya verilmedi.");
        }

        public void Cancel(OrderContext context)
        {
            Console.WriteLine("Sipariş iptal edildi.");
        }
    }

    #endregion

    #region Paid

    public class PaidState : IOrderState
    {
        public void Pay(OrderContext context)
        {
            Console.WriteLine("Sipariş zaten ödendi.");
        }

        public void Ship(OrderContext context)
        {
            Console.WriteLine("Sipariş kargoya verildi.");
            Console.WriteLine("State : Paid -> Shipped");

            context.State = new ShippedState();
        }

        public void Deliver(OrderContext context)
        {
            Console.WriteLine("Henüz kargoya verilmedi.");
        }

        public void Cancel(OrderContext context)
        {
            Console.WriteLine("Refund işlemi başlatıldı.");
        }
    }

    #endregion

    #region Shipped

    public class ShippedState : IOrderState
    {
        public void Pay(OrderContext context)
        {
            Console.WriteLine("Sipariş zaten ödendi.");
        }

        public void Ship(OrderContext context)
        {
            Console.WriteLine("Sipariş zaten kargoda.");
        }

        public void Deliver(OrderContext context)
        {
            Console.WriteLine("Sipariş teslim edildi.");
            Console.WriteLine("State : Shipped -> Completed");

            context.State = new CompletedState();
        }

        public void Cancel(OrderContext context)
        {
            Console.WriteLine("Kargoya verilen sipariş iptal edilemez.");
        }
    }

    #endregion

    #region Completed

    public class CompletedState : IOrderState
    {
        public void Pay(OrderContext context)
        {
            Console.WriteLine("Sipariş tamamlandı.");
        }

        public void Ship(OrderContext context)
        {
            Console.WriteLine("Sipariş teslim edildi.");
        }

        public void Deliver(OrderContext context)
        {
            Console.WriteLine("Sipariş zaten teslim edildi.");
        }

        public void Cancel(OrderContext context)
        {
            Console.WriteLine("Tamamlanan sipariş iptal edilemez.");
        }
    }

    #endregion
}
