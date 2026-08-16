# Design Patterns

```diff
@@ Factory Pattern @@

Factory Pattern centralizes object creation logic.

Instead of letting the client decide which concrete class should be created,
we move that decision into a Factory.
```

---

# Understanding the Problem

Suppose we are building a payment system.

We have different payment processors:

```text
Visa
MasterCard
Troy
```

Without Factory, the client may create concrete classes directly:

```csharp
IPaymentProcessor processor;

if (cardType == CardType.Visa)
{
    processor = new VisaPaymentProcessor();
}
else if (cardType == CardType.MasterCard)
{
    processor = new MasterCardPaymentProcessor();
}
else if (cardType == CardType.Troy)
{
    processor = new TroyPaymentProcessor();
}
else
{
    throw new NotSupportedException();
}

processor.Pay(1000);
```

This works.

But now the client knows every concrete implementation:

```text
VisaPaymentProcessor
MasterCardPaymentProcessor
TroyPaymentProcessor
```

And every time a new payment type is added:

```text
Amex
UnionPay
Discover
```

we modify the object creation logic again.

```diff
@@ The client is responsible for too many things. @@

- It knows which concrete classes exist.
- It knows how to create them.
- It knows which class belongs to which card type.
- It also uses the created object.
```

Factory Pattern separates these responsibilities.

---

# Core Idea

Instead of this:

```csharp
if (cardType == CardType.Visa)
{
    return new VisaPaymentProcessor();
}
```

being scattered through the application, we move object creation into one place.

```text
Client
  ↓
Factory
  ↓
Choose Concrete Class
  ↓
Create Object
  ↓
Return Abstraction
```

The client simply asks:

```text
"Give me the correct payment processor."
```

The Factory decides which object should be created.

---

# Payment Processor Interface

```csharp
public interface IPaymentProcessor
{
    void Pay(decimal amount);
}
```

---

# Concrete Implementations

## Visa

```csharp
public sealed class VisaPaymentProcessor
    : IPaymentProcessor
{
    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"Visa payment processed: {amount}");
    }
}
```

---

## MasterCard

```csharp
public sealed class MasterCardPaymentProcessor
    : IPaymentProcessor
{
    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"MasterCard payment processed: {amount}");
    }
}
```

---

## Troy

```csharp
public sealed class TroyPaymentProcessor
    : IPaymentProcessor
{
    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"Troy payment processed: {amount}");
    }
}
```

---

# Factory

```csharp
public enum CardType
{
    Visa,
    MasterCard,
    Troy
}
```

```csharp
public static class PaymentProcessorFactory
{
    public static IPaymentProcessor Create(
        CardType cardType)
    {
        return cardType switch
        {
            CardType.Visa =>
                new VisaPaymentProcessor(),

            CardType.MasterCard =>
                new MasterCardPaymentProcessor(),

            CardType.Troy =>
                new TroyPaymentProcessor(),

            _ =>
                throw new NotSupportedException(
                    $"Unsupported card type: {cardType}")
        };
    }
}
```

---

# Usage

```csharp
IPaymentProcessor processor =
    PaymentProcessorFactory.Create(
        CardType.Visa);

processor.Pay(1000);
```

The client no longer writes:

```csharp
new VisaPaymentProcessor();
```

Instead:

```csharp
PaymentProcessorFactory.Create(
    CardType.Visa);
```

The Factory owns the object creation decision.

---

# Execution Flow

```text
Client
  ↓
PaymentProcessorFactory.Create(Visa)
  ↓
Factory checks CardType
  ↓
new VisaPaymentProcessor()
  ↓
Return IPaymentProcessor
  ↓
processor.Pay(1000)
```

---

# Why Do We Use Factory?

The main benefit is not simply avoiding the `new` keyword.

```diff
@@ Factory Pattern is not about hiding "new". @@

@@ It is about hiding object creation decisions from the client. @@
```

Without Factory:

