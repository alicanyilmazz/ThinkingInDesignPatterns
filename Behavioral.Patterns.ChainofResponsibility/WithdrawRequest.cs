using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.ChainofResponsibility;

public class WithdrawRequest
{
    public string CardNumber { get; set; }

    public string Pin { get; set; }

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }
}