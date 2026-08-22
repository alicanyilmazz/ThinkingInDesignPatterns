namespace Behavioral.Patterns.Command.BackgroundJob.Services;

public class PaymentService
{
    public void ProcessPayment(int orderId,decimal amount)
    {
        Console.WriteLine($"Order {orderId} için ödeme işleniyor.");

        Console.WriteLine($"Tutar: {amount:N2} TL");

        Console.WriteLine("Ödeme başarıyla tamamlandı.");
    }
}