using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern;

public sealed class VendorPaymentAdapter : IPaymentGateway
{
    private readonly VendorPaymentClient _client;

    public VendorPaymentAdapter(VendorPaymentClient client)
    {
        _client = client;
    }

    public PaymentResult Pay(PaymentRequest request)
    {
        var vendorRequest = new VendorPaymentRequest
            {
                AmountInCents = (long)(request.Amount * 100),

                CurrencyCode = MapCurrency(request.Currency),

                CustomerNumber = request.CustomerId
            };

        VendorPaymentResponse vendorResponse = _client.Execute(vendorRequest);

        return new PaymentResult
        {
            IsSuccess = vendorResponse.ResultCode == 0,

            TransactionId = vendorResponse.TransactionReference,

            Message = vendorResponse.ResultCode == 0 ? "Payment completed." : "Payment failed."
        };
    }

    private static int MapCurrency(string currency)
    {
        return currency switch
        {
            "TRY" => 949,
            "USD" => 840,
            "EUR" => 978,

            _ => throw new NotSupportedException(
                $"Unsupported currency: {currency}")
        };
    }
}