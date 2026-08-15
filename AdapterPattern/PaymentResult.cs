using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern;

public sealed class PaymentResult
{
    public bool IsSuccess { get; set; }

    public string TransactionId { get; set; }

    public string Message { get; set; }
}
