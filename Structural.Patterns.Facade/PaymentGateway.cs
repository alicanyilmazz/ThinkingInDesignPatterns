using System;
using System.Collections.Generic;
using System.Text;

namespace Structural.Patterns.Facade;

public class PaymentGateway
{
    public bool Charge(string cardNumber,decimal amount)
    {
        Console.WriteLine("Ödeme alınıyor...");

        Console.WriteLine($"Kart: {cardNumber}");

        Console.WriteLine($"Tutar: {amount:N2} TL");

        Console.WriteLine("Ödeme başarıyla alındı.");

        return true;
    }
}
