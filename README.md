# Design Patterns

```diff
@@ Abstract Factory Pattern @@

Abstract Factory provides an interface for creating
families of related or compatible objects
without exposing their concrete classes.
```

---

# Understanding the Problem

Suppose we are developing ATM software that must work with multiple hardware vendors.

For example:

```text
NCR

Diebold

Hyosung
```

Each vendor provides a complete family of devices:

```text
Card Reader

Cash Dispenser

Receipt Printer
```

For NCR:

```text
NcrCardReader

NcrCashDispenser

NcrReceiptPrinter
```

For Diebold:

```text
DieboldCardReader

DieboldCashDispenser

DieboldReceiptPrinter
```

These objects belong together.

We normally do not want this:

```text
NCR Card Reader

+

Diebold Cash Dispenser

+

Hyosung Printer
```

because they may require completely different SDKs, drivers, configuration, protocols, and initialization logic.

```diff
@@ We need a way to create a compatible FAMILY of objects. @@
```

This is exactly the problem solved by **Abstract Factory**.

---

# Without Abstract Factory

Suppose we write:

```csharp
ICardReader cardReader;
ICashDispenser cashDispenser;
IReceiptPrinter printer;

if (vendor == AtmVendor.Ncr)
{
    cardReader =
        new NcrCardReader();

    cashDispenser =
        new NcrCashDispenser();

    printer =
        new NcrReceiptPrinter();
}
else if (vendor == AtmVendor.Diebold)
{
    cardReader =
        new DieboldCardReader();

    cashDispenser =
        new DieboldCashDispenser();

    printer =
        new DieboldReceiptPrinter();
}
else
{
    throw new NotSupportedException();
}
```

This works.

But the client now knows:

```text
NcrCardReader

NcrCashDispenser

NcrReceiptPrinter

DieboldCardReader

DieboldCashDispenser

DieboldReceiptPrinter
```

The client is responsible for knowing which objects belong together.

That responsibility should be moved somewhere else.

---

# Core Idea

Abstract Factory says:

```text
Do not ask the Client to create every concrete object.

Give the Client a Factory representing a PRODUCT FAMILY.
```

For example:

```text
NcrAtmDeviceFactory
        ↓
 ┌──────┼────────┐
 ↓      ↓        ↓
Card   Cash    Printer
Reader Dispenser
```

or:

```text
DieboldAtmDeviceFactory
        ↓
 ┌──────┼────────┐
 ↓      ↓        ↓
Card   Cash    Printer
Reader Dispenser
```

The important point is:

```diff
@@ One Factory creates multiple related products. @@
```

---

# Product Interfaces

First, define abstractions for the products.

## Card Reader

```csharp
public interface ICardReader
{
    void ReadCard();
}
```

## Cash Dispenser

```csharp
public interface ICashDispenser
{
    void Dispense(decimal amount);
}
```

## Receipt Printer

```csharp
public interface IReceiptPrinter
{
    void PrintReceipt();
}
```

These are the **Abstract Products**.

---

# NCR Product Family

```csharp
public sealed class NcrCardReader
    : ICardReader
{
    public void ReadCard()
    {
        Console.WriteLine(
            "Card read using NCR device.");
    }
}
```

```csharp
public sealed class NcrCashDispenser
    : ICashDispenser
{
    public void Dispense(decimal amount)
    {
        Console.WriteLine(
            $"NCR dispenser dispensed {amount}.");
    }
}
```

```csharp
public sealed class NcrReceiptPrinter
    : IReceiptPrinter
{
    public void PrintReceipt()
    {
        Console.WriteLine(
            "Receipt printed using NCR printer.");
    }
}
```

These are:

```text
Concrete Products
```

belonging to the same family:

```text
NCR
```

---

# Diebold Product Family

```csharp
public sealed class DieboldCardReader
    : ICardReader
{
    public void ReadCard()
    {
        Console.WriteLine(
            "Card read using Diebold device.");
    }
}
```

```csharp
public sealed class DieboldCashDispenser
    : ICashDispenser
{
    public void Dispense(decimal amount)
    {
        Console.WriteLine(
            $"Diebold dispenser dispensed {amount}.");
    }
}
```

```csharp
public sealed class DieboldReceiptPrinter
    : IReceiptPrinter
{
    public void PrintReceipt()
    {
        Console.WriteLine(
            "Receipt printed using Diebold printer.");
    }
}
```

