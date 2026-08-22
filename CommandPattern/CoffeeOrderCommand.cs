using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern;

// ======================================================
// CONCRETE COMMAND
// ======================================================

public class CoffeeOrderCommand : IOrderCommand
{
    private readonly Kitchen _kitchen;
    private readonly string _customerName;

    public CoffeeOrderCommand(Kitchen kitchen,string customerName)
    {
        _kitchen = kitchen;
        _customerName = customerName;
    }

    public void Execute()
    {
        _kitchen.PrepareCoffee(_customerName);
    }
}