```text
Client
 ├── knows VisaPaymentProcessor
 ├── knows MasterCardPaymentProcessor
 ├── knows TroyPaymentProcessor
 └── decides which one should be instantiated
```

With Factory:

```text
Client
   ↓
IPaymentProcessor
   ↓
Factory
   ↓
Concrete Implementation
```

The client depends primarily on:

```csharp
IPaymentProcessor
```

rather than knowing every implementation.

---

# A More Realistic Banking Example

Suppose an ATM needs to communicate with different card networks.

```text
Visa
MasterCard
Troy
```

Each network may require a different client.

```csharp
public interface ICardNetworkClient
{
    Task AuthorizeAsync(
        decimal amount,
        CancellationToken cancellationToken);
}
```

Concrete clients:

```csharp
public sealed class VisaNetworkClient
    : ICardNetworkClient
{
    public Task AuthorizeAsync(
        decimal amount,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Visa authorization: {amount}");

        return Task.CompletedTask;
    }
}
```

```csharp
public sealed class MasterCardNetworkClient
    : ICardNetworkClient
{
    public Task AuthorizeAsync(
        decimal amount,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"MasterCard authorization: {amount}");

        return Task.CompletedTask;
    }
}
```

```csharp
public sealed class TroyNetworkClient
    : ICardNetworkClient
{
    public Task AuthorizeAsync(
        decimal amount,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Troy authorization: {amount}");

        return Task.CompletedTask;
    }
}
```

Factory:

```csharp
public sealed class CardNetworkFactory
{
    public ICardNetworkClient Create(
        CardType cardType)
    {
        return cardType switch
        {
            CardType.Visa =>
                new VisaNetworkClient(),

            CardType.MasterCard =>
                new MasterCardNetworkClient(),

            CardType.Troy =>
                new TroyNetworkClient(),

            _ =>
                throw new NotSupportedException()
        };
    }
}
```

Usage:

```csharp
var factory =
    new CardNetworkFactory();

ICardNetworkClient client =
    factory.Create(CardType.MasterCard);

await client.AuthorizeAsync(
    2500,
    CancellationToken.None);
```

---

# Factory With Dependency Injection

In real applications, concrete classes often have dependencies.

For example:

```csharp
public sealed class VisaPaymentProcessor
    : IPaymentProcessor
{
    private readonly ILogger<VisaPaymentProcessor> _logger;

    private readonly IVisaClient _visaClient;

    public VisaPaymentProcessor(
        ILogger<VisaPaymentProcessor> logger,
        IVisaClient visaClient)
    {
        _logger = logger;
        _visaClient = visaClient;
    }

    public void Pay(decimal amount)
    {
        _logger.LogInformation(
            "Visa payment started.");

        _visaClient.Pay(amount);
    }
}
```

Now manually writing:

```csharp
new VisaPaymentProcessor(
    logger,
    visaClient);
```

inside a Factory becomes less attractive.

We can let Dependency Injection create the objects.

---

# DI-Based Factory

```csharp
public sealed class PaymentProcessorFactory
{
    private readonly IEnumerable<IPaymentProcessor> _processors;

    public PaymentProcessorFactory(
        IEnumerable<IPaymentProcessor> processors)
    {
        _processors = processors;
    }

    public IPaymentProcessor Create(
        CardType cardType)
    {
        return cardType switch
        {
            CardType.Visa =>
                _processors
                    .OfType<VisaPaymentProcessor>()
                    .Single(),

            CardType.MasterCard =>
                _processors
                    .OfType<MasterCardPaymentProcessor>()
                    .Single(),

            CardType.Troy =>
                _processors
                    .OfType<TroyPaymentProcessor>()
                    .Single(),

            _ =>
                throw new NotSupportedException()
        };
    }
}
```

Registrations:

```csharp
services.AddScoped<
    IPaymentProcessor,
    VisaPaymentProcessor>();

services.AddScoped<
    IPaymentProcessor,
    MasterCardPaymentProcessor>();

services.AddScoped<
    IPaymentProcessor,
    TroyPaymentProcessor>();

services.AddScoped<PaymentProcessorFactory>();
```

