namespace Behavioral.Patterns.State.VendingMachine;

public class NoMoneyState : IVendingMachineState
{
    private readonly VendingMachine _machine;

    public NoMoneyState(VendingMachine machine)
    {
        _machine = machine;
    }

    public void InsertMoney(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Geçersiz para miktarı.");
            return;
        }

        _machine.AddBalance(amount);

        Console.WriteLine($"{amount} TL para yatırıldı. Bakiye: {_machine.Balance} TL");

        _machine.ChangeState(new HasMoneyState(_machine));
    }

    public void SelectProduct(string productName)
    {
        Console.WriteLine("Önce para yatırmalısınız.");
    }

    public void Dispense()
    {
        Console.WriteLine("Ürün seçilmedi ve para yatırılmadı.");
    }

    public void Cancel()
    {
        Console.WriteLine("İptal edilecek bir işlem bulunmuyor.");
    }
}