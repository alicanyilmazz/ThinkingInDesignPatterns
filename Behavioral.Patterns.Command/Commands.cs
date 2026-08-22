using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Command;

public interface ICommand
{
    void Execute();
}

public sealed class WithdrawCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Withdraw command executed.");
    }
}

public sealed class DepositCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Deposit command executed.");
    }
}

public sealed class BalanceInquiryCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Balance inquiry executed.");
    }
}