Again, all three belong to one compatible family:

```text
Diebold
```

---

# Abstract Factory

Now define the factory contract.

```csharp
public interface IAtmDeviceFactory
{
    ICardReader CreateCardReader();

    ICashDispenser CreateCashDispenser();

    IReceiptPrinter CreateReceiptPrinter();
}
```

This interface does not know anything about:

```text
NCR

Diebold
```

It only says:

```text
A valid ATM factory must be able to create:

Card Reader

Cash Dispenser

Receipt Printer
```

---

# NCR Factory

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

    public IReceiptPrinter CreateReceiptPrinter()
    {
        return new NcrReceiptPrinter();
    }
}
```

This factory guarantees:

```text
NCR Factory
    ↓
Only NCR products
```

---

# Diebold Factory

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

    public IReceiptPrinter CreateReceiptPrinter()
    {
        return new DieboldReceiptPrinter();
    }
}
```

This guarantees:

```text
Diebold Factory
      ↓
Only Diebold products
```

---

# Client

Now let's create an ATM service.

```csharp
public sealed class AtmService
{
    private readonly ICardReader _cardReader;

    private readonly ICashDispenser _cashDispenser;

    private readonly IReceiptPrinter _receiptPrinter;

    public AtmService(
        IAtmDeviceFactory factory)
    {
        _cardReader =
            factory.CreateCardReader();

        _cashDispenser =
            factory.CreateCashDispenser();

        _receiptPrinter =
            factory.CreateReceiptPrinter();
    }

    public void Withdraw(decimal amount)
    {
        _cardReader.ReadCard();

        _cashDispenser.Dispense(amount);

        _receiptPrinter.PrintReceipt();
    }
}
```

Notice something important.

`AtmService` does not know:

```text
NcrCardReader

DieboldCardReader

NcrCashDispenser

DieboldCashDispenser
```

It only knows abstractions:

```text
ICardReader

ICashDispenser

IReceiptPrinter

IAtmDeviceFactory
```

---

# Usage

## NCR ATM

```csharp
IAtmDeviceFactory factory =
    new NcrAtmDeviceFactory();

var atm =
    new AtmService(factory);

atm.Withdraw(1000);
```

Execution:

```text
AtmService
    ↓
NcrAtmDeviceFactory
    ↓
 ┌─────────────┬────────────────┬─────────────┐
 ↓             ↓                ↓
NcrCardReader NcrCashDispenser NcrReceiptPrinter
```

---

## Diebold ATM

We change only the Factory:

```csharp
IAtmDeviceFactory factory =
    new DieboldAtmDeviceFactory();

var atm =
    new AtmService(factory);

atm.Withdraw(1000);
```

Now:

```text
AtmService
    ↓
DieboldAtmDeviceFactory
    ↓
 ┌──────────────────┬─────────────────────┬──────────────────┐
 ↓                  ↓                     ↓
DieboldCardReader DieboldCashDispenser DieboldReceiptPrinter
```

The `AtmService` itself did not change.

```diff
@@ We changed the entire PRODUCT FAMILY by changing one Factory. @@
```

This is the core power of Abstract Factory.

---

# Structure of Abstract Factory

The classical structure is:

```text
                    AbstractFactory
                          ↑
                 ┌────────┴────────┐
                 │                 │
          ConcreteFactoryA   ConcreteFactoryB
                 │                 │
          ┌──────┼──────┐   ┌──────┼──────┐
          ↓      ↓      ↓   ↓      ↓      ↓
        A1      B1     C1  A2      B2     C2
```

For our example:

```text
                   IAtmDeviceFactory
                          ↑
              ┌───────────┴───────────┐
              │                       │
     NcrAtmDeviceFactory     DieboldAtmDeviceFactory
              │                       │
      ┌───────┼────────┐      ┌───────┼────────┐
      ↓       ↓        ↓      ↓       ↓        ↓
     NCR     NCR      NCR   Diebold Diebold Diebold
    Reader Dispenser Printer Reader Dispenser Printer
```

---

# Abstract Factory Components

There are usually four important parts.

## 1. Abstract Factory

Defines methods for creating related products.

```csharp
IAtmDeviceFactory
```

---

## 2. Concrete Factory

Creates one specific product family.

```csharp
NcrAtmDeviceFactory

DieboldAtmDeviceFactory
```

---

## 3. Abstract Products

Contracts used by the application.

