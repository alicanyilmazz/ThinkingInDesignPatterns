using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.ChainofResponsibility;

public class CardValidationHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("Kart doğrulandı.");

        base.Handle(request);
    }
}

public class PinValidationHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("PIN doğrulandı.");

        base.Handle(request);
    }
}

public class BalanceHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        if (request.Balance < request.Amount)
        {
            Console.WriteLine("Yetersiz bakiye.");

            return;
        }

        Console.WriteLine("Bakiye uygun.");

        base.Handle(request);
    }
}

public class CashHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("ATM parayı verdi.");

        base.Handle(request);
    }
}