Now DI constructs each dependency graph.

---

# A Cleaner Factory With a Key

We can avoid checking concrete types by letting every implementation identify itself.

```csharp
public interface IPaymentProcessor
{
    CardType Type { get; }

    void Pay(decimal amount);
}
```

Visa:

```csharp
public sealed class VisaPaymentProcessor
    : IPaymentProcessor
{
    public CardType Type =>
        CardType.Visa;

    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"Visa payment: {amount}");
    }
}
```

MasterCard:

```csharp
public sealed class MasterCardPaymentProcessor
    : IPaymentProcessor
{
    public CardType Type =>
        CardType.MasterCard;

    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"MasterCard payment: {amount}");
    }
}
```

Factory:

```csharp
public sealed class PaymentProcessorFactory
{
    private readonly IReadOnlyDictionary<
        CardType,
        IPaymentProcessor> _processors;

    public PaymentProcessorFactory(
        IEnumerable<IPaymentProcessor> processors)
    {
        _processors = processors.ToDictionary(
            x => x.Type);
    }

    public IPaymentProcessor Create(
        CardType cardType)
    {
        if (_processors.TryGetValue(
            cardType,
            out var processor))
        {
            return processor;
        }

        throw new NotSupportedException(
            $"No payment processor registered for {cardType}.");
    }
}
```

Now:

```text
Visa
   ↓
VisaPaymentProcessor

MasterCard
   ↓
MasterCardPaymentProcessor

Troy
   ↓
TroyPaymentProcessor
```

Usage:

```csharp
IPaymentProcessor processor =
    factory.Create(CardType.Visa);

processor.Pay(1000);
```

---

# Factory vs Direct Object Creation

Without Factory:

```csharp
var processor =
    new VisaPaymentProcessor();
```

The client explicitly depends on:

```text
VisaPaymentProcessor
```

With Factory:

```csharp
IPaymentProcessor processor =
    factory.Create(CardType.Visa);
```

The object creation decision is centralized.

```text
Client
  ↓
Factory
  ↓
Concrete Type
```

---

# Factory and Open/Closed Principle

A basic Factory often contains:

```csharp
switch
```

Example:

```csharp
return cardType switch
{
    CardType.Visa =>
        new VisaPaymentProcessor(),

    CardType.MasterCard =>
        new MasterCardPaymentProcessor(),

    CardType.Troy =>
        new TroyPaymentProcessor(),

    _ =>
        throw new NotSupportedException()
};
```

When a new type is introduced:

```text
Amex
```

the Factory itself must change.

So a simple Factory does **not automatically guarantee perfect OCP compliance**.

This is an important interview detail.

More extensible implementations can use:

```text
Dependency Injection

Dictionary / Registry

Keyed Services

Reflection

Factory Method

Abstract Factory
```

depending on the problem.

---

# Factory Pattern vs Factory Method

These terms are often mixed together.

A simple centralized factory might look like:

```csharp
PaymentProcessorFactory.Create(type);
```

This is commonly called a:

```text
Simple Factory
```

It is useful, but **Simple Factory is not one of the original GoF patterns**.

The GoF pattern is:

```text
Factory Method
```

---

# Factory Method Pattern

Factory Method says:

> Define an object creation method, but allow subclasses or specialized creators to decide which concrete object should be created.

For example:

```csharp
public abstract class PaymentProcessorCreator
{
    public abstract IPaymentProcessor Create();

    public void Process(decimal amount)
    {
        IPaymentProcessor processor =
            Create();

        processor.Pay(amount);
    }
}
```

Visa Creator:

```csharp
public sealed class VisaProcessorCreator
    : PaymentProcessorCreator
{
    public override IPaymentProcessor Create()
    {
        return new VisaPaymentProcessor();
    }
}
```

MasterCard Creator:

```csharp
public sealed class MasterCardProcessorCreator
    : PaymentProcessorCreator
{
    public override IPaymentProcessor Create()
    {
        return new MasterCardPaymentProcessor();
    }
}
```