```csharp
ICardReader

ICashDispenser

IReceiptPrinter
```

---

## 4. Concrete Products

Vendor-specific implementations.

```text
NcrCardReader

NcrCashDispenser

NcrReceiptPrinter

DieboldCardReader

DieboldCashDispenser

DieboldReceiptPrinter
```

---

# Why Do We Use Abstract Factory?

The primary reason is:

```diff
@@ We want to create related objects together without coupling the Client to their concrete classes. @@
```

Without Abstract Factory:

```text
Client
 ├── new NcrCardReader()
 ├── new NcrCashDispenser()
 └── new NcrReceiptPrinter()
```

With Abstract Factory:

```text
Client
   ↓
IAtmDeviceFactory
   ↓
Compatible Product Family
```

This gives us several benefits.

---

# Benefit 1 — Product Family Consistency

Imagine accidentally writing:

```csharp
ICardReader cardReader =
    new NcrCardReader();

ICashDispenser cashDispenser =
    new DieboldCashDispenser();

IReceiptPrinter printer =
    new NcrReceiptPrinter();
```

This may be technically compilable because all objects implement the correct interfaces.

But architecturally it may be invalid.

Abstract Factory prevents the client from manually mixing families.

```text
NcrAtmDeviceFactory
        ↓
NCR Reader
NCR Dispenser
NCR Printer
```

The whole family is created consistently.

---

# Benefit 2 — Replace the Entire Family

Suppose a bank moves from NCR ATMs to Diebold.

Without Abstract Factory, the application may contain many concrete constructions:

```csharp
new NcrCardReader();

new NcrCashDispenser();

new NcrReceiptPrinter();
```

With Abstract Factory:

```csharp
IAtmDeviceFactory factory =
    new NcrAtmDeviceFactory();
```

becomes:

```csharp
IAtmDeviceFactory factory =
    new DieboldAtmDeviceFactory();
```

The client remains unchanged.

---

# Benefit 3 — Hide Vendor-Specific Details

NCR might require:

```text
NCR SDK

NCR configuration

NCR initialization

NCR COM components
```

Diebold may require something completely different.

The application should not know those details.

```text
Application
     ↓
IAtmDeviceFactory
     ↓
Vendor-specific creation logic
```

The Factory creates the right objects.

---

# More Realistic Enterprise Example

Abstract Factory is not only useful for physical hardware.

Suppose a financial application supports two different infrastructure providers.

```text
Provider A

Provider B
```

Each provider offers:

```text
Payment Gateway

Fraud Client

Notification Client
```

Define:

```csharp
public interface IPaymentGateway
{
    void Pay(decimal amount);
}
```

```csharp
public interface IFraudClient
{
    bool Check(decimal amount);
}
```

```csharp
public interface INotificationClient
{
    void Send(string message);
}
```

Abstract Factory:

```csharp
public interface IFinancialProviderFactory
{
    IPaymentGateway CreatePaymentGateway();

    IFraudClient CreateFraudClient();

    INotificationClient CreateNotificationClient();
}
```

Provider A:

```csharp
public sealed class ProviderAFactory
    : IFinancialProviderFactory
{
    public IPaymentGateway CreatePaymentGateway()
    {
        return new ProviderAPaymentGateway();
    }

    public IFraudClient CreateFraudClient()
    {
        return new ProviderAFraudClient();
    }

    public INotificationClient CreateNotificationClient()
    {
        return new ProviderANotificationClient();
    }
}
```

Provider B:

```csharp
public sealed class ProviderBFactory
    : IFinancialProviderFactory
{
    public IPaymentGateway CreatePaymentGateway()
    {
        return new ProviderBPaymentGateway();
    }

    public IFraudClient CreateFraudClient()
    {
        return new ProviderBFraudClient();
    }

    public INotificationClient CreateNotificationClient()
    {
        return new ProviderBNotificationClient();
    }
}
```

Now we can switch the entire infrastructure family.

---

# Another Real-World Example — Cloud Providers

Suppose an application supports:

```text
AWS

Azure
```

Each provider has:

```text
Storage

Queue

Secrets
```

We may define:

```csharp
public interface ICloudFactory
{
    IStorageService CreateStorage();

    IQueueService CreateQueue();

    ISecretService CreateSecretService();
}
```

AWS Factory:

```text
AwsCloudFactory
    ↓
S3Storage
SqsQueue
SecretsManager
```

Azure Factory:

