namespace Behavioral.Patterns.State.VendingMachine;

public class DispensingState : IVendingMachineState
{
    private readonly VendingMachine _machine;

    public DispensingState(VendingMachine machine)
    {
        _machine = machine;
    }

    public void InsertMoney(decimal amount)
    {
        Console.WriteLine("Ürün verilirken para yatıramazsınız.");
    }

    public void SelectProduct(string productName)
    {
        Console.WriteLine("Ürün veriliyor. Yeni ürün seçemezsiniz.");
    }

    public void Dispense()
    {
        decimal change = _machine.Balance - _machine.SelectedProductPrice;

        Console.WriteLine($"{_machine.SelectedProduct} verildi.");

        if (change > 0)
        {
            Console.WriteLine($"Para üstü: {change} TL");
        }

        _machine.Clear();

        _machine.ChangeState(new NoMoneyState(_machine));

        Console.WriteLine("Makine yeni işlem için hazır.");
    }

    public void Cancel()
    {
        Console.WriteLine("Ürün verilmeye başladığı için işlem iptal edilemez.");
    }
}
