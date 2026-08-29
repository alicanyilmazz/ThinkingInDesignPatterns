namespace Behavioral.Patterns.State.VendingMachine;

public interface IVendingMachineState
{
    void InsertMoney(decimal amount);
    void SelectProduct(string productName);
    void Dispense();
    void Cancel();
}