```text
AzureCloudFactory
      ↓
BlobStorage
ServiceBusQueue
KeyVault
```

This is another strong Abstract Factory scenario.

---

# Abstract Factory vs Factory

This is one of the most important interview questions.

## Factory

Usually creates **one type of product**.

```text
PaymentProcessorFactory
        ↓
VisaPaymentProcessor
```

Question:

```text
Which payment processor should I create?
```

---

## Abstract Factory

Creates a **family of related products**.

```text
NcrAtmDeviceFactory
        ↓
CardReader
CashDispenser
Printer
```

Question:

```text
Which compatible family of products should I create?
```

Short version:

```text
Factory
→ One product decision.


Abstract Factory
→ Product family decision.
```

---

# Abstract Factory vs Factory Method

These are related but not identical.

## Factory Method

Usually lets subclasses decide which product to create.

```text
Creator
   ↓
Factory Method
   ↓
Concrete Product
```

Example:

```csharp
public abstract class PaymentCreator
{
    public abstract IPaymentProcessor Create();
}
```

---

## Abstract Factory

Contains multiple factory methods for creating related products.

```csharp
public interface IAtmDeviceFactory
{
    ICardReader CreateCardReader();

    ICashDispenser CreateCashDispenser();

    IReceiptPrinter CreateReceiptPrinter();
}
```

You can think of Abstract Factory as:

```text
A collection of factory methods
that create a compatible product family.
```

This is a useful interview explanation.

---

# Abstract Factory vs Builder

## Abstract Factory

Creates a family of related objects.

```text
NCR Factory
   ↓
Reader
Dispenser
Printer
```

## Builder

Constructs one complex object step by step.

```text
UserBuilder
    ↓
Name
    ↓
Email
    ↓
Address
    ↓
Build()
```

Short version:

```text
Abstract Factory
→ Which FAMILY should I create?


Builder
→ How should ONE complex object be constructed?
```

---

# Abstract Factory vs Strategy

## Strategy

Chooses interchangeable behavior.

```text
Commission
    ↓
Visa Strategy

OR

MasterCard Strategy
```

## Abstract Factory

Chooses interchangeable families of objects.

```text
ATM Hardware
      ↓
NCR Family

OR

Diebold Family
```

Short version:

```text
Strategy
→ Behavior family.


Abstract Factory
→ Object family.
```

---

# Abstract Factory vs Adapter

## Adapter

Makes incompatible interfaces work together.

```text
Our Interface
     ↓
Adapter
     ↓
Vendor Interface
```

## Abstract Factory

Creates compatible objects.

```text
Factory
  ↓
Related Products
```

They can also be used together.

For example:

```text
NcrAtmDeviceFactory
        ↓
NcrCardReaderAdapter
NcrCashDispenserAdapter
NcrPrinterAdapter
```

This is actually a very realistic architecture.

The Factory chooses the NCR family.

The Adapters translate the NCR SDK into our application interfaces.

---

# Abstract Factory + Adapter

This combination is especially useful when integrating hardware or third-party SDKs.

Suppose the real NCR SDK contains:

```text
NcrCardSdk

NcrCashSdk

NcrPrinterSdk
```

Our application uses:

```text
ICardReader

ICashDispenser

IReceiptPrinter
```

We can create:

```text
NcrCardReaderAdapter

NcrCashDispenserAdapter

NcrPrinterAdapter
```

and then:

```csharp
public sealed class NcrAtmDeviceFactory
    : IAtmDeviceFactory
{
    public ICardReader CreateCardReader()
    {
        return new NcrCardReaderAdapter(
            new NcrCardSdk());
    }

    public ICashDispenser CreateCashDispenser()
    {
        return new NcrCashDispenserAdapter(
            new NcrCashSdk());
    }

    public IReceiptPrinter CreateReceiptPrinter()
    {
        return new NcrPrinterAdapter(
            new NcrPrinterSdk());
    }
}
```

Now two patterns work together:

```text
Abstract Factory
       ↓
Select Product Family
       ↓
NCR


Adapter
       ↓
Translate Vendor SDK
       ↓
Our Interfaces
```

This is a strong enterprise architecture example.

---

# Abstract Factory With Dependency Injection

Modern .NET applications often use DI.

Instead of writing:

```csharp
return new NcrCardReader();
```

the Factory may receive dependencies through its constructor.

For example:

