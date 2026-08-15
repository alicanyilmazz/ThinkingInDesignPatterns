# Design Patterns

```diff
@@ Strategy Pattern @@

- Strategy Pattern solves the following problem:

@@ When we have multiple algorithms that perform the same type of operation in different ways, we place each algorithm in a separate class and choose which one to use at runtime. @@
```

For example, suppose an ATM calculates withdrawal commission differently depending on the card brand:

```text
Visa       → 2%
MasterCard → 3%
Troy       → 1%
```

---

## Without Strategy Pattern

```csharp
public decimal CalculateCommission(
    string cardType,
    decimal amount)
{
    if (cardType == "Visa")
    {
        return amount * 0.02m;
    }

    if (cardType == "MasterCard")
    {
        return amount * 0.03m;
    }

    if (cardType == "Troy")
    {
        return amount * 0.01m;
    }

    throw new NotSupportedException(
        $"Unsupported card type: {cardType}");
}
```

```diff
@@ This code works, but every time a new commission algorithm is introduced, we must modify the existing method. @@
```

For example:

```csharp
if (cardType == "Amex")
{
    return amount * 0.04m;
}
```

```diff
@@ This is not ideal from an Open/Closed Principle perspective. @@
```

---

## Strategy Pattern Solution

```diff
@@ First, we define a common contract for all commission algorithms. @@
```

```csharp
public interface ICommissionStrategy
{
    decimal Calculate(decimal amount);
}
```

### Visa Strategy

```csharp
public sealed class VisaCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.02m;
    }
}
```

### MasterCard Strategy

```csharp
public sealed class MasterCardCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.03m;
    }
}
```

### Troy Strategy

```csharp
public sealed class TroyCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.01m;
    }
}
```

```diff
@@ Now each algorithm is isolated in its own class. @@
```

---

## Context

```diff
@@ In the Strategy Pattern, the class that uses a strategy is usually called the Context. @@
```

In this example, the context is `CommissionCalculator`.

```csharp
public sealed class CommissionCalculator
{
    private readonly ICommissionStrategy _commissionStrategy;

    public CommissionCalculator(ICommissionStrategy commissionStrategy)
    {
        _commissionStrategy = commissionStrategy;
    }

    public decimal Calculate(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero.");
        }

        return _commissionStrategy.Calculate(amount);
    }
}
```

```diff
@@ The important point is that CommissionCalculator does not know about Visa, MasterCard, or Troy directly. @@

@@ It only depends on the abstraction: @@
```

```csharp
ICommissionStrategy
```

When calculation is required:

```csharp
return _commissionStrategy.Calculate(amount);
```

```diff
@@ The Context does not know how the calculation is performed. @@

@@ The selected Strategy contains that algorithm. @@
```

---

## Usage

### Visa

```csharp
ICommissionStrategy strategy =
    new VisaCommissionStrategy();

var calculator =
    new CommissionCalculator(strategy);

decimal commission = calculator.Calculate(1000);

Console.WriteLine(commission); // 20
```

Flow:

```text
CommissionCalculator.Calculate(1000)
                ↓
VisaCommissionStrategy.Calculate(1000)
                ↓
               20
```

### MasterCard

```csharp
ICommissionStrategy strategy =
    new MasterCardCommissionStrategy();

var calculator =
    new CommissionCalculator(strategy);

decimal commission = calculator.Calculate(1000);

Console.WriteLine(commission); // 30
```

```diff
@@ CommissionCalculator did not change. @@

@@ We only changed the Strategy that was passed to it. @@
```

```csharp
new VisaCommissionStrategy()
```

becomes:

```csharp
new MasterCardCommissionStrategy()
```

> **This is the core idea of the Strategy Pattern: the algorithm can be replaced without changing the class that uses it.**

---

## Main Components

```diff
@@ Strategy Pattern usually contains four important parts. @@
```

### 1. Strategy

The common contract for all algorithms.

```csharp
ICommissionStrategy
```

### 2. Concrete Strategies

The actual algorithms.

```csharp
VisaCommissionStrategy
MasterCardCommissionStrategy
TroyCommissionStrategy
```

### 3. Context

The class that uses the selected algorithm.

```csharp
CommissionCalculator
```

### 4. Client

The code that decides which Strategy should be used.

```csharp
new CommissionCalculator(
    new VisaCommissionStrategy());
```

---

## Does Strategy Eliminate `switch` Completely?

```diff
@@ Not necessarily. @@
```

We may still need to select the appropriate Strategy based on some runtime value.

```csharp
ICommissionStrategy strategy = cardType switch
{
    CardType.Visa =>
        new VisaCommissionStrategy(),

    CardType.MasterCard =>
        new MasterCardCommissionStrategy(),

    CardType.Troy =>
        new TroyCommissionStrategy(),

    _ =>
        throw new NotSupportedException()
};
```

