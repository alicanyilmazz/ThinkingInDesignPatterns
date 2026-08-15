using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern;

public sealed class PaymentRequest
{
    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string CustomerId { get; set; }
}