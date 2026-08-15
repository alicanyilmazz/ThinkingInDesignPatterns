# Design Patterns

```diff
@@ Adapter Pattern @@

The Adapter Pattern is used when two components need to work together,
but their interfaces are incompatible.

The Adapter translates one interface into another interface
that the client already understands.
```

---

# Understanding the Problem

Suppose our application depends on this interface:

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

The rest of the application knows only this abstraction:

```csharp
_paymentService.Pay(100);
```

Now imagine that we integrate a third-party banking SDK.

The vendor gives us this class:

```csharp
public class LegacyBankApi
{
    public void ExecutePayment(decimal amount)
    {
        Console.WriteLine(
            $"Legacy payment executed: {amount}");
    }
}
```

The problem is obvious:

```text
Our Application
      ↓
Pay()

Vendor SDK
      ↓
ExecutePayment()
```

The operations are conceptually similar, but the interfaces are different.

Our application expects:

```csharp
Pay()
```

The vendor exposes:

```csharp
ExecutePayment()
```

```diff
@@ The two systems cannot communicate directly. @@
```

---

# Why Not Change the Existing Code?

There are two common reasons.

## 1. We cannot change the vendor library

The third-party code may be:

```text
NuGet Package

External DLL

SOAP Client

Vendor SDK

Generated Proxy

Legacy Assembly

Hardware SDK
```

We do not own it.

Changing it may be impossible.

---

## 2. We do not want to change our application

Imagine that `IPaymentService` is already used in hundreds of places:

```csharp
_paymentService.Pay(amount);
```

Changing the application to understand:

```csharp
ExecutePayment()
```

would leak vendor-specific details into the entire system.

Then tomorrow we integrate another provider:

```text
Bank A → ExecutePayment()

Bank B → SendTransaction()

Bank C → Process()

Bank D → MakeTransfer()
```

Without Adapter, the application starts knowing every vendor's API.

That creates tight coupling.

```text
Application
   ↓
BankA.ExecutePayment()

Application
   ↓
BankB.SendTransaction()

Application
   ↓
BankC.Process()
```

This is exactly what we want to avoid.

---

# Adapter Solution

The Adapter sits between our application and the incompatible system.

```text
Our Application
      ↓
IPaymentService.Pay()
      ↓
LegacyBankAdapter
      ↓
LegacyBankApi.ExecutePayment()
```

The client still works with:

```csharp
IPaymentService
```

and never needs to know that the vendor uses:

```csharp
ExecutePayment()
```

---

# Implementation

## Target Interface

This is the interface expected by our application.

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

In Adapter terminology, this is called the:

```text
Target
```

---

## Adaptee

This is the incompatible external component.

```csharp
public class LegacyBankApi
{
    public void ExecutePayment(decimal amount)
    {
        Console.WriteLine(
            $"Legacy payment executed: {amount}");
    }
}
```

In Adapter terminology, this is called the:

```text
Adaptee
```

---

## Adapter

```csharp
public class LegacyBankAdapter : IPaymentService
{
    private readonly LegacyBankApi _legacyBankApi;

    public LegacyBankAdapter(
        LegacyBankApi legacyBankApi)
    {
        _legacyBankApi = legacyBankApi;
    }

    public void Pay(decimal amount)
    {
        _legacyBankApi.ExecutePayment(amount);
    }
}
```

The Adapter implements the interface expected by the application:

```csharp
IPaymentService
```

but internally translates the call:

```text
Pay()
 ↓
ExecutePayment()
```

---

# Usage

```csharp
IPaymentService paymentService =
    new LegacyBankAdapter(
        new LegacyBankApi());

paymentService.Pay(500);
```

The client only sees:

```csharp
IPaymentService
```

It does not know about:

```text
LegacyBankApi
ExecutePayment()
Vendor SDK
```

Execution flow:

```text
Client
  ↓
IPaymentService.Pay(500)
  ↓
LegacyBankAdapter.Pay(500)
  ↓
LegacyBankApi.ExecutePayment(500)
```

---

# What Does the Adapter Actually Translate?

An Adapter does not only rename methods.

A real Adapter may translate:

```text
Method Names

Request Models

Response Models

Data Types

Enums

Error Codes

Exceptions

Protocols

Date Formats

External Status Codes
```

This is where Adapter becomes much more useful than a simple method wrapper.

---

# More Realistic Banking Example

Suppose our application uses this model:

```csharp
public sealed class PaymentRequest
{
    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string CustomerId { get; set; }
}
```

Our application expects:

```csharp
public interface IPaymentGateway
{
    PaymentResult Pay(PaymentRequest request);
}
```

Now imagine a vendor SDK exposes this completely different contract:

