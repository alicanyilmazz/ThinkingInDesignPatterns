using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Commands;

public class TransferMoneyCommand : ICommand
{
    private readonly BankService _bankService;

    private readonly BankAccount _sender;

    private readonly BankAccount _receiver;

    private readonly decimal _amount;

    public TransferMoneyCommand(BankService bankService,BankAccount sender,BankAccount receiver,decimal amount)
    {
        _bankService = bankService;
        _sender = sender;
        _receiver = receiver;
        _amount = amount;
    }

    public void Execute()
    {
        _bankService.TransferMoney(_sender,_receiver,_amount);
    }
}