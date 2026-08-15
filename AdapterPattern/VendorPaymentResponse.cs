using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern;

public sealed class VendorPaymentResponse
{
    public int ResultCode { get; set; }

    public string TransactionReference { get; set; }
}