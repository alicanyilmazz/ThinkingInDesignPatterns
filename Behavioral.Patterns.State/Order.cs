using Behavioral.Patterns.State.States;

namespace Behavioral.Patterns.State;

public class Order
{
    private IOrderState _state;

    public int Id { get; }

    public string ProductName { get; }

    public decimal Amount { get; }


    public string Status
    {
        get
        {
            return _state.Name;
        }
    }


    public Order(int id, string productName, decimal amount)
    {
        Id = id;
        ProductName = productName;
        Amount = amount;
        _state = new PendingOrderState();
    }


    public void ChangeState(IOrderState state)
    {
        Console.WriteLine($"State değişiyor: {_state.Name} → {state.Name}");

        _state = state;
    }


    public void Pay()
    {
        _state.Pay(this);
    }


    public void Ship()
    {
        _state.Ship(this);
    }


    public void Deliver()
    {
        _state.Deliver(this);
    }


    public void Cancel()
    {
        _state.Cancel(this);
    }
}
