using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Commands;


public class BankService
{
    public void TransferMoney(BankAccount sender,BankAccount receiver,decimal amount)
    {
        Console.WriteLine("Transfer işlemi başladı.");

        sender.Withdraw(amount);

        receiver.Deposit(amount);

        Console.WriteLine($"{amount:N2} TRY transfer edildi.");

        Console.WriteLine($"Gönderen: {sender.CustomerName}");

        Console.WriteLine($"Alıcı: {receiver.CustomerName}");
    }
}