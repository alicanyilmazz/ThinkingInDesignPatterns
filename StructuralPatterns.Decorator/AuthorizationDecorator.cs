using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralPatterns.Decorator;

public class AuthorizationDecorator : IPaymentService
{
    private readonly IPaymentService _service;

    public AuthorizationDecorator(IPaymentService service)
    {
        _service = service;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine("Checking permission");

        _service.Pay(amount);
    }
}