The important difference is that the `switch` no longer contains the business algorithms.

Before:

```csharp
return amount * 0.02m;
return amount * 0.03m;
return amount * 0.01m;
```

After:

```text
switch
   ↓
Select Strategy
   ↓
Strategy executes the algorithm
```

The Strategy selection itself can later be handled by:

```text
Factory
Dependency Injection
Dictionary / Lookup
Resolver
```

---

## Strategy vs Factory

```diff
@@ Strategy @@

> How should this operation be performed?

Examples:

Visa commission algorithm
MasterCard commission algorithm
Troy commission algorithm


@@ Factory @@

> Which implementation should be created or selected?

Example:

Visa → VisaCommissionStrategy
Troy → TroyCommissionStrategy
```

The short version:

```text
Factory  → selects / creates

Strategy → executes the behavior
```

---

## Strategy vs Decorator

### Strategy

Selects one behavior from multiple alternatives.

```text
Visa
 OR
MasterCard
 OR
Troy
```

### Decorator

Adds additional behavior around an existing object.

```text
Logging
   ↓
Retry
   ↓
Authorization
   ↓
PaymentService
```

```diff
@@ Strategy usually chooses between alternative algorithms. @@

@@ Decorator usually combines additional behaviors in layers. @@
```

---

## Summary

```text
Client
   ↓
Select Strategy
   ↓
Context
   ↓
ICommissionStrategy
   ↓
Concrete Strategy
   ↓
Execute Algorithm
```

> **Strategy Pattern defines a family of interchangeable algorithms, places each algorithm in a separate class, and allows the algorithm to be selected at runtime.**

For interviews, remember:

```text
Strategy → How should the behavior be performed?

Factory  → Which implementation should be selected?

Decorator → What additional behavior should wrap the object?
```

# Real-World Example

```diff
@@ Real-World Payment Processing Example @@
```

In a real application, different payment methods usually require different business rules.

For example:

```text
Credit Card
Bank Transfer
Wallet
```

All of them perform the same high-level operation:

```text
Pay
```

However, the actual payment process is different for each payment type.

```text
Credit Card
     ↓
Validate Card Token
     ↓
Call Card / Authorization Service
     ↓
Calculate 2% Commission


Bank Transfer
     ↓
Validate IBAN
     ↓
Call Bank Transfer Service
     ↓
Calculate 0.5% Commission


Wallet
     ↓
Validate Wallet ID
     ↓
Call Wallet Service
     ↓
No Commission
```

```diff
@@ This is a very suitable scenario for the Strategy Pattern. @@
```

---

## Strategy Interface

All payment strategies implement the same contract:

```csharp
public interface IPaymentStrategy
{
    PaymentType Type { get; }

    Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken);
}
```

```diff
@@ IPaymentStrategy defines what every payment algorithm must provide. @@

@@ PaymentService does not need to know how Credit Card, Bank Transfer, or Wallet payments are implemented. @@
```

The `Type` property is also used to identify which strategy belongs to which payment type:

```csharp
PaymentType Type { get; }
```

---

## Payment Types

```csharp
public enum PaymentType
{
    CreditCard,
    BankTransfer,
    Wallet
}
```

The client specifies the payment method through `PaymentRequest`.

```csharp
public sealed record PaymentRequest(
    PaymentType Type,
    decimal Amount,
    string ReferenceNumber,
    string? CardToken,
    string? Iban,
    string? WalletId);
```

---

## Concrete Strategies

### Credit Card Strategy

```csharp
public sealed class CreditCardPaymentStrategy
    : IPaymentStrategy
{
    private readonly ILogger<CreditCardPaymentStrategy> _logger;

    public CreditCardPaymentStrategy(
        ILogger<CreditCardPaymentStrategy> logger)
    {
        _logger = logger;
    }

    public PaymentType Type =>
        PaymentType.CreditCard;

    public async Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CardToken))
        {
            throw new ArgumentException(
                "Credit card payment requires a card token.");
        }

        _logger.LogInformation(
            "Credit card payment started. Reference: {Reference}",
            request.ReferenceNumber);

        // In a real application,
        // the card authorization/payment service would be called here.

        await Task.Delay(100, cancellationToken);

        decimal commission =
            request.Amount * 0.02m;

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString("N"),
            Commission: commission,
            Message: "Credit card payment completed.");
    }
}
```

```text
CreditCard
     ↓
CreditCardPaymentStrategy
     ↓
Validate CardToken
     ↓
Process Payment
     ↓
2% Commission
```

---

### Bank Transfer Strategy

