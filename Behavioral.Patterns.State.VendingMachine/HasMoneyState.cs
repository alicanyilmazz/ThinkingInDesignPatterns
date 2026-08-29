namespace Behavioral.Patterns.State.VendingMachine;

public class HasMoneyState : IVendingMachineState
{
    private readonly VendingMachine _machine;

    public HasMoneyState(VendingMachine machine)
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

        Console.WriteLine($"{amount} TL daha yatırıldı. Bakiye: {_machine.Balance} TL");
    }

    public void SelectProduct(string productName)
    {
        decimal price = GetProductPrice(productName);

        if (price == 0)
        {
            Console.WriteLine("Ürün bulunamadı.");
            return;
        }

        if (_machine.Balance < price)
        {
            Console.WriteLine($"Yetersiz bakiye. Ürün fiyatı: {price} TL, " + $"Bakiye: {_machine.Balance} TL");
            return;
        }

        _machine.Select(productName, price);

        Console.WriteLine($"{productName} seçildi. Fiyat: {price} TL");

        _machine.ChangeState(new ProductSelectedState(_machine));
    }

    public void Dispense()
    {
        Console.WriteLine("Önce bir ürün seçmelisiniz.");
    }

    public void Cancel()
    {
        Console.WriteLine($"{_machine.Balance} TL iade edildi.");

        _machine.Clear();

        _machine.ChangeState(new NoMoneyState(_machine));
    }

    private decimal GetProductPrice(string productName)
    {
        return productName.ToLower() switch
        {
            "cola" => 30,
            "water" => 10,
            "chips" => 25,
            _ => 0
        };
    }
}