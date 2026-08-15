# Design Patterns

```diff
@@ Chain of Responsibility (CoR) Pattern @@

Chain of Responsibility is an important pattern for understanding
pipeline-based architectures.

You will encounter similar concepts in:

+ ASP.NET Core Middleware
+ MediatR Pipeline Behaviors
+ Authentication / Authorization Pipelines
+ Exception Handling
+ Validation Pipelines
+ HTTP Processing Pipelines
+ Banking and ATM Transaction Flows
```

---

# Understanding the Problem

```diff
@@ Suppose an ATM receives a withdrawal request. @@

Before dispensing cash, several checks must be performed.
```

```text
Card Inserted
     ↓
Validate Card
     ↓
Validate PIN
     ↓
Check Card Status
     ↓
Check Daily Limit
     ↓
Check Balance
     ↓
Fraud Check
     ↓
Check ATM Cash
     ↓
Dispense Cash
```

Without Chain of Responsibility, we might write:

```csharp
public void Withdraw()
{
    ValidateCard();

    ValidatePin();

    CheckLimit();

    CheckBalance();

    CheckFraud();

    CheckCash();

    Dispense();
}
```

```diff
@@ This works, but the method becomes harder to maintain as requirements grow. @@
```

Tomorrow:

```text
Add AML Check
```

The method changes.

Then:

```text
Add Blacklist Check
```

The method changes again.

Then:

```text
Skip PIN validation for QR withdrawals
```

Again, the same method must be modified.

Eventually:

```text
Withdraw()

ValidateCard()
ValidatePin()
CheckBlacklist()
CheckAML()
CheckLimit()
CheckBalance()
CheckFraud()
CheckCash()
...
```

```diff
@@ The transaction flow becomes tightly coupled to one large method. @@
```

---

# Chain of Responsibility Solution

```diff
@@ Chain of Responsibility separates each processing step into its own Handler. @@
```

Instead of one large method:

```text
CardValidationHandler
        ↓
PinValidationHandler
        ↓
LimitHandler
        ↓
BalanceHandler
        ↓
FraudHandler
        ↓
CashHandler
        ↓
WithdrawHandler
```

Each Handler:

```text
1. Performs its own responsibility.

2. Decides whether the request should continue.

3. Passes the request to the next Handler if appropriate.
```

---

# Request

First, let's define the request that moves through the chain.

```csharp
public class WithdrawRequest
{
    public string CardNumber { get; set; }

    public string Pin { get; set; }

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }
}
```

The same request travels through the entire pipeline:

```text
WithdrawRequest
      ↓
Card Handler
      ↓
PIN Handler
      ↓
Balance Handler
      ↓
Cash Handler
```

---

# Handler Interface

```csharp
public interface IHandler
{
    void SetNext(IHandler handler);

    void Handle(WithdrawRequest request);
}
```

There are two important operations:

```text
SetNext()
   ↓
Defines the next Handler


Handle()
   ↓
Processes the request
```

---

# Base Handler

```diff
@@ We can create a Base Handler to avoid repeating chain-management logic in every Handler. @@
```

```csharp
public abstract class Handler : IHandler
{
    private IHandler? _next;

    public void SetNext(IHandler handler)
    {
        _next = handler;
    }

    public virtual void Handle(WithdrawRequest request)
    {
        _next?.Handle(request);
    }
}
```

The most important line is:

```csharp
_next?.Handle(request);
```

This means:

```text
Current Handler
      ↓
Finish current responsibility
      ↓
Call next Handler
```

That line keeps the chain moving.

---

# Card Validation Handler

```csharp
public class CardValidationHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("Card validated.");

        base.Handle(request);
    }
}
```

The Handler first performs its own responsibility:

```csharp
Console.WriteLine("Card validated.");
```

Then:

```csharp
base.Handle(request);
```

passes the request to the next Handler.

```text
CardValidationHandler
        ↓
Card Valid
        ↓
base.Handle()
        ↓
Next Handler
```

---

# PIN Validation Handler

```csharp
public class PinValidationHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("PIN validated.");

        base.Handle(request);
    }
}
```

Again:

```text
Validate PIN
     ↓
Continue Chain
```

---

# Balance Handler

This Handler demonstrates one of the most important characteristics of Chain of Responsibility.

