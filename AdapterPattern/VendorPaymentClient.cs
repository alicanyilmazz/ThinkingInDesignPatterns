using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern;

public class VendorPaymentClient
{
    public VendorPaymentResponse Execute(VendorPaymentRequest request)
    {
        Console.WriteLine("Vendor payment service called.");

        return new VendorPaymentResponse
        {
            ResultCode = 0,
            TransactionReference = Guid.NewGuid().ToString("N")
        };
    }
}