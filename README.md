# Facade Design Pattern

## What Is the Facade Pattern?

The **Facade Pattern** provides a simple, unified interface to a complex subsystem.

Instead of forcing the client to understand and coordinate many different services, the client communicates with a single **Facade**.

In short:

```text
Without Facade:

Client
  ↓
Service A
Service B
Service C
Service D
```

With Facade:

```text
Client
  ↓
Facade
  ↓
Service A
Service B
Service C
Service D
```

The main idea is:

> **Hide subsystem complexity behind a simpler high-level interface.**

---

# The Problem

Suppose we are building an e-commerce application.

When the customer clicks:

```text
Pay
```

the system may need to perform several operations:

```text
Fraud Check
    ↓
Charge Card
    ↓
Create Ledger Record
    ↓
Send Notification
```

Without Facade Pattern, the client may need to coordinate all these services itself.

```csharp
bool fraudResult =
    fraudService.CheckFraud(
        cardNumber,
        amount);

if (!fraudResult)
{
    return;
}

bool paymentResult =
    paymentGateway.Charge(
        cardNumber,
        amount);

if (!paymentResult)
{
    return;
}

ledgerService.RecordPayment(
    orderId,
    amount);

notificationService.SendPaymentSuccess(
    customerEmail,
    orderId);
```

This means the client knows:

```text
Which service runs first?

Which service runs second?

What happens if fraud validation fails?

What happens if payment fails?

When should the ledger be updated?

When should the notification be sent?
```

The client knows too much about the subsystem.

---

# Facade Pattern Solution

We introduce:

```text
PaymentFacade
```

Now the client only needs to call:

```csharp
paymentFacade.ProcessPayment(
    orderId,
    cardNumber,
    amount,
    customerEmail);
```

The Facade coordinates the internal services.

```text
                  Client
                     ↓
               PaymentFacade
                     ↓
 ┌───────────────────┼───────────────────┐
 ↓                   ↓                   ↓
FraudService    PaymentGateway      LedgerService
                                         ↓
                              NotificationService
```

A clearer representation:

```text
Program
   ↓
PaymentFacade
   ↓
FraudService
   ↓
PaymentGateway
   ↓
LedgerService
   ↓
NotificationService
```

---

# Project Structure

```text
ECommerceFacadePattern

├── FraudService.cs
├── PaymentGateway.cs
├── LedgerService.cs
├── NotificationService.cs
├── PaymentFacade.cs
└── Program.cs
```

---

# 1. FraudService

The `FraudService` is responsible for checking whether the payment looks suspicious.

```csharp
namespace ECommerceFacadePattern
{
    public class FraudService
    {
        public bool CheckFraud(
            string cardNumber,
            decimal amount)
        {
            Console.WriteLine(
                "Checking fraud...");

            Console.WriteLine(
                $"Card: {cardNumber}");

            Console.WriteLine(
                $"Amount: {amount:N2} TL");

            Console.WriteLine(
                "Fraud check completed successfully.");

            return true;
        }
    }
}
```

Its responsibility is only:

```text
Fraud Validation
```

It does not need to know:

```text
How payment is processed
How ledger entries are created
How notifications are sent
```

---

# 2. PaymentGateway

The `PaymentGateway` is responsible for charging the customer's card.

```csharp
namespace ECommerceFacadePattern
{
    public class PaymentGateway
    {
        public bool Charge(
            string cardNumber,
            decimal amount)
        {
            Console.WriteLine(
                "Processing payment...");

            Console.WriteLine(
                $"Card: {cardNumber}");

            Console.WriteLine(
                $"Amount: {amount:N2} TL");

            Console.WriteLine(
                "Payment completed successfully.");

            return true;
        }
    }
}
```

In a real application, this could communicate with:

```text
Stripe

PayPal

Iyzico

Masterpass

Bank API

Payment Provider
```

---

# 3. LedgerService

The `LedgerService` records the financial transaction.

```csharp
namespace ECommerceFacadePattern
{
    public class LedgerService
    {
        public void RecordPayment(
            int orderId,
            decimal amount)
        {
            Console.WriteLine(
                "Creating ledger record...");

            Console.WriteLine(
                $"OrderId: {orderId}");

            Console.WriteLine(
                $"Amount: {amount:N2} TL");

            Console.WriteLine(
                "Ledger record created.");
        }
    }
}
```

