namespace Behavioral.Patterns.State.VendingMachine;

public class ProductSelectedState : IVendingMachineState
{
    private readonly VendingMachine _machine;

    public ProductSelectedState(VendingMachine machine)
    {
        _machine = machine;
    }

    public void InsertMoney(decimal amount)
    {
        Console.WriteLine("Ürün zaten seçildi. Önce işlemi tamamlayın veya iptal edin.");
    }

    public void SelectProduct(string productName)
    {
        Console.WriteLine($"Zaten {_machine.SelectedProduct} ürünü seçildi.");
    }

    public void Dispense()
    {
        Console.WriteLine($"{_machine.SelectedProduct} hazırlanıyor...");

        _machine.ChangeState(new DispensingState(_machine));

        _machine.Dispense();
    }

    public void Cancel()
    {
        Console.WriteLine($"İşlem iptal edildi. {_machine.Balance} TL iade edildi.");

        _machine.Clear();

        _machine.ChangeState(new NoMoneyState(_machine));
    }
}