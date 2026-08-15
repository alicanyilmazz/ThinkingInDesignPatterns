

using AdapterPattern;

var paymentAdapter = new VendorPaymentAdapter(new VendorPaymentClient());

paymentAdapter.Pay(new PaymentRequest
{
    Amount = 100.00m,
    Currency = "USD",
    CustomerId = "CUST-001"
});