Its responsibility is:

```text
Accounting / Financial Record
```

---

# 4. NotificationService

The `NotificationService` notifies the customer after the payment succeeds.

```csharp
namespace ECommerceFacadePattern
{
    public class NotificationService
    {
        public void SendPaymentSuccess(
            string email,
            int orderId)
        {
            Console.WriteLine(
                "Sending notification...");

            Console.WriteLine(
                $"Email: {email}");

            Console.WriteLine(
                $"OrderId: {orderId}");

            Console.WriteLine(
                "Payment success notification sent.");
        }
    }
}
```

---

# 5. PaymentFacade

This is the most important class in the example.

The `PaymentFacade` knows how the payment subsystem should be coordinated.

```csharp
namespace ECommerceFacadePattern
{
    public class PaymentFacade
    {
        private readonly FraudService _fraudService;
        private readonly PaymentGateway _paymentGateway;
        private readonly LedgerService _ledgerService;
        private readonly NotificationService _notificationService;

        public PaymentFacade(
            FraudService fraudService,
            PaymentGateway paymentGateway,
            LedgerService ledgerService,
            NotificationService notificationService)
        {
            _fraudService = fraudService;
            _paymentGateway = paymentGateway;
            _ledgerService = ledgerService;
            _notificationService = notificationService;
        }

        public void ProcessPayment(
            int orderId,
            string cardNumber,
            decimal amount,
            string customerEmail)
        {
            Console.WriteLine(
                "Payment process started.");

            Console.WriteLine(
                "-----------------------------");

            bool fraudResult =
                _fraudService.CheckFraud(
                    cardNumber,
                    amount);

            if (!fraudResult)
            {
                Console.WriteLine(
                    "Fraud validation failed.");

                return;
            }

            bool paymentResult =
                _paymentGateway.Charge(
                    cardNumber,
                    amount);

            if (!paymentResult)
            {
                Console.WriteLine(
                    "Payment could not be completed.");

                return;
            }

            _ledgerService.RecordPayment(
                orderId,
                amount);

            _notificationService.SendPaymentSuccess(
                customerEmail,
                orderId);

            Console.WriteLine(
                "-----------------------------");

            Console.WriteLine(
                "Payment process completed successfully.");
        }
    }
}
```

The Facade coordinates:

```text
Fraud Check
    ↓
Payment
    ↓
Ledger
    ↓
Notification
```

The client does not need to know this workflow.

---

# 6. Program.cs

```csharp
using ECommerceFacadePattern;

FraudService fraudService =
    new FraudService();

PaymentGateway paymentGateway =
    new PaymentGateway();

LedgerService ledgerService =
    new LedgerService();

NotificationService notificationService =
    new NotificationService();

PaymentFacade paymentFacade =
    new PaymentFacade(
        fraudService,
        paymentGateway,
        ledgerService,
        notificationService);

paymentFacade.ProcessPayment(
    orderId: 1001,
    cardNumber: "4532123412345678",
    amount: 2500,
    customerEmail: "alican@test.com");
```

The client only needs to know:

```csharp
paymentFacade.ProcessPayment(...);
```

It does not need to understand the internal payment subsystem.

---

# Execution Flow

```text
Program
   ↓
PaymentFacade.ProcessPayment()
   ↓
FraudService.CheckFraud()
   ↓
PaymentGateway.Charge()
   ↓
LedgerService.RecordPayment()
   ↓
NotificationService.SendPaymentSuccess()
```

---

# Without Facade

The client directly communicates with every subsystem service.

```text
Program
   ↓
FraudService
   ↓
PaymentGateway
   ↓
LedgerService
   ↓
NotificationService
```

The client must understand the whole process.

For example:

```csharp
if (!fraudService.CheckFraud(
        cardNumber,
        amount))
{
    return;
}

if (!paymentGateway.Charge(
        cardNumber,
        amount))
{
    return;
}

ledgerService.RecordPayment(
    orderId,
    amount);

notificationService.SendPaymentSuccess(
    email,
    orderId);
```

The client is tightly coupled to the payment workflow.

---

# With Facade

The client communicates with one object.

```text
Program
   ↓
PaymentFacade
```

Internally:

```text
PaymentFacade
   ↓
FraudService
   ↓
PaymentGateway
   ↓
LedgerService
   ↓
NotificationService
```