Usage:

```csharp
PaymentProcessorCreator creator =
    new VisaProcessorCreator();

creator.Process(1000);
```

Flow:

```text
Client
   ↓
VisaProcessorCreator
   ↓
Create()
   ↓
VisaPaymentProcessor
   ↓
Pay()
```

Here the base creator defines the workflow:

```csharp
Process()
```

but subclasses decide which product gets created.

That is closer to the classical **Factory Method Pattern**.

---

# Factory Method Structure

```text
             Product
                ↑
        ┌───────┴────────┐
        │                │
ConcreteProductA   ConcreteProductB


             Creator
                ↑
        ┌───────┴────────┐
        │                │
ConcreteCreatorA  ConcreteCreatorB
```

Creator:

```csharp
public abstract class Creator
{
    public abstract IProduct CreateProduct();
}
```

Concrete Creator:

```csharp
public sealed class VisaCreator
    : Creator
{
    public override IProduct CreateProduct()
    {
        return new VisaProduct();
    }
}
```

---

# Factory vs Strategy

This is one of the most important comparisons.

## Factory

Answers:

```text
"Which object should I create?"
```

Example:

```text
Visa
   ↓
Create VisaPaymentProcessor
```

## Strategy

Answers:

```text
"Which algorithm should execute?"
```

Example:

```text
Commission Calculation
       ↓
VisaCommissionStrategy
```

Short version:

```text
Factory
→ Creates / selects an implementation.


Strategy
→ Executes an interchangeable behavior.
```

They are often used together.

For example:

```text
Client
  ↓
Factory
  ↓
Select Strategy
  ↓
VisaStrategy
  ↓
Execute Algorithm
```

---

# Factory vs Builder

## Factory

Usually creates an object in one operation.

```csharp
factory.Create(CardType.Visa);
```

Question:

```text
Which object should be created?
```

---

## Builder

Constructs a complex object step by step.

```csharp
new UserBuilder()
    .WithName("Ali")
    .WithEmail("ali@test.com")
    .WithAge(28)
    .Build();
```

Question:

```text
How should this complex object be constructed?
```

Short version:

```text
Factory
→ Which object?


Builder
→ How should the object be assembled?
```

---

# Factory vs Abstract Factory

Factory Method usually focuses on creating one product type.

Abstract Factory creates **families of related objects**.

For example:

```text
ATM Vendor
```

Suppose NCR requires:

```text
NCR Card Reader
NCR Cash Dispenser
NCR Printer
```

and Diebold requires:

```text
Diebold Card Reader
Diebold Cash Dispenser
Diebold Printer
```

We want compatible device families.

```csharp
public interface IAtmDeviceFactory
{
    ICardReader CreateCardReader();

    ICashDispenser CreateCashDispenser();

    IReceiptPrinter CreatePrinter();
}
```

NCR:

```csharp
public sealed class NcrAtmDeviceFactory
    : IAtmDeviceFactory
{
    public ICardReader CreateCardReader()
    {
        return new NcrCardReader();
    }

    public ICashDispenser CreateCashDispenser()
    {
        return new NcrCashDispenser();
    }

    public IReceiptPrinter CreatePrinter()
    {
        return new NcrReceiptPrinter();
    }
}
```

Diebold:

```csharp
public sealed class DieboldAtmDeviceFactory
    : IAtmDeviceFactory
{
    public ICardReader CreateCardReader()
    {
        return new DieboldCardReader();
    }

    public ICashDispenser CreateCashDispenser()
    {
        return new DieboldCashDispenser();
    }

    public IReceiptPrinter CreatePrinter()
    {
        return new DieboldReceiptPrinter();
    }
}
```

This is **Abstract Factory** because we are creating a related family:

```text
NCR Family

Card Reader
Cash Dispenser
Printer
```

instead of one isolated object.

---

# Real-World ATM Example

Suppose the ATM application supports multiple vendors:

```text
NCR
Diebold
Hyosung
```

