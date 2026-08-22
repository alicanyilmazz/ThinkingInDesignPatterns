
// ======================================================
// PROGRAM.CS
// CLIENT
// ======================================================

using CommandPattern;

public class Program
{
    public static void Main()
    {
        // Receiver
        var kitchen = new Kitchen();


        // Invoker
        var waiter = new Waiter();


        // Commands
        IOrderCommand order1 = new BurgerOrderCommand(kitchen,"Ali");

        IOrderCommand order2 = new PizzaOrderCommand(kitchen,"John");

        IOrderCommand order3 = new CoffeeOrderCommand(kitchen,"Sarah");


        // Garson siparişleri alıyor.
        // Henüz mutfakta hazırlanmadılar.

        waiter.TakeOrder(order1);
        waiter.TakeOrder(order2);
        waiter.TakeOrder(order3);


        Console.WriteLine("Orders received.");

        Console.WriteLine();


        // Garson siparişleri mutfağa gönderiyor.

        waiter.SendOrdersToKitchen();
    }
}








