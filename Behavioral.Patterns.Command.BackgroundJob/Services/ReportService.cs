namespace Behavioral.Patterns.Command.BackgroundJob.Services;

public class ReportService
{
    public void GenerateReport(int customerId)
    {
        Console.WriteLine($"CustomerId: {customerId} için rapor oluşturuluyor...");

        Console.WriteLine("Rapor başarıyla oluşturuldu.");
    }
}