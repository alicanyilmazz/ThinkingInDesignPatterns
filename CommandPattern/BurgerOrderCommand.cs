using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern;

// ======================================================
// CONCRETE COMMAND
// ======================================================

public class BurgerOrderCommand : IOrderCommand
{
    private readonly Kitchen _kitchen;
    private readonly string _customerName;

    public BurgerOrderCommand(Kitchen kitchen,string customerName)
    {
        _kitchen = kitchen;
        _customerName = customerName;
    }

    public void Execute()
    {
        _kitchen.PrepareBurger(_customerName);
    }
}