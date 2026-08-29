namespace Behavioral.Patterns.State.VendingMachine;

public class VendingMachine
{
    private IVendingMachineState _state;

    public decimal Balance { get; private set; }

    public string SelectedProduct { get; private set; }

    public decimal SelectedProductPrice { get; private set; }

    public VendingMachine()
    {
        _state = new NoMoneyState(this);
    }

    public void ChangeState(IVendingMachineState state)
    {
        _state = state;
    }

    public void AddBalance(decimal amount)
    {
        Balance += amount;
    }

    public void Select(string productName, decimal price)
    {
        SelectedProduct = productName;
        SelectedProductPrice = price;
    }

    public void Clear()
    {
        Balance = 0;
        SelectedProduct = null;
        SelectedProductPrice = 0;
    }

    public void InsertMoney(decimal amount)
    {
        _state.InsertMoney(amount);
    }

    public void SelectProduct(string productName)
    {
        _state.SelectProduct(productName);
    }

    public void Dispense()
    {
        _state.Dispense();
    }

    public void Cancel()
    {
        _state.Cancel();
    }
}