```csharp
public sealed class VendorPaymentRequest
{
    public long AmountInCents { get; set; }

    public int CurrencyCode { get; set; }

    public string CustomerNumber { get; set; }
}
```

Vendor response:

```csharp
public sealed class VendorPaymentResponse
{
    public int ResultCode { get; set; }

    public string TransactionReference { get; set; }
}
```

Vendor client:

```csharp
public class VendorPaymentClient
{
    public VendorPaymentResponse Execute(
        VendorPaymentRequest request)
    {
        Console.WriteLine(
            "Vendor payment service called.");

        return new VendorPaymentResponse
        {
            ResultCode = 0,
            TransactionReference =
                Guid.NewGuid().ToString("N")
        };
    }
}
```

Now the problem is much bigger than:

```text
Pay() vs Execute()
```

We also have:

```text
decimal Amount
        ↓
long AmountInCents

string Currency
        ↓
int CurrencyCode

CustomerId
        ↓
CustomerNumber

Vendor ResultCode
        ↓
Application IsSuccess
```

This is a perfect Adapter scenario.

---

# Application Response

```csharp
public sealed class PaymentResult
{
    public bool IsSuccess { get; set; }

    public string TransactionId { get; set; }

    public string Message { get; set; }
}
```

---

# Real Adapter

```csharp
public sealed class VendorPaymentAdapter
    : IPaymentGateway
{
    private readonly VendorPaymentClient _client;

    public VendorPaymentAdapter(
        VendorPaymentClient client)
    {
        _client = client;
    }

    public PaymentResult Pay(
        PaymentRequest request)
    {
        var vendorRequest =
            new VendorPaymentRequest
            {
                AmountInCents =
                    (long)(request.Amount * 100),

                CurrencyCode =
                    MapCurrency(
                        request.Currency),

                CustomerNumber =
                    request.CustomerId
            };

        VendorPaymentResponse vendorResponse =
            _client.Execute(vendorRequest);

        return new PaymentResult
        {
            IsSuccess =
                vendorResponse.ResultCode == 0,

            TransactionId =
                vendorResponse.TransactionReference,

            Message =
                vendorResponse.ResultCode == 0
                    ? "Payment completed."
                    : "Payment failed."
        };
    }

    private static int MapCurrency(
        string currency)
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
```

This Adapter performs several translations:

```text
Application Request
       ↓
Adapter
       ↓
Vendor Request


decimal
  ↓
long cents


TRY
 ↓
949


Vendor ResultCode
       ↓
bool IsSuccess


Vendor TransactionReference
       ↓
TransactionId
```

```diff
@@ This is a much more realistic use of Adapter. @@
```

---

# Why Is This Better?

Without Adapter:

```text
Application
   ↓
Vendor DTOs

Application
   ↓
Vendor Enums

Application
   ↓
Vendor Error Codes

Application
   ↓
Vendor Method Names
```

Vendor details spread through the application.

With Adapter:

```text
Application
       ↓
Own Interface
       ↓
Adapter
       ↓
Vendor SDK
```

Only the Adapter understands the vendor.

This creates an **Anti-Corruption Layer-like boundary** around the external dependency.

The application stays clean.

---

# ATM Example

Suppose our ATM application expects:

```csharp
public interface ICardReader
{
    CardData ReadCard();
}
```

But the NCR SDK provides:

```csharp
public class NcrCardReaderSdk
{
    public TrackData ReadTrackData()
    {
        return new TrackData();
    }
}
```

These contracts are incompatible.

Our application expects:

```text
ReadCard()
      ↓
CardData
```

NCR gives us:

```text
ReadTrackData()
      ↓
TrackData
```

Adapter:

```csharp
public sealed class NcrCardReaderAdapter
    : ICardReader
{
    private readonly NcrCardReaderSdk _sdk;

    public NcrCardReaderAdapter(
        NcrCardReaderSdk sdk)
    {
        _sdk = sdk;
    }

    public CardData ReadCard()
    {
        TrackData trackData =
            _sdk.ReadTrackData();

        return new CardData
        {
            CardNumber =
                ParsePan(trackData),

            ExpiryDate =
                ParseExpiry(trackData)
        };
    }

    private string ParsePan(
        TrackData trackData)
    {
        // Parse vendor-specific track data

        return "529545******1234";
    }

    private string ParseExpiry(
        TrackData trackData)
    {
        return "12/30";
    }
}
```

Now the ATM application never needs to know about:

```text
TrackData

NCR SDK

ReadTrackData()

Vendor-specific parsing
```

It only knows:

```csharp
ICardReader.ReadCard();
```

This is a strong real-world Adapter example.

---

# Another Banking Example — SOAP to Internal Interface

Suppose an old service exposes a generated SOAP client:

```csharp
LegacyCustomerServiceClient
```

