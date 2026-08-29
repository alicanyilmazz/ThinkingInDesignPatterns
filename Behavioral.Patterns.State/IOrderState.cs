namespace Behavioral.Patterns.State;

public interface IOrderState
{
    string Name { get; }

    void Pay(Order order);

    void Ship(Order order);

    void Deliver(Order order);

    void Cancel(Order order);
}
