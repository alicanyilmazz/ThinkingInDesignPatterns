using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern;

public sealed class VendorPaymentRequest
{
    public long AmountInCents { get; set; }

    public int CurrencyCode { get; set; }

    public string CustomerNumber { get; set; }
}