```csharp
public sealed class NcrAtmDeviceFactory
    : IAtmDeviceFactory
{
    private readonly NcrCardReader _cardReader;

    private readonly NcrCashDispenser _cashDispenser;

    private readonly NcrReceiptPrinter _printer;

    public NcrAtmDeviceFactory(
        NcrCardReader cardReader,
        NcrCashDispenser cashDispenser,
        NcrReceiptPrinter printer)
    {
        _cardReader = cardReader;
        _cashDispenser = cashDispenser;
        _printer = printer;
    }

    public ICardReader CreateCardReader()
    {
        return _cardReader;
    }

    public ICashDispenser CreateCashDispenser()
    {
        return _cashDispenser;
    }

    public IReceiptPrinter CreateReceiptPrinter()
    {
        return _printer;
    }
}
```

DI registrations:

```csharp
services.AddScoped<NcrCardReader>();

services.AddScoped<NcrCashDispenser>();

services.AddScoped<NcrReceiptPrinter>();

services.AddScoped<
    IAtmDeviceFactory,
    NcrAtmDeviceFactory>();
```

In real systems, DI may take over much of the construction work.

Abstract Factory is still useful when:

```text
The product family must be selected at runtime.

Different environments require different families.

Multiple related dependencies must change together.
```

---

# Runtime Factory Selection

Suppose vendor information comes from configuration:

```text
ATM_VENDOR=NCR
```

We could have:

```csharp
public enum AtmVendor
{
    Ncr,
    Diebold
}
```

and a factory provider:

```csharp
public sealed class AtmFactoryProvider
{
    public IAtmDeviceFactory GetFactory(
        AtmVendor vendor)
    {
        return vendor switch
        {
            AtmVendor.Ncr =>
                new NcrAtmDeviceFactory(),

            AtmVendor.Diebold =>
                new DieboldAtmDeviceFactory(),

            _ =>
                throw new NotSupportedException()
        };
    }
}
```

Then:

```csharp
IAtmDeviceFactory factory =
    provider.GetFactory(
        AtmVendor.Ncr);
```

and the Abstract Factory handles the rest.

---

# Important OCP Detail

Abstract Factory supports Open/Closed Principle when adding new **families**.

For example:

```text
NCR

Diebold

Hyosung
```

Adding Hyosung means creating:

```text
HyosungCardReader

HyosungCashDispenser

HyosungReceiptPrinter

HyosungAtmDeviceFactory
```

Existing client code can remain unchanged.

However, there is an important disadvantage.

Suppose we add a completely new product type:

```text
BarcodeScanner
```

Then this interface changes:

```csharp
public interface IAtmDeviceFactory
{
    ICardReader CreateCardReader();

    ICashDispenser CreateCashDispenser();

    IReceiptPrinter CreateReceiptPrinter();

    IBarcodeScanner CreateBarcodeScanner();
}
```

Now every concrete Factory must implement the new method:

```text
NcrAtmDeviceFactory

DieboldAtmDeviceFactory

HyosungAtmDeviceFactory
```

So Abstract Factory is:

```diff
+ Very good at adding new product FAMILIES.

- More expensive when adding new PRODUCT TYPES.
```

This is one of the most important interview points.

---

# When Should We Use Abstract Factory?

```diff
@@ Use Abstract Factory when: @@

+ You need to create families of related objects.

+ Products from the same family must remain compatible.

+ The Client should not depend on concrete product classes.

+ You need to switch an entire implementation family.

+ Different vendors provide equivalent groups of components.

+ Environment-specific implementations must change together.

+ You want vendor-specific construction details in one place.
```

Strong examples:

```text
ATM Vendors

Cloud Providers

Database Provider Families

Cross-platform UI Components

Third-party Infrastructure Providers

Hardware SDK Families
```

---

# When Should We NOT Use Abstract Factory?

Do not use Abstract Factory when you only need to create one simple object.

For example:

```csharp
new User();
```

does not require:

```text
UserAbstractFactory
```

Also, if the products are unrelated:

```text
EmailSender

DatabaseConnection

UserRepository
```

putting them into one Abstract Factory just because they are objects is usually bad design.

The products should represent a meaningful family.

---

# Advantages

```diff
@@ Advantages @@

+ Creates compatible families of related objects.

+ Hides concrete implementations from the Client.

+ Makes switching entire product families easy.

+ Centralizes vendor-specific construction logic.

+ Reduces coupling.

+ Encourages programming against abstractions.

+ Prevents accidental mixing of incompatible implementations.

+ Works well with Adapter and Dependency Injection.
```

