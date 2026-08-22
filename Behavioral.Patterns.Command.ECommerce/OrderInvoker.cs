namespace Behavioral.Patterns.Command.ECommerce;

public class OrderInvoker
{
    public void ExecuteCommand(ICommand command)
    {
        Console.WriteLine("İşlem alındı.");

        command.Execute();

        Console.WriteLine("İşlem tamamlandı.");
    }
}