```csharp
public class BalanceHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        if (request.Balance < request.Amount)
        {
            Console.WriteLine("Insufficient balance.");

            return;
        }

        Console.WriteLine("Balance is sufficient.");

        base.Handle(request);
    }
}
```

Notice:

```csharp
return;
```

There is no:

```csharp
base.Handle(request);
```

after an insufficient balance is detected.

Therefore:

```text
BalanceHandler
      ↓
Insufficient Balance
      ↓
return
      ↓
STOP
```

The next Handler is never called.

This ability to **short-circuit the chain** is an important characteristic of the pattern.

---

# Cash Handler

```csharp
public class CashHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("ATM dispensed the cash.");

        base.Handle(request);
    }
}
```

In this example, this is effectively the final business Handler.

Because there is no Handler after it:

```csharp
_next?.Handle(request);
```

does nothing.

---

# Building the Chain

Now we create the Handlers:

```csharp
var card = new CardValidationHandler();

var pin = new PinValidationHandler();

var balance = new BalanceHandler();

var cash = new CashHandler();
```

Then connect them:

```csharp
card.SetNext(pin);

pin.SetNext(balance);

balance.SetNext(cash);
```

The resulting chain is:

```text
Card
 ↓
PIN
 ↓
Balance
 ↓
Cash
```

---

# Executing the Chain

We only call the **first Handler**:

```csharp
card.Handle(request);
```

We do NOT write:

```csharp
card.Handle(request);
pin.Handle(request);
balance.Handle(request);
cash.Handle(request);
```

Each Handler is responsible for forwarding the request.

```text
Client
  ↓
Card.Handle()
  ↓
PIN.Handle()
  ↓
Balance.Handle()
  ↓
Cash.Handle()
```

---

# Short-Circuiting the Chain

Suppose the requested amount is:

```text
Amount  = 1000
Balance = 500
```

Execution becomes:

```text
Card
 ↓
PASS
 ↓
PIN
 ↓
PASS
 ↓
Balance
 ↓
FAIL
 ↓
STOP
```

`CashHandler` never executes.

Why?

Because `BalanceHandler` does not call:

```csharp
base.Handle(request);
```

when validation fails.

---

# Core Idea

```diff
@@ Every Handler essentially says: @@
```

```text
"I will perform my responsibility."

            ↓

"Should processing continue?"

       ↙           ↘

     YES            NO
      ↓              ↓
 Next Handler       STOP
```

> **Chain of Responsibility passes a request through a sequence of Handlers. Each Handler can process the request and decide whether processing should continue to the next Handler.**

---

# ASP.NET Core Middleware

One of the most important real-world examples of this concept is the ASP.NET Core middleware pipeline.

A typical pipeline may look like:

```csharp
app.UseExceptionHandler("/error");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

Conceptually:

```text
Request
   ↓
Exception Handling
   ↓
Routing
   ↓
Authentication
   ↓
Authorization
   ↓
Endpoint
```

A custom middleware typically contains:

```csharp
await next(context);
```

Conceptually, this is similar to:

```csharp
base.Handle(request);
```

in our Handler example.

Both mean:

```text
Continue processing
        ↓
Call the next component
```

---

# Middleware Short-Circuiting

A middleware does not have to call `next`.

For example:

```csharp
if (!IsAllowed(context))
{
    context.Response.StatusCode =
        StatusCodes.Status403Forbidden;

    return;
}

await next(context);
```

If the request is not allowed:

```text
Current Middleware
       ↓
403 Forbidden
       ↓
return
       ↓
Pipeline Stops
```

The downstream middleware and endpoint are not executed.

```diff
@@ This short-circuiting behavior is one of the reasons middleware pipelines are often explained using Chain of Responsibility. @@
```

> ASP.NET Core middleware is not a textbook implementation of the GoF Chain of Responsibility pattern, but it strongly applies the same request-pipeline and short-circuiting concepts.

---

# An Important Middleware Detail

Middleware can also execute logic **after** the next component finishes.

```csharp
Console.WriteLine("Before");

await next(context);

Console.WriteLine("After");
```

Suppose we have:

```text
Middleware A
Middleware B
Controller
```

Execution becomes:

```text
A Before
   ↓
B Before
   ↓
Controller
   ↓
