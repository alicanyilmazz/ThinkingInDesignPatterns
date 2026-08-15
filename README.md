# Design Patterns

```diff
- Decorator Pattern
```

```diff
@@ Let's say we have a service @@
```

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

```diff
@@ Concrete implementation: @@
```

```csharp
public class PaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Payment completed : {amount}");
    }
}
```

```diff
@@ Usage: @@
```

```csharp
IPaymentService paymentService = new PaymentService();

paymentService.Pay(100);
```

```diff
@@ Output @@
```

```diff
+ Payment completed : 100
```

```diff
@@ Then the client starts requesting new features @@

✅ Let's add logging before every payment.

✅ Done.

✅ Then:

✅ Let's add caching.

✅ Then:

✅ Let's add authorization.

✅ Then:

✅ Let's add retry logic.

✅ Then:

✅ Let's measure performance.

- What should we do now?
```

---

```diff
- Bad Solution
- Put everything inside PaymentService.
```

```csharp
public class PaymentService
{
    public void Pay(decimal amount)
    {
        Log();

        Validate();

        Retry();

        Performance();

        Cache();

        Payment();

        Log();
    }
}
```

```diff
@@ Now PaymentService violates the Single Responsibility Principle. @@

Because PaymentService is responsible for:

* processing payments
* logging
* caching
* retry logic
* authorization
* performance measurement
```

---

```diff
@@ What does the Decorator Pattern say? @@

+ Don't modify the original class.
+ Add new behavior around it.
```

```text
Logging
   ↓
Caching
   ↓
Authorization
   ↓
PaymentService
```

```diff
@@ We wrap the service layer by layer. @@
```

---

```diff
@@ First Decorator @@
```

```diff
- Interface
```

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

```diff
- Concrete Service
```

```csharp
public class PaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Payment : {amount}");
    }
}
```

```diff
- Decorator
```

```csharp
public class LoggingPaymentDecorator : IPaymentService
{
    private readonly IPaymentService _paymentService;

    public LoggingPaymentDecorator(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine("Log started");

        _paymentService.Pay(amount);

        Console.WriteLine("Log finished");
    }
}
```

```diff
- Usage
```

```csharp
IPaymentService payment =
    new LoggingPaymentDecorator(
        new PaymentService());

payment.Pay(500);
```

```diff
@@ Output @@
```

```text
Log started

Payment : 500

Log finished
```

---

```diff
@@ Second Decorator @@
+ Let's add Authorization.
```

```csharp
public class AuthorizationDecorator : IPaymentService
{
    private readonly IPaymentService _service;

    public AuthorizationDecorator(IPaymentService service)
    {
        _service = service;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine("Checking permission");

        _service.Pay(amount);
    }
}
```

```diff
@@ Now we can compose them like this: @@
```

```csharp
IPaymentService payment =
    new AuthorizationDecorator(
        new LoggingPaymentDecorator(
            new PaymentService()));
```

```diff
@@ Execution order @@
```

```text
Authorization
      ↓
Logging
      ↓
PaymentService
```

```diff
@@ We can add as many decorators as we need. @@
```

```text
Cache
   ↓
Retry
   ↓
Authorization
   ↓
Logging
   ↓
PaymentService
```

---

```diff
@@ How does the chain work? @@
```

```text
Pay()
  ↓
AuthorizationDecorator
  ↓
LoggingDecorator
  ↓
PaymentService
```

```diff
✅ This is the core idea of the Decorator Pattern.

✅ Every decorator implements the same interface.

✅ Every decorator also holds a reference to that same interface.
```

```text
Decorator
   ↓
IPaymentService
   ↓
PaymentService
```

---

```diff
@@ The Most Important Characteristic @@
```

```diff
- A decorator implements the same interface:
```

```csharp
public class LoggingDecorator : IPaymentService
```

```diff
@@ And it also contains a reference to the same interface: @@
```

```csharp
private readonly IPaymentService _paymentService;
```

```diff
@@ Therefore, it behaves like an IPaymentService while also wrapping another IPaymentService. @@

@@ This is the essence of the Decorator Pattern. @@
```

---

```diff
@@ Real-World .NET Example @@
```

```csharp
IRepository
```

```diff
+ Concrete repository:
```

```text
SqlRepository
```

```diff
@@ Now suppose we want to add caching. @@
```

```text
CachingRepositoryDecorator
```

```diff
@@ The flow could look like this: @@
```

