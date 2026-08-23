using System;
using System.Collections.Generic;
using System.Text;

namespace Structural.Patterns.Facade;

public class FraudService
{
    public bool CheckFraud(string cardNumber, decimal amount)
    {
        Console.WriteLine("Fraud kontrolü yapılıyor...");

        Console.WriteLine($"Kart: {cardNumber}");

        Console.WriteLine($"Tutar: {amount:N2} TL");

        Console.WriteLine("Fraud kontrolü başarılı.");

        return true;
    }
}
