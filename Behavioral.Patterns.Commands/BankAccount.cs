using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Commands;

public class BankAccount
{
    public string Iban { get; }
    public string CustomerName { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string iban,string customerName,decimal balance)
    {
        Iban = iban;
        CustomerName = customerName;
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Transfer tutarı sıfırdan büyük olmalıdır.");
        }

        if (Balance < amount)
        {
            throw new Exception("Yetersiz bakiye.");
        }

        Balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Yatırılacak tutar sıfırdan büyük olmalıdır.");
        }

        Balance += amount;
    }
}