```csharp
public sealed class BankTransferPaymentStrategy
    : IPaymentStrategy
{
    private readonly ILogger<BankTransferPaymentStrategy> _logger;

    public BankTransferPaymentStrategy(
        ILogger<BankTransferPaymentStrategy> logger)
    {
        _logger = logger;
    }

    public PaymentType Type =>
        PaymentType.BankTransfer;

    public async Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Iban))
        {
            throw new ArgumentException(
                "Bank transfer requires an IBAN.");
        }

        _logger.LogInformation(
            "Bank transfer started. Reference: {Reference}",
            request.ReferenceNumber);

        // In a real application,
        // the bank transfer service would be called here.

        await Task.Delay(100, cancellationToken);

        decimal commission =
            request.Amount * 0.005m;

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString("N"),
            Commission: commission,
            Message: "Bank transfer completed.");
    }
}
```

---

### Wallet Strategy

```csharp
public sealed class WalletPaymentStrategy
    : IPaymentStrategy
{
    private readonly ILogger<WalletPaymentStrategy> _logger;

    public WalletPaymentStrategy(
        ILogger<WalletPaymentStrategy> logger)
    {
        _logger = logger;
    }

    public PaymentType Type =>
        PaymentType.Wallet;

    public async Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.WalletId))
        {
            throw new ArgumentException(
                "Wallet payment requires a wallet ID.");
        }

        _logger.LogInformation(
            "Wallet payment started. Reference: {Reference}",
            request.ReferenceNumber);

        // In a real application,
        // the wallet service would be called here.

        await Task.Delay(100, cancellationToken);

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString("N"),
            Commission: 0,
            Message: "Wallet payment completed.");
    }
}
```

---

## Strategy Resolver

```diff
@@ The application still needs a way to select the correct Strategy. @@

@@ PaymentStrategyResolver is responsible for this selection. @@
```

```csharp
public sealed class PaymentStrategyResolver
    : IPaymentStrategyResolver
{
    private readonly IReadOnlyDictionary<
        PaymentType,
        IPaymentStrategy> _strategies;

    public PaymentStrategyResolver(
        IEnumerable<IPaymentStrategy> strategies)
    {
        IPaymentStrategy[] strategyArray =
            strategies.ToArray();

        var duplicateType = strategyArray
            .GroupBy(strategy => strategy.Type)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateType is not null)
        {
            throw new InvalidOperationException(
                $"More than one strategy is registered for " +
                $"{duplicateType.Key}.");
        }

        _strategies = strategyArray.ToDictionary(
            strategy => strategy.Type);
    }

    public IPaymentStrategy Resolve(
        PaymentType paymentType)
    {
        if (_strategies.TryGetValue(
            paymentType,
            out IPaymentStrategy? strategy))
        {
            return strategy;
        }

        throw new NotSupportedException(
            $"No payment strategy is registered for {paymentType}.");
    }
}
```

The resolver converts the registered strategies into a dictionary:

```text
CreditCard
    ↓
CreditCardPaymentStrategy

BankTransfer
    ↓
BankTransferPaymentStrategy

Wallet
    ↓
WalletPaymentStrategy
```

So strategy selection becomes effectively:

```text
PaymentType
     ↓
Dictionary Lookup
     ↓
Correct IPaymentStrategy
```

instead of:

```csharp
if (type == PaymentType.CreditCard)
{
    ...
}
else if (type == PaymentType.BankTransfer)
{
    ...
}
else if (type == PaymentType.Wallet)
{
    ...
}
```

---

## PaymentService — The Context

`PaymentService` acts as the Context.

```csharp
public sealed class PaymentService
{
    private readonly IPaymentStrategyResolver _strategyResolver;

    public PaymentService(
        IPaymentStrategyResolver strategyResolver)
    {
        _strategyResolver = strategyResolver;
    }

    public Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request.Amount),
                "Payment amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(
            request.ReferenceNumber))
        {
            throw new ArgumentException(
                "Reference number is required.");
        }

        IPaymentStrategy strategy =
            _strategyResolver.Resolve(request.Type);

        return strategy.PayAsync(
            request,
            cancellationToken);
    }
}
```

The most important part is:

```csharp
IPaymentStrategy strategy =
    _strategyResolver.Resolve(request.Type);

return strategy.PayAsync(
    request,
    cancellationToken);
```

```diff
@@ PaymentService does not contain Credit Card, Bank Transfer, or Wallet business rules. @@

@@ It only selects the correct Strategy and delegates the operation. @@
```

---

## Dependency Injection

All concrete strategies are registered as `IPaymentStrategy`.