B After
   ↓
A After
```

This gives ASP.NET Core middleware both:

```text
Chain of Responsibility characteristics

+

Decorator / nested pipeline characteristics
```

This distinction is useful in interviews.

---

# MediatR Pipeline Behaviors

MediatR pipeline behaviors use a similar pipeline concept.

For example:

```text
Request
   ↓
Logging
   ↓
Validation
   ↓
Performance
   ↓
Transaction
   ↓
Handler
```

A behavior typically calls:

```csharp
await next();
```

which delegates execution to the next behavior.

Conceptually:

```text
LoggingBehavior
      ↓
ValidationBehavior
      ↓
TransactionBehavior
      ↓
RequestHandler
```

Again, MediatR pipeline behaviors can also execute code both before and after `next()`, so they are often described as having both **Decorator** and **Chain of Responsibility-like** characteristics.

---

# Exception Middleware

Suppose the pipeline is:

```text
Exception Middleware
        ↓
Authentication
        ↓
Authorization
        ↓
Controller
```

The exception middleware can execute:

```csharp
try
{
    await next(context);
}
catch (Exception ex)
{
    // Handle exception
}
```

The request moves down:

```text
Exception Middleware
        ↓
Authentication
        ↓
Authorization
        ↓
Controller
```

If the Controller throws an exception:

```text
Controller
   ↓
Exception
   ↑
Authorization
   ↑
Authentication
   ↑
Exception Middleware
   ↓
Handle Exception
```

This is why exception middleware is generally registered early in the pipeline: it needs to wrap downstream processing.

---

# API Gateway / Microservice Example

The same pipeline concept can appear at an API Gateway:

```text
Incoming Request
       ↓
JWT Validation
       ↓
Rate Limiting
       ↓
IP Filtering
       ↓
Request Validation
       ↓
Routing
       ↓
Microservice
```

Any component may reject the request:

```text
JWT Validation
      ↓
Invalid Token
      ↓
401
      ↓
STOP
```

or:

```text
Rate Limiting
      ↓
Limit Exceeded
      ↓
429
      ↓
STOP
```

---

# ATM Example

A more realistic ATM transaction chain could look like:

```text
WithdrawRequest
      ↓
CardValidationHandler
      ↓
PinValidationHandler
      ↓
CardStatusHandler
      ↓
DailyLimitHandler
      ↓
BalanceHandler
      ↓
FraudHandler
      ↓
CashAvailabilityHandler
      ↓
DispenseHandler
      ↓
JournalHandler
```

If Fraud detects a problem:

```text
FraudHandler
     ↓
Transaction Rejected
     ↓
STOP
```

Therefore:

```text
CashAvailabilityHandler
DispenseHandler
JournalHandler
```

are not executed in the normal downstream flow.

---

# Advantages

```diff
@@ Advantages @@

+ Handlers are loosely coupled.

+ Each Handler can have a single responsibility.

+ New Handlers can be added without rewriting one large processing method.

+ Handler order can be changed.

+ Processing can be stopped at any point.

+ Individual Handlers are easier to test.

+ Complex processing pipelines become easier to compose.
```

---

# Disadvantages

```diff
@@ Disadvantages @@

- Handler order can become critical.

- Incorrect ordering may introduce difficult bugs.

- Long chains can be harder to debug.

- It may not always be obvious which Handler stopped the request.

- The request is not guaranteed to reach the end of the chain.
```

For example, this order would be dangerous:

```text
CashHandler
     ↓
BalanceHandler
```

because cash would be dispensed before checking the balance.

The correct order is:

```text
BalanceHandler
     ↓
CashHandler
```

---

# Chain of Responsibility vs Strategy

### Strategy

Chooses one algorithm from multiple alternatives.

```text
Commission Calculation
        ↓
   ┌────┼────┐
   ↓    ↓    ↓
 Visa Master Troy
```

Usually:

```text
Visa OR MasterCard OR Troy
```

### Chain of Responsibility

Passes a request through multiple processing steps.

```text
Card
 ↓
PIN
 ↓
Balance
 ↓
Fraud
 ↓
Cash
```

The short version:

```text
Strategy
   ↓
Which algorithm should perform the job?


Chain of Responsibility
   ↓
