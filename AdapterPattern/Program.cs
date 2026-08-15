

using AdapterPattern;

var paymentAdapter = new VendorPaymentAdapter(new VendorPaymentClient());

paymentAdapter.Pay(new PaymentRequest
{
    Amount = 100.00m,
    Currency = "USD",
    CardNumber = "4111111111111111",
    ExpiryMonth = 12,
    ExpiryYear = 2025,
    Cvv = "123"
});