using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern;

// ======================================================
// INVOKER
// Garson siparişleri topluyor.
// Nasıl hazırlanacağını bilmiyor.
// ======================================================

public class Waiter
{
    private readonly Queue<IOrderCommand> _orders = new();

    public void TakeOrder(IOrderCommand command)
    {
        _orders.Enqueue(command);
    }

    public void SendOrdersToKitchen()
    {
        while (_orders.Count > 0)
        {
            IOrderCommand command = _orders.Dequeue();

            command.Execute();
        }
    }
}