```text
GetById()
    ↓
Is the item in cache?
    ↓
Yes → Return it
    ↓
No
    ↓
Repository.GetById()
    ↓
Store it in cache
    ↓
Return it
```

```diff
@@ The original Repository does not need to change. @@
```

---

```diff
@@ Banking Example @@
```

```text
Suppose an ATM has a Withdraw() operation.

Every withdrawal may require:

Fraud Check
     ↓
AML Check
     ↓
Logging
     ↓
Performance
     ↓
Withdraw
```

```diff
@@ Each of these cross-cutting behaviors can potentially be implemented using decorators. @@
```

---

```diff
@@ Where do we see this idea in ASP.NET Core? @@
```

```diff
@@ For example: @@
@@ MediatR @@
@@ Pipeline Behaviors @@
```

```text
Validation
    ↓
Logging
    ↓
Transaction
    ↓
Handler
```

```diff
@@ Pipeline behaviors use a decorator-like approach around the request handler. @@
```

```diff
@@ Scrutor @@
```

```csharp
services.Decorate<IOrderService, LoggingOrderService>();
```

```diff
@@ This is a direct example of decorating a registered service. @@
```

---

```diff
@@ Advantages @@

✅ Supports the Open/Closed Principle.

✅ New behavior can be added without modifying existing code.

✅ Helps preserve the Single Responsibility Principle.

✅ Multiple behaviors can be composed together.

✅ Behaviors can be added dynamically at runtime.
```

---

```diff
@@ Disadvantages @@

❌ Too many decorators can make the execution flow harder to understand and debug.

❌ Deep decorator chains can make object construction more complex.
```

---

```diff
@@ Decorator vs Inheritance @@

- Bad approach:
```

```text
PaymentService
      ↓
LoggingPaymentService
      ↓
CachingLoggingPaymentService
      ↓
AuthorizationCachingLoggingPaymentService
```

```diff
@@ This can quickly become unmanageable. @@

@@ The number of classes can grow dramatically. @@

@@ Decorator avoids this problem by favoring composition over inheritance. @@
```

---

```diff
@@ Decorator vs Proxy @@

@@ A common interview question. @@

- Decorator

> Adds additional behavior to an object.

Examples:

* Logging
* Caching
* Retry
* Validation
* Performance measurement

- Proxy

> Controls or manages access to another object.

Examples:

* Remote Proxy
* Lazy Proxy
* Security / Protection Proxy
```

```diff
@@ Why does Decorator prefer composition over inheritance? @@

@@ Because behaviors can be dynamically combined at runtime without creating a large inheritance hierarchy. @@
```

> **Decorator Pattern extends an object's behavior without modifying the original object by wrapping it with other objects that implement the same interface.**

---

```diff
@@ Interview Question @@

@@ In the following decorator chain, which one executes first? @@
```

```csharp
IPaymentService payment =
    new AuthorizationDecorator(
        new LoggingDecorator(
            new PaymentService()));
```

```text
AuthorizationDecorator executes first.

Then:

AuthorizationDecorator
        ↓
LoggingDecorator
        ↓
PaymentService
```

---

## Questions

### Question 1

Which of the following is one of the most important advantages of the Decorator Pattern?

**A)** It reduces the memory usage of an object.

**B)** It allows behaviors to be dynamically added at runtime.

**C)** It reduces the number of constructors.

**D)** It eliminates the need for interfaces.

**✅ Answer: B**

The Decorator Pattern allows us to dynamically compose additional behaviors around an existing object.

---

### Question 2

Which SOLID principle is most directly supported by the Decorator Pattern?

**A)** SRP — Single Responsibility Principle

**B)** OCP — Open/Closed Principle

**C)** ISP — Interface Segregation Principle

**D)** DIP — Dependency Inversion Principle

**✅ Answer: B — Open/Closed Principle**

We can extend the behavior of an existing component without modifying its implementation.

> **Note:** Decorator can also help with SRP because each decorator can have a single responsibility. However, **OCP** is usually the primary answer expected in interviews.

---

### Question 3

Which statement about the Decorator Pattern is **incorrect**?

**A)** A decorator implements the same interface as the object it wraps.

**B)** A decorator holds a reference to the same interface.

**C)** A decorator can add additional behavior.

**D)** A decorator decides which concrete object should be created.

**✅ Answer: D**

Deciding which concrete object should be created is primarily the responsibility of **Factory Patterns**, not the Decorator Pattern.
