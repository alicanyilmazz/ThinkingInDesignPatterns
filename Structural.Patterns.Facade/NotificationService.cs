using System;
using System.Collections.Generic;
using System.Text;

namespace Structural.Patterns.Facade;

public class NotificationService
{
    public void SendPaymentSuccess(string email, int orderId)
    {
        Console.WriteLine("Bildirim gönderiliyor...");

        Console.WriteLine($"Email: {email}");

        Console.WriteLine($"OrderId: {orderId}");

        Console.WriteLine("Ödeme başarılı bildirimi gönderildi.");
    }
}