with:

```csharp
GetCustomerInfoAsync(
    GetCustomerInfoRequest request);
```

But our application wants:

```csharp
public interface ICustomerService
{
    Task<Customer> GetAsync(
        string customerId);
}
```

The Adapter can hide:

```text
SOAP Request

SOAP Response

Generated Proxy

Serialization Details

FaultException
```

from the application.

```text
Application
     ↓
ICustomerService
     ↓
SoapCustomerServiceAdapter
     ↓
Generated SOAP Client
```

This is much better than allowing SOAP-generated classes to spread through the business layer.

---

# Object Adapter vs Class Adapter

There are two theoretical Adapter approaches.

## Object Adapter

Uses composition.

```csharp
public class LegacyBankAdapter
    : IPaymentService
{
    private readonly LegacyBankApi _legacyApi;
}
```

Flow:

```text
Adapter
  ↓
HAS-A
  ↓
LegacyBankApi
```

This is the most common approach in C#.

---

## Class Adapter

Class Adapter uses inheritance to adapt the existing class.

Conceptually:

```text
Adapter
  ↓
inherits
  ↓
Legacy API
```

But C# does not support multiple class inheritance, so the classical GoF Class Adapter implementation is less flexible.

For most modern .NET code:

```text
Prefer Composition
```

Therefore:

```csharp
private readonly LegacyBankApi _legacyApi;
```

is usually the better choice.

---

# Adapter and Dependency Injection

Adapter works very naturally with Dependency Injection.

```csharp
services.AddScoped<LegacyBankApi>();

services.AddScoped<
    IPaymentService,
    LegacyBankAdapter>();
```

Then your business service depends only on:

```csharp
public class OrderService
{
    private readonly IPaymentService _paymentService;

    public OrderService(
        IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
}
```

`OrderService` knows nothing about:

```text
LegacyBankApi
Vendor SDK
ExecutePayment()
```

This also supports Dependency Inversion.

---

# Adapter vs Decorator

This is an important interview question.

## Decorator

Keeps the same abstraction and adds behavior.

```text
IPaymentService
      ↓
LoggingDecorator
      ↓
RetryDecorator
      ↓
PaymentService
```

For example:

```text
Pay()
 ↓
Logging
 ↓
Retry
 ↓
Pay()
```

The interface stays compatible.

---

## Adapter

Changes one interface into another compatible form.

```text
Application expects:

Pay()

        ↓
     Adapter
        ↓

Vendor provides:

ExecutePayment()
```

Short version:

```text
Decorator
→ Adds behavior.


Adapter
→ Converts an interface.
```

---

# Adapter vs Facade

Another common interview question.

## Adapter

Solves **incompatibility**.

```text
Expected Interface
       ↓
Adapter
       ↓
Different Interface
```

Example:

```text
ICardReader.ReadCard()
       ↓
NCR Adapter
       ↓
NCR.ReadTrackData()
```

---

## Facade

Solves **complexity**.

Suppose a payment operation requires:

```text
CustomerService

FraudService

CardService

LedgerService

NotificationService
```

A Facade might expose:

```csharp
ProcessPayment();
```

So:

```text
Facade
→ Simplifies a complex subsystem.


Adapter
→ Makes incompatible interfaces compatible.
```

---

# Adapter vs Strategy

## Strategy

Changes the algorithm or behavior.

```text
Payment
   ↓
VisaStrategy

OR

MasterCardStrategy

OR

TroyStrategy
```

The client chooses among interchangeable algorithms.

---

## Adapter

Does not primarily choose an algorithm.

It makes an incompatible API usable through the interface expected by our application.

```text
Our Interface
      ↓
Adapter
      ↓
External Interface
```

Short version:

```text
Strategy
→ Which behavior should execute?


Adapter
→ How can this incompatible component fit our interface?
```

---

# Adapter vs Proxy

These are sometimes confused because both wrap another object.

## Adapter

Changes the interface.

```text
Pay()
 ↓
Adapter
 ↓
ExecutePayment()
```

## Proxy

Usually preserves the same interface but controls access.

```text
IPaymentService
      ↓
PaymentProxy
      ↓
PaymentService
```

Proxy may provide:

```text
Lazy Loading

Remote Access

Access Control

Caching

Security
```

Short version:

```text
Adapter
→ Change interface.


Proxy
→ Control access.
```

---

# When Should We Use Adapter?

Use Adapter when:

```diff
+ You need to integrate a third-party SDK.

+ You need to integrate legacy code.

+ Your application interface differs from an external service interface.

+ Vendor request/response models should not leak into your business layer.

+ You are replacing one external provider with another.

+ Generated SOAP / REST client models should remain isolated.

+ Hardware vendor APIs use incompatible contracts.

+ You want to protect your domain/application layer from external dependencies.
```

