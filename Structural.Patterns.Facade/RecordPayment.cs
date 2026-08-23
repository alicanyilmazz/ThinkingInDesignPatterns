using System;
using System.Collections.Generic;
using System.Text;

namespace Structural.Patterns.Facade;

public class LedgerService
{
    public void RecordPayment(int orderId, decimal amount)
    {
        Console.WriteLine("Muhasebe kaydı oluşturuluyor...");

        Console.WriteLine($"OrderId: {orderId}");

        Console.WriteLine($"Tutar: {amount:N2} TL");

        Console.WriteLine("Muhasebe kaydı tamamlandı.");
    }
}