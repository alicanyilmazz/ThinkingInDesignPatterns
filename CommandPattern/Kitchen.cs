using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern;

// ======================================================
// RECEIVER
// Gerçek işi yapan sınıf
// ======================================================

public class Kitchen
{
    public void PrepareBurger(string customerName)
    {
        Console.WriteLine($"Burger is being prepared for {customerName}.");
    }

    public void PreparePizza(string customerName)
    {
        Console.WriteLine($"Pizza is being prepared for {customerName}.");
    }

    public void PrepareCoffee(string customerName)
    {
        Console.WriteLine($"Coffee is being prepared for {customerName}.");
    }
}