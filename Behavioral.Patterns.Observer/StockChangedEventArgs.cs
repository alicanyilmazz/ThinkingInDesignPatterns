namespace Behavioral.Patterns.Observer;

public class StockChangedEventArgs : EventArgs
{
    public string ProductName { get; init; } = null!;
    public int OldStock { get; init; }
    public int NewStock { get; init; }
}