Without Factory:

```csharp
if (vendor == Vendor.Ncr)
{
    cardReader =
        new NcrCardReader();

    cashDispenser =
        new NcrCashDispenser();

    printer =
        new NcrPrinter();
}
else if (vendor == Vendor.Diebold)
{
    cardReader =
        new DieboldCardReader();

    cashDispenser =
        new DieboldCashDispenser();

    printer =
        new DieboldPrinter();
}
```

This logic may start appearing everywhere.

With Abstract Factory:

```csharp
IAtmDeviceFactory factory =
    vendor switch
    {
        Vendor.Ncr =>
            new NcrAtmDeviceFactory(),

        Vendor.Diebold =>
            new DieboldAtmDeviceFactory(),

        _ =>
            throw new NotSupportedException()
    };
```

Then:

```csharp
ICardReader cardReader =
    factory.CreateCardReader();

ICashDispenser cashDispenser =
    factory.CreateCashDispenser();

IReceiptPrinter printer =
    factory.CreatePrinter();
```

The application no longer manually creates every vendor-specific device.

---

# Factory and Dependency Injection

A common question is:

```text
"If we have Dependency Injection,
do we still need Factory?"
```

Sometimes yes.

DI is excellent when the dependency is known during composition:

```csharp
public PaymentService(
    IPaymentProcessor processor)
```

But suppose we need to choose implementation dynamically:

```text
Request 1 → Visa

Request 2 → MasterCard

Request 3 → Troy
```

The correct implementation depends on runtime data.

That is where Factory / Resolver logic can still be useful.

```text
Dependency Injection
        ↓
Provides available implementations

Factory
        ↓
Selects the correct implementation at runtime
```

They complement each other.

---

# Factory With Runtime Selection

```csharp
public sealed class PaymentService
{
    private readonly PaymentProcessorFactory _factory;

    public PaymentService(
        PaymentProcessorFactory factory)
    {
        _factory = factory;
    }

    public void Pay(
        CardType cardType,
        decimal amount)
    {
        IPaymentProcessor processor =
            _factory.Create(cardType);

        processor.Pay(amount);
    }
}
```

Usage:

```csharp
paymentService.Pay(
    CardType.MasterCard,
    2000);
```

Execution:

```text
PaymentService
      ↓
Factory
      ↓
MasterCard
      ↓
MasterCardPaymentProcessor
      ↓
Pay(2000)
```

---

# When Should We Use Factory?

```diff
@@ Use Factory when: @@

+ Object creation logic is complex.

+ The concrete type depends on runtime information.

+ The client should not know concrete implementations.

+ Multiple implementations share the same abstraction.

+ Constructor logic should be centralized.

+ Third-party or infrastructure implementations need to be selected dynamically.

+ You want object creation responsibilities separated from business logic.
```

---

# When Should We NOT Use Factory?

Do not create a Factory just to hide every `new`.

For example:

```csharp
public class UserFactory
{
    public User Create()
    {
        return new User();
    }
}
```

If there is no creation decision, complexity, configuration, or variation, this Factory adds little value.

Use:

```csharp
new User();
```

instead.

```diff
@@ A Factory should solve a real object-creation problem. @@
```

---

# Advantages

```diff
@@ Advantages @@

+ Centralizes object creation logic.

+ Reduces coupling between clients and concrete implementations.

+ Makes runtime implementation selection easier.

+ Keeps business logic separate from creation logic.

+ Improves testability.

+ Makes object creation rules easier to maintain.

+ Can work naturally with Dependency Injection.
```

---

# Disadvantages

```diff
@@ Disadvantages @@

- Introduces additional classes.

- A large Factory can become a giant switch statement.

- Simple object creation may become unnecessarily complicated.

- Poor Factory design can move complexity instead of eliminating it.
```

---

# Interview Summary

The most important question Factory answers is:

```text
Which object should be created?
```

Basic flow:

```text
Client
  ↓
Factory
  ↓
Creation Decision
  ↓
Concrete Implementation
  ↓
Return Abstraction
```

