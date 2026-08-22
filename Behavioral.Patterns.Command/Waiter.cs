using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Command;

public class Waiter
{
    public void TakeOrder(ICommand command)
    {
        Console.WriteLine("Garson siparişi aldı.");

        command.Execute();
    }
}