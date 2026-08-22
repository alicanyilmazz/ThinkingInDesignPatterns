namespace Behavioral.Patterns.Command.BackgroundJob.Services;

public class EmailService
{
    public void SendEmail(string email,string message)
    {
        Console.WriteLine($"Email gönderiliyor...");

        Console.WriteLine($"Adres: {email}");

        Console.WriteLine($"Mesaj: {message}");

        Console.WriteLine("Email başarıyla gönderildi.");
    }
}