Client code becomes:

```csharp
paymentFacade.ProcessPayment(
    orderId,
    cardNumber,
    amount,
    customerEmail);
```

Much simpler.

---

# Main Participants

## Client

The object that wants to use the subsystem.

In this example:

```text
Program.cs
```

---

## Facade

Provides a simple high-level API.

In this example:

```text
PaymentFacade
```

---

## Subsystem Classes

These classes perform the actual work.

```text
FraudService

PaymentGateway

LedgerService

NotificationService
```

---

# General Structure

```text
              Client
                 ↓
               Facade
                 ↓
       ┌─────────┼─────────┐
       ↓         ↓         ↓
Subsystem A  Subsystem B  Subsystem C
```

Our example:

```text
               Program
                  ↓
            PaymentFacade
                  ↓
 ┌────────────────┼──────────────────┐
 ↓                ↓                  ↓
FraudService  PaymentGateway    LedgerService
                                    ↓
                          NotificationService
```

---

# Why Is Facade Useful?

## 1. Simpler Client Code

Without Facade:

```csharp
fraudService.CheckFraud(...);

paymentGateway.Charge(...);

ledgerService.RecordPayment(...);

notificationService.SendPaymentSuccess(...);
```

With Facade:

```csharp
paymentFacade.ProcessPayment(...);
```

---

# 2. Lower Coupling

Without Facade:

```text
Client
 ↓
FraudService

Client
 ↓
PaymentGateway

Client
 ↓
LedgerService

Client
 ↓
NotificationService
```

With Facade:

```text
Client
 ↓
PaymentFacade
```

The client becomes less dependent on subsystem details.

---

# 3. Centralized Workflow

Suppose the required execution order is:

```text
Fraud
 ↓
Payment
 ↓
Ledger
 ↓
Notification
```

The Facade keeps this workflow in one place.

---

# 4. Easier Maintenance

Suppose a new requirement arrives.

Before charging the card, we now need:

```text
3D Secure Validation
```

The new flow becomes:

```text
Fraud
 ↓
3D Secure
 ↓
Payment
 ↓
Ledger
 ↓
Notification
```

Without Facade, every client might need to change.

With Facade, clients still call:

```csharp
paymentFacade.ProcessPayment(...);
```

Only the internal implementation changes.

---

# 5. Better High-Level APIs

Instead of exposing low-level operations:

```text
CheckFraud()

Charge()

CreateLedger()

SendEmail()
```

we can expose meaningful business operations:

```text
ProcessPayment()

CreateOrder()

CreateShipment()

CancelOrder()
```

---

# Important Detail

Facade does **not** necessarily make subsystem classes inaccessible.

This is still possible:

```csharp
fraudService.CheckFraud(...);
```

The Facade simply gives clients an easier alternative:

```csharp
paymentFacade.ProcessPayment(...);
```

So:

> Facade simplifies access to the subsystem. It does not necessarily completely hide the subsystem.

---

# Real-World Examples

## Banking

```text
WithdrawalFacade.Withdraw()
        ↓
CardService
        ↓
BalanceService
        ↓
FraudService
        ↓
CashService
        ↓
JournalService
```

The client only calls:

```csharp
withdrawalFacade.Withdraw(...);
```

---

## E-Commerce Order

```text
OrderFacade.CreateOrder()
        ↓
StockService
        ↓
PaymentService
        ↓
InvoiceService
        ↓
ShippingService
        ↓
NotificationService
```

---

## File Upload

```text
FileUploadFacade.Upload()
        ↓
VirusScanner
        ↓
CompressionService
        ↓
StorageService
        ↓
MetadataService
```

---

## Video Conversion

```text
VideoConverterFacade.Convert()
        ↓
CodecService
        ↓
AudioService
        ↓
BitrateService
        ↓
Encoder
```

---

# Facade vs Command Pattern

This distinction is important.

## Command Pattern

Command answers:

```text
WHAT should be done?
```

Example:

```text
ProcessPaymentCommand
```

The operation itself becomes an object.

```csharp
ICommand command =
    new ProcessPaymentCommand(...);
```

It can then be:

```text
Queued

Stored

Retried

Logged

Scheduled

Executed later
```

---

## Facade Pattern

Facade answers:

```text
How can I use this complex subsystem easily?
```

Example:

