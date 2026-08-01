using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralPatterns.Decorator;

public class LoggingPaymentDecorator : IPaymentService
{
    private readonly IPaymentService _paymentService;

    public LoggingPaymentDecorator(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine("Log started");

        _paymentService.Pay(amount);

        Console.WriteLine("Log finished");
    }
}