---

# Disadvantages

```diff
@@ Disadvantages @@

- Introduces many interfaces and classes.

- Can be overengineering for simple systems.

- Adding a new PRODUCT TYPE requires changing every Factory.

- The design can become difficult to understand if product families are poorly defined.

- Factories with too many creation methods can become large.
```

---

# Key Design Tradeoff

Remember this:

```text
Adding a new family
        ↓
Easy
```

Example:

```text
NCR
Diebold
Hyosung ← NEW
```

We mainly add new implementations.

But:

```text
Adding a new product type
        ↓
More expensive
```

Example:

```text
CardReader
CashDispenser
Printer
BarcodeScanner ← NEW
```

Every Factory must now support `BarcodeScanner`.

This distinction is extremely important.

---

# Interview Summary

The main question Abstract Factory answers is:

```text
Which FAMILY of related objects should I create?
```

Execution:

```text
Client
   ↓
Abstract Factory
   ↓
Concrete Factory
   ↓
Compatible Product Family
```

Example:

```text
ATM Application
      ↓
IAtmDeviceFactory
      ↓
NcrAtmDeviceFactory
      ↓
 ┌─────────────┬────────────────┬─────────────┐
 ↓             ↓                ↓
CardReader CashDispenser ReceiptPrinter
```

> **Abstract Factory provides an interface for creating families of related objects without requiring the client to know their concrete classes.**

---

# Interview Questions

### Question 1

What is the primary purpose of Abstract Factory?

**A)** Create one object step by step

**B)** Create families of related objects

**C)** Convert incompatible interfaces

**D)** Execute different algorithms

**✅ Answer: B**

---

### Question 2

Which scenario is the best example of Abstract Factory?

**A)** Creating one `VisaPaymentProcessor`

**B)** Creating an NCR CardReader, CashDispenser, and Printer as one compatible family

**C)** Adding logging around a service

**D)** Validating an ATM PIN

**✅ Answer: B**

---

### Question 3

What is the main difference between Factory and Abstract Factory?

**A)** There is no difference.

**B)** Factory usually focuses on one product, while Abstract Factory creates families of related products.

**C)** Abstract Factory cannot use interfaces.

**D)** Factory only works with databases.

**✅ Answer: B**

---

### Question 4

Which operation is generally easier with Abstract Factory?

**A)** Adding a completely new product type to every family

**B)** Adding a new product family

**C)** Removing all interfaces

**D)** Changing an object's algorithm

**✅ Answer: B**

---

### Question 5

Why can adding a new product type be expensive?

Suppose we add:

```text
BarcodeScanner
```

**A)** Because all concrete factories may need a new `CreateBarcodeScanner()` implementation.

**B)** Because Abstract Factory cannot create new objects.

**C)** Because inheritance is forbidden.

**D)** Because only one factory may exist.

**✅ Answer: A**

---

### Question 6

What does the Abstract Factory return?

**A)** Only concrete classes

**B)** Usually abstractions representing related products

**C)** Events

**D)** Middleware

**✅ Answer: B**

---

### Question 7

Which patterns work especially well together when integrating an ATM vendor SDK?

**A)** Abstract Factory + Adapter

**B)** Observer + Singleton only

**C)** Builder + Command only

**D)** Strategy + Prototype only

**✅ Answer: A**

The Abstract Factory chooses the vendor family, while Adapters translate vendor-specific SDKs into application interfaces.

---

# Final Cheat Sheet

```text
Factory
→ Which OBJECT should I create?


Abstract Factory
→ Which FAMILY of objects should I create?


Builder
→ HOW should I construct an object?


Strategy
→ Which BEHAVIOR should I execute?


Adapter
→ How do I make incompatible interfaces compatible?
```

The most important mental model:

```text
               ABSTRACT FACTORY
                       ↓
                 Product Family
                       ↓
            ┌──────────┼──────────┐
            ↓          ↓          ↓
         Product A  Product B  Product C
```

ATM example:

```text
IAtmDeviceFactory
       ↓
      NCR
       ↓
┌──────┼──────────┐
↓      ↓          ↓
Reader Dispenser Printer
```

Change one Factory:

```text
NCR
 ↓
Diebold
```

and the entire product family changes.

That is the core idea of **Abstract Factory Pattern**.