```csharp
paymentFacade.ProcessPayment(...);
```

Facade does not primarily represent an operation as an object.

It simplifies a complex subsystem.

---

# Command + Facade Together

They can be used together.

```text
ProcessPaymentCommand
        ↓
PaymentFacade
        ↓
FraudService
        ↓
PaymentGateway
        ↓
LedgerService
        ↓
NotificationService
```

Example:

```csharp
public class ProcessPaymentCommand : ICommand
{
    private readonly PaymentFacade _paymentFacade;

    public ProcessPaymentCommand(
        PaymentFacade paymentFacade)
    {
        _paymentFacade =
            paymentFacade;
    }

    public void Execute()
    {
        _paymentFacade.ProcessPayment(
            1001,
            "4532123412345678",
            2500,
            "alican@test.com");
    }
}
```

Responsibilities:

```text
ProcessPaymentCommand
→ Represents the operation.

PaymentFacade
→ Simplifies the complex payment subsystem.
```

---

# Facade vs Strategy

Strategy answers:

```text
HOW should an algorithm be performed?
```

Example:

```text
Payment Strategy

CreditCardPaymentStrategy

PayPalPaymentStrategy

CryptoPaymentStrategy
```

Facade answers:

```text
How can I simplify access to this subsystem?
```

Example:

```text
PaymentFacade
```

Short version:

```text
Strategy
→ HOW?

Facade
→ SIMPLIFY
```

---

# Facade vs Adapter

Adapter solves incompatible interfaces.

```text
Old System
    ↓
Adapter
    ↓
New Interface
```

Adapter answers:

```text
How can incompatible interfaces work together?
```

Facade answers:

```text
How can a complex subsystem be easier to use?
```

Short version:

```text
Adapter
→ Makes interfaces compatible.

Facade
→ Makes a subsystem simpler.
```

---

# Facade vs Mediator

These patterns can also look similar.

## Facade

```text
Client
 ↓
Facade
 ↓
Subsystem A
Subsystem B
Subsystem C
```

The Facade provides a simplified entry point.

---

## Mediator

```text
A ─┐
B ─┼→ Mediator
C ─┘
```

The Mediator coordinates communication between objects.

Short version:

```text
Facade
→ Simplifies subsystem access.

Mediator
→ Coordinates object communication.
```

---

# Facade vs Proxy

Proxy provides another object with the same or similar interface and controls access to the real object.

Example:

```text
Client
 ↓
Proxy
 ↓
Real Service
```

Possible responsibilities:

```text
Security

Caching

Lazy Loading

Remote Communication
```

Facade instead simplifies multiple subsystem classes.

```text
Client
 ↓
Facade
 ↓
A
B
C
D
```

---

# When Should We Use Facade Pattern?

Use Facade when:

```text
The subsystem is complex.

The client should not know subsystem details.

Several classes must be coordinated.

You want a simple high-level API.

You want to reduce coupling.

You want to centralize orchestration.

You want to isolate legacy systems.

You want to provide a clean boundary between layers.
```

Typical examples:

```text
PaymentFacade

OrderFacade

BankingFacade

FileUploadFacade

VideoConverterFacade

ShippingFacade

ReportingFacade
```

---

# When Should We NOT Use Facade?

Do not create a Facade when the subsystem is already simple.

For example:

```csharp
public class UserFacade
{
    private readonly UserService _userService;

    public User GetUser(int id)
    {
        return _userService.GetUser(id);
    }
}
```

If the Facade only forwards one simple method and gives no architectural value, it may be unnecessary.

---

# Avoid the God Facade

A bad design could look like this:

```csharp
public class ApplicationFacade
{
    public void CreateUser()
    {
    }

    public void ProcessPayment()
    {
    }

    public void ShipOrder()
    {
    }

    public void GenerateReport()
    {
    }

    public void ResetPassword()
    {
    }

    public void BlockCard()
    {
    }
}
```

This class contains unrelated responsibilities.

It becomes a:

```text
God Object
```

Prefer focused Facades:

```text
PaymentFacade

ShippingFacade

ReportingFacade

AccountFacade
```

Each Facade should represent a meaningful subsystem.

---

# Advantages

