using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralPatterns.Decorator;

public class PaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Payment : {amount}");
    }
}