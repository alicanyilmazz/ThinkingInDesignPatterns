using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Command;

public class Kitchen
{
    public void MakePizza(string customerName)
    {
        Console.WriteLine($"{customerName} için pizza hazırlanıyor.");
    }

    public void MakeBurger(string customerName)
    {
        Console.WriteLine($"{customerName} için burger hazırlanıyor.");
    }
}