```text
+ Simplifies complex subsystem usage.

+ Reduces client-side complexity.

+ Reduces coupling.

+ Provides a clean high-level interface.

+ Centralizes subsystem orchestration.

+ Makes client code easier to read.

+ Makes internal changes easier to isolate.

+ Can provide a stable boundary around legacy systems.

+ Makes common workflows easier to reuse.
```

---

# Disadvantages

```text
- Can become a God Object.

- Can become too large.

- May hide useful subsystem functionality.

- May introduce unnecessary abstraction for simple systems.

- Developers may incorrectly place all business logic inside the Facade.

- A very large Facade can become difficult to maintain.
```

---

# Important Design Principle

The Facade should mainly coordinate subsystem objects.

For example:

```csharp
public void ProcessPayment(...)
{
    _fraudService.CheckFraud(...);

    _paymentGateway.Charge(...);

    _ledgerService.RecordPayment(...);

    _notificationService.SendPaymentSuccess(...);
}
```

The actual specialized business logic should remain inside the appropriate subsystem classes.

```text
Fraud logic
→ FraudService

Payment gateway logic
→ PaymentGateway

Accounting logic
→ LedgerService

Notification logic
→ NotificationService
```

The Facade orchestrates them.

---

# Interview Definition

A good short interview answer:

> **Facade Pattern provides a simplified and unified interface to a complex subsystem, allowing clients to use that subsystem without needing to understand its internal details.**

---

# Interview Example

Suppose payment processing requires:

```text
Fraud Check

Payment Gateway

Ledger

Notification
```

Without Facade:

```text
Controller
   ↓
FraudService
   ↓
PaymentGateway
   ↓
LedgerService
   ↓
NotificationService
```

With Facade:

```text
Controller
   ↓
PaymentFacade
   ↓
FraudService
PaymentGateway
LedgerService
NotificationService
```

The Controller only needs:

```csharp
paymentFacade.ProcessPayment();
```

---

# Interview Questions

## Question 1

What is the primary purpose of Facade Pattern?

**A)** Create related object families

**B)** Simplify access to a complex subsystem

**C)** Change an algorithm at runtime

**D)** Create objects without exposing construction logic

**Answer: B**

---

## Question 2

Which participant provides the simplified interface?

**A)** Client

**B)** Receiver

**C)** Facade

**D)** Factory

**Answer: C**

---

## Question 3

Does Facade always prevent direct access to subsystem classes?

**A)** Yes

**B)** No

**C)** Only in .NET

**D)** Only when dependency injection is used

**Answer: B**

---

## Question 4

Which class is the Facade in our example?

**A)** `FraudService`

**B)** `LedgerService`

**C)** `PaymentGateway`

**D)** `PaymentFacade`

**Answer: D**

---

## Question 5

What problem does Facade primarily solve?

**A)** Object creation

**B)** Interface incompatibility

**C)** Subsystem complexity

**D)** Event subscription

**Answer: C**

---

## Question 6

What is the main difference between Command and Facade?

**A)** They are identical.

**B)** Command represents an operation as an object, while Facade simplifies access to a complex subsystem.

**C)** Facade represents algorithms.

**D)** Command can only be used in desktop applications.

**Answer: B**

---

## Question 7

Which is a good use case for Facade?

**A)** A payment flow requiring fraud, gateway, ledger and notification services

**B)** Reading a single property

**C)** Selecting between sorting algorithms

**D)** Creating subclasses dynamically

**Answer: A**

---

## Question 8

What is a common risk when using Facade?

**A)** It always causes memory leaks.

**B)** It can become a God Object if too many responsibilities are added.

**C)** It cannot work with dependency injection.

**D)** It requires inheritance.

**Answer: B**

---

# Final Cheat Sheet

```text
FACADE PATTERN
```

Purpose:

```text
Simplify a complex subsystem.
```

Structure:

```text
Client
  ↓
Facade
  ↓
Subsystem Classes
```

Our example:

```text
Program
   ↓
PaymentFacade
   ↓
FraudService
   ↓
PaymentGateway
   ↓
LedgerService
   ↓
NotificationService
```

Remember:

```text
Facade
→ Simplifies subsystem access.

Command
→ Represents WHAT should be done.

Strategy
→ Defines HOW something should be done.

Adapter
→ Makes incompatible interfaces compatible.

Mediator
→ Coordinates communication between objects.
```

The most important sentence:

> **Facade Pattern hides subsystem coordination behind a simple, high-level interface.**
> ::: 