For example:

```text
CardType.Visa
      ↓
PaymentProcessorFactory
      ↓
VisaPaymentProcessor
      ↓
IPaymentProcessor
```

> **Factory Pattern encapsulates object creation logic and allows the client to work with abstractions without directly depending on concrete implementations.**

---

# Simple Factory vs Factory Method vs Abstract Factory

```text
Simple Factory

Input
 ↓
Factory
 ↓
Choose Concrete Object
```

Example:

```csharp
factory.Create(CardType.Visa);
```

---

```text
Factory Method

Creator
   ↓
Factory Method
   ↓
Subclass decides which Product to create
```

Example:

```csharp
VisaProcessorCreator.Create();
```

---

```text
Abstract Factory

Factory
   ↓
Creates a family of related objects
```

Example:

```text
NcrAtmDeviceFactory
      ↓
CardReader
CashDispenser
Printer
```

Remember:

```text
Simple Factory
→ Centralized creation decision.

Factory Method
→ Subclasses decide which Product to create.

Abstract Factory
→ Creates families of related Products.
```

---

# Factory vs Other Patterns

```text
Factory
→ Which object should I create?

Strategy
→ Which algorithm should I use?

Builder
→ How should I construct this complex object?

Adapter
→ How do I make incompatible interfaces work together?

Decorator
→ How do I add behavior around an existing object?

Abstract Factory
→ Which family of related objects should I create?
```

---

# Interview Questions

### Question 1

What is the primary responsibility of a Factory?

**A)** Execute an algorithm

**B)** Decide and encapsulate which object should be created

**C)** Add behavior to an existing object

**D)** Convert interfaces

**✅ Answer: B**

---

### Question 2

What is the main benefit of Factory Pattern?

**A)** It eliminates all interfaces.

**B)** It reduces coupling between the client and concrete implementations.

**C)** It guarantees better runtime performance.

**D)** It automatically makes every class thread-safe.

**✅ Answer: B**

---

### Question 3

Which question best describes Factory Pattern?

**A)** How should this object be decorated?

**B)** Which object should be created?

**C)** Which event should be published?

**D)** How should this interface be converted?

**✅ Answer: B**

---

### Question 4

What is the main difference between Factory and Strategy?

**A)** They are the same pattern.

**B)** Factory creates/selects objects, while Strategy encapsulates interchangeable algorithms.

**C)** Strategy creates database connections.

**D)** Factory can only create immutable objects.

**✅ Answer: B**

---

### Question 5

What is the main difference between Builder and Factory?

**A)** Builder constructs an object step by step, while Factory focuses on selecting or creating the appropriate object.

**B)** Factory always uses inheritance.

**C)** Builder cannot use interfaces.

**D)** They are identical.

**✅ Answer: A**

---

### Question 6

Which scenario is the best example of Abstract Factory?

**A)** Creating a single `VisaPaymentProcessor`

**B)** Creating NCR-compatible CardReader, CashDispenser, and Printer objects together

**C)** Adding logging to a service

**D)** Translating a vendor SDK interface

**✅ Answer: B**

---

### Question 7

Do Dependency Injection and Factory Pattern always replace each other?

**A)** Yes.

**B)** No. DI can construct dependencies while Factory can select the correct implementation based on runtime information.

**C)** Factory only works without DI.

**D)** DI is a Factory Pattern implementation in every scenario.

**✅ Answer: B**

---

# Final Cheat Sheet

```text
FACTORY
   ↓
Object Creation
   ↓
Which implementation?
```

```text
Client
   ↓
Abstraction
   ↑
Factory
   ↓
Concrete Object
```

For interviews, remember these three points:

```text
1. Encapsulate object creation.

2. Hide concrete implementation decisions from the client.

3. Return abstractions whenever possible.
```

And the most important comparison:

```text
Factory
→ WHICH object?


Strategy
→ WHICH behavior?


Builder
→ HOW to build?


Adapter
→ HOW to connect incompatible interfaces?
```