---

# When Should We NOT Use Adapter?

Do not introduce Adapter unnecessarily when:

```text
Both components already use compatible interfaces.

There is no external or incompatible contract.

The wrapper performs no translation and adds no architectural value.
```

For example:

```csharp
public void Pay(decimal amount)
{
    _paymentService.Pay(amount);
}
```

If both sides already expose exactly the same abstraction and the wrapper adds nothing, calling it an Adapter may be misleading.

---

# Advantages

```diff
@@ Advantages @@

+ Keeps legacy or vendor code unchanged.

+ Keeps the application independent from vendor-specific APIs.

+ Solves interface incompatibility.

+ Isolates third-party DTOs and enums.

+ Makes external providers easier to replace.

+ Supports Dependency Inversion.

+ Makes testing easier because the application depends on its own interface.

+ Prevents external APIs from contaminating the domain/application layer.
```

---

# Disadvantages

```diff
@@ Disadvantages @@

- Introduces additional classes.

- Mapping logic can become complex.

- A large number of external systems may require many Adapters.

- Poorly designed Adapters can become large translation classes.
```

---

# Interview Summary

```text
Client
  ↓
Target Interface
  ↓
Adapter
  ↓
Adaptee
```

Using our example:

```text
Application
    ↓
IPaymentService
    ↓
LegacyBankAdapter
    ↓
LegacyBankApi
```

The four important terms are:

```text
Client
→ The code that wants to use the functionality.

Target
→ The interface expected by the Client.

Adaptee
→ The incompatible existing component.

Adapter
→ The component that translates Target calls to Adaptee calls.
```

> **Adapter Pattern converts the interface of an existing class into another interface expected by the client, allowing incompatible components to work together without modifying either side.**

---

# Key Interview Question

```diff
@@ Why would we use Adapter instead of calling the vendor SDK directly? @@
```

Because calling the vendor SDK directly creates coupling:

```text
Business Layer
     ↓
Vendor SDK
     ↓
Vendor DTOs
     ↓
Vendor Enums
     ↓
Vendor Error Codes
```

With Adapter:

```text
Business Layer
     ↓
Our Interface
     ↓
Adapter
     ↓
Vendor SDK
```

If the vendor changes:

```text
OldBank
   ↓
NewBank
```

the business layer does not need to change.

We can create:

```text
OldBankAdapter

NewBankAdapter
```

both implementing:

```csharp
IPaymentService
```

This is one of the biggest practical benefits of Adapter.

---

# Interview Questions

### Question 1

What is the primary purpose of the Adapter Pattern?

**A)** Create new objects

**B)** Make incompatible interfaces work together

**C)** Change an algorithm at runtime

**D)** Add logging

**✅ Answer: B**

---

### Question 2

Which of the following is a good Adapter Pattern scenario?

**A)** Integrating a SOAP service into an application-specific interface

**B)** Adapting a hardware SDK to an internal device interface

**C)** Wrapping a vendor payment SDK behind `IPaymentService`

**D)** All of the above

**✅ Answer: D**

---

### Question 3

Which SOLID principle can Adapter strongly support when an application depends on its own abstraction instead of a vendor API?

**A)** Open/Closed Principle

**B)** Dependency Inversion Principle

**C)** Liskov Substitution Principle

**D)** Interface Segregation Principle

**✅ Answer: B**

---

### Question 4

What is the main difference between Decorator and Adapter?

**A)** They are identical.

**B)** Decorator adds behavior while Adapter converts an interface.

**C)** Adapter always improves performance.

**D)** Decorator creates objects.

**✅ Answer: B**

---

### Question 5

An ATM application expects:

```csharp
ReadCard()
```

but an NCR SDK exposes:

```csharp
ReadTrackData()
```

and returns a vendor-specific `TrackData` object.

Which pattern is the best fit?

**A)** Builder

**B)** Strategy

**C)** Adapter

**D)** Factory

**✅ Answer: C**

---

### Question 6

Which statement best describes the Adaptee?

**A)** The interface expected by the application.

**B)** The incompatible existing component being adapted.

**C)** The class that chooses a Strategy.

**D)** The class that creates objects.

**✅ Answer: B**

---

# Final Cheat Sheet

```text
Adapter
→ Make incompatible interfaces compatible.

Decorator
→ Add behavior around an object.

Facade
→ Simplify a complex subsystem.

Strategy
→ Select one interchangeable behavior.

Proxy
→ Control access to another object.
```

And remember the most important Adapter flow:

```text
Application
     ↓
OUR Interface
     ↓
Adapter
     ↓
EXTERNAL Interface
```

The application should depend on **its own abstraction**, not on the vendor's API.
