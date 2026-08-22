using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern;

// ======================================================
// CONCRETE COMMAND
// ======================================================

public class PizzaOrderCommand : IOrderCommand
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
        _kitchen.PreparePizza(_customerName);
    }
}