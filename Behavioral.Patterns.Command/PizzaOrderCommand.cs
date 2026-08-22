using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Command;

public class PizzaOrderCommand : ICommand
{
    private readonly Kitchen _kitchen;
    private readonly string _customerName;

    public PizzaOrderCommand(Kitchen kitchen,string customerName)
    {
        _kitchen = kitchen;
        _customerName = customerName;
    }

    public void Execute()
    {
        _kitchen.MakePizza(_customerName);
    }
}