```csharp
builder.Services.AddScoped<
    IPaymentStrategy,
    CreditCardPaymentStrategy>();

builder.Services.AddScoped<
    IPaymentStrategy,
    BankTransferPaymentStrategy>();

builder.Services.AddScoped<
    IPaymentStrategy,
    WalletPaymentStrategy>();
```

Then the resolver is registered:

```csharp
builder.Services.AddScoped<
    IPaymentStrategyResolver,
    PaymentStrategyResolver>();

builder.Services.AddScoped<PaymentService>();
```

ASP.NET Core injects all registered implementations into:

```csharp
IEnumerable<IPaymentStrategy> strategies
```

Therefore the resolver automatically receives:

```text
CreditCardPaymentStrategy
BankTransferPaymentStrategy
WalletPaymentStrategy
```

---

## Full Execution Flow

Suppose the API receives:

```json
{
  "type": 0,
  "amount": 1000,
  "referenceNumber": "PAY-1001",
  "cardToken": "token-123",
  "iban": null,
  "walletId": null
}
```

The flow is:

```text
POST /api/payments
        ↓
PaymentsController
        ↓
PaymentService.PayAsync()
        ↓
request.Type = CreditCard
        ↓
PaymentStrategyResolver.Resolve()
        ↓
CreditCardPaymentStrategy
        ↓
PayAsync()
        ↓
Validate CardToken
        ↓
Process Credit Card Payment
        ↓
Calculate 2% Commission
        ↓
PaymentResult
```

For an amount of:

```text
1000
```

the commission becomes:

```text
1000 × 0.02 = 20
```

---

## Adding a New Payment Method

Suppose we need to support:

```text
Crypto
```

First add the new type:

```csharp
public enum PaymentType
{
    CreditCard,
    BankTransfer,
    Wallet,
    Crypto
}
```

Then implement a new Strategy:

```csharp
public sealed class CryptoPaymentStrategy
    : IPaymentStrategy
{
    public PaymentType Type =>
        PaymentType.Crypto;

    public Task<PaymentResult> PayAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        // Crypto-specific payment logic

        throw new NotImplementedException();
    }
}
```

Register it:

```csharp
builder.Services.AddScoped<
    IPaymentStrategy,
    CryptoPaymentStrategy>();
```

```diff
@@ PaymentService does not change. @@

@@ PaymentStrategyResolver does not change. @@

@@ Existing Strategies do not change. @@
```

This is the important Open/Closed Principle benefit of the design.

```text
New Requirement
      ↓
New Strategy
      ↓
Register Strategy
      ↓
Done
```

---

## Alternative: Keyed Services

The project also contains a `KeyedPaymentStrategyResolver`.

It uses .NET keyed dependency injection:

```csharp
public sealed class KeyedPaymentStrategyResolver
    : IPaymentStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public KeyedPaymentStrategyResolver(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentStrategy Resolve(
        PaymentType paymentType)
    {
        return _serviceProvider
            .GetRequiredKeyedService<IPaymentStrategy>(
                paymentType);
    }
}
```

Strategies can then be registered using keys:

```csharp
builder.Services.AddKeyedScoped<
    IPaymentStrategy,
    CreditCardPaymentStrategy>(
    PaymentType.CreditCard);

builder.Services.AddKeyedScoped<
    IPaymentStrategy,
    BankTransferPaymentStrategy>(
    PaymentType.BankTransfer);

builder.Services.AddKeyedScoped<
    IPaymentStrategy,
    WalletPaymentStrategy>(
    PaymentType.Wallet);
```

```diff
@@ Important @@

The current implementation uses PaymentStrategyResolver.

The keyed registrations in Program.cs are currently commented out,
so KeyedPaymentStrategyResolver represents an alternative approach,
not the active implementation.
```

---

## Why Strategy Pattern Fits This Example

```diff
+ Different payment methods perform the same conceptual operation: Pay.

+ Each payment method has different validation and business rules.

+ Each algorithm is isolated in its own class.

+ PaymentService does not depend on concrete payment implementations.

+ The correct algorithm is selected at runtime.

+ Adding another payment type does not require modifying PaymentService.

+ Each Strategy can be tested independently.
```

The final architecture looks like this:

```text
                    IPaymentStrategy
                          ↑
          ┌───────────────┼───────────────┐
          │               │               │
 CreditCardStrategy   BankTransfer    WalletStrategy
          │             Strategy           │
          └───────────────┼───────────────┘
                          ↑
                 PaymentStrategyResolver
                          ↑
                    PaymentService
                          ↑
                 PaymentsController
                          ↑
                       Client
```

```diff
@@ Key Takeaway @@
```

> **Strategy Pattern allows the payment algorithm to vary independently from the code that uses it. `PaymentService` defines when a payment should be executed, while each `IPaymentStrategy` implementation defines how that specific payment is executed.**

