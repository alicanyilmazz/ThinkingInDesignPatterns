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
