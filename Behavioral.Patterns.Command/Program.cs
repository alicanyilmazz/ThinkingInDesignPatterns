using Behavioral.Patterns.Command;

public sealed class CashService
{
    public void Withdraw(decimal amount)
    {
        Console.WriteLine($"Cash dispensed: {amount}");
    }
}

public sealed class WithdrawCommand : ICommand
{
    private readonly CashService _cashService;
    private readonly decimal _amount;

    public WithdrawCommand(CashService cashService,decimal amount)
    {
        _cashService = cashService;
        _amount = amount;
    }

    public void Execute()
    {
        _cashService.Withdraw(_amount);
    }
}



public sealed class Button
{
    private readonly ICommand _command;

    public Button(ICommand command)
    {
        _command = command;
    }

    public void Click()
    {
        _command.Execute();
    }
}

var command = new WithdrawCommand(new CashService(), 1000);

command.Execute();

// Using the Button class to execute the command

var cashService = new CashService();

var withdrawCommand = new WithdrawCommand(cashService,1000);

var withdrawButton = new Button(withdrawCommand);

withdrawButton.Click();