Which handlers should process the request,
and how far should it travel through the chain?
```

---

# Chain of Responsibility vs Observer

### Observer

One event occurs and multiple subscribers are notified.

```text
OrderCreated
      ↓
 ┌────┼─────┐
 ↓    ↓     ↓
Email SMS Analytics
```

### Chain of Responsibility

One request travels through handlers sequentially.

```text
Request
   ↓
Handler A
   ↓
Handler B
   ↓
Handler C
```

So:

```text
Observer
   ↓
Broadcast / notification


Chain
   ↓
Sequential processing
```

---

# Chain of Responsibility vs Command

### Command

Represents an action or request as an object.

```text
WithdrawCommand
```

For example:

```csharp
public class WithdrawCommand
{
    public decimal Amount { get; set; }
}
```

### Chain of Responsibility

Defines how that request moves through processing steps.

```text
WithdrawCommand
      ↓
Validation
      ↓
Fraud
      ↓
Balance
      ↓
Cash
      ↓
Journal
```

Therefore:

```text
Command
   ↓
"What operation should be executed?"


Chain
   ↓
"Which processing steps should handle it?"
```

They can be used together.

---

# Chain of Responsibility vs Decorator

This is an important interview comparison.

### Decorator

Adds behavior around another object.

```text
Logging
   ↓
Retry
   ↓
Authorization
   ↓
PaymentService
```

The main goal is:

```text
Extend behavior without modifying
the original object.
```

### Chain of Responsibility

Passes a request through a sequence of handlers.

```text
Validation
    ↓
Fraud
    ↓
Limit
    ↓
Balance
```

A Handler can decide:

```text
STOP
```

and prevent downstream processing.

The main goal is:

```text
Pass a request through a processing chain
while reducing coupling between the sender
and individual handlers.
```

---

# Interview Summary

```diff
@@ Chain of Responsibility @@
```

```text
Request
   ↓
Handler A
   ↓
Handler B
   ↓
Handler C
   ↓
Handler D
```

Each Handler:

```text
Receive Request
      ↓
Perform Responsibility
      ↓
Continue?
   ↙      ↘
 YES      NO
  ↓        ↓
Next      STOP
```

> **Chain of Responsibility Pattern passes a request through a chain of handlers. Each handler can process the request and decide whether to pass it to the next handler or stop further processing.**

For interviews, remember these four concepts:

```text
1. Handler

2. Next Handler

3. Request Pipeline

4. Short-Circuit
```

And remember the ASP.NET Core connection:

```text
Handler               → Middleware

Handle(request)        → InvokeAsync(context)

base.Handle(request)   → await next(context)

return                 → Short-circuit
```

---

# Interview Questions

### Question 1

What is the primary purpose of the Chain of Responsibility Pattern?

**A)** Change an algorithm at runtime

**B)** Pass a request through a sequence of handlers

**C)** Create objects

**D)** Convert one interface into another

**✅ Answer: B**

---

### Question 2

When can a Chain of Responsibility stop processing?

**A)** Immediately after the first Handler

**B)** Only after the last Handler

**C)** When a Handler decides not to forward the request

**D)** Every Handler must always execute

**✅ Answer: C**

---

### Question 3

Which ASP.NET Core concept strongly resembles Chain of Responsibility?

**A)** Dependency Injection

**B)** Middleware Pipeline

**C)** AutoMapper

**D)** Entity Framework Core

**✅ Answer: B**

---

### Question 4

What does `await next(context)` conceptually mean inside middleware?

**A)** Create a new object

**B)** Invoke the next component in the pipeline

**C)** Publish an event

**D)** Start a database transaction

**✅ Answer: B**

---

### Question 5

Which pattern best represents the following ATM flow?

```text
Card Validation
      ↓
PIN Validation
      ↓
Limit Check
      ↓
Fraud Check
      ↓
Dispense Cash
```

**A)** Strategy

**B)** Factory

**C)** Chain of Responsibility

**D)** Observer

**✅ Answer: C**

---

# Final Cheat Sheet

```text
Strategy
→ Choose ONE algorithm.

Decorator
→ WRAP an object with additional behavior.

Observer
→ NOTIFY multiple subscribers.

Command
→ REPRESENT an operation as an object.

Chain of Responsibility
→ PASS a request through multiple handlers.
```
