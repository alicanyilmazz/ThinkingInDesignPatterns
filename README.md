# Observer Design Pattern

> **Behavioral Design Pattern**

The **Observer Pattern** defines a one-to-many dependency between objects.

When the state of one object changes, all interested objects are automatically notified.

In simple terms:

```text
Something happens
       ↓
An event is published
       ↓
All interested subscribers are notified
```

The Observer Pattern is one of the fundamental ideas behind:

- C# events and delegates
- UI event handling
- Notification systems
- Domain events
- Event-driven applications
- Reactive systems
- Message-based architectures

---

# 1. The Problem

Imagine that we have a product whose stock can change.

Whenever the stock changes, several parts of the system need to react:

```text
Product Stock Changed
        |
        +----> Send Email
        |
        +----> Send Push Notification
        |
        +----> Update Dashboard
        |
        +----> Write Analytics
```

A naive implementation could look like this:

```csharp
public class ProductStock
{
    private readonly EmailService _emailService;
    private readonly MobileNotificationService _mobileService;
    private readonly DashboardService _dashboardService;

    public int Stock { get; private set; }

    public ProductStock(
        EmailService emailService,
        MobileNotificationService mobileService,
        DashboardService dashboardService)
    {
        _emailService = emailService;
        _mobileService = mobileService;
        _dashboardService = dashboardService;
    }

    public void UpdateStock(int newStock)
    {
        Stock = newStock;

        _emailService.Send(Stock);
        _mobileService.Send(Stock);
        _dashboardService.Refresh(Stock);
    }
}
```

The problem is that `ProductStock` now knows about every action that must happen after a stock change.

What happens when we add another requirement?

```text
Send SMS
Write audit log
Update analytics
Notify another system
```

We must modify `ProductStock` again.

```csharp
public void UpdateStock(int newStock)
{
    Stock = newStock;

    _emailService.Send(Stock);
    _mobileService.Send(Stock);
    _dashboardService.Refresh(Stock);

    _smsService.Send(Stock);
    _analyticsService.Track(Stock);
    _auditService.Write(Stock);
}
```

The class becomes increasingly coupled to unrelated components.

This violates an important design goal:

> The object producing the change should not need to know every object interested in that change.

This is where the **Observer Pattern** becomes useful.

---

# 2. Core Idea

There are two main roles.

### Subject / Publisher

The object being observed.

It maintains the subscribers and notifies them when something happens.

### Observer / Subscriber

An object interested in changes occurring in the subject.

```text
                     Subject
                        |
                        |
                     Notify
                        |
          +-------------+-------------+
          |             |             |
          v             v             v
      Observer A    Observer B    Observer C
```

The important point is that the subject does not need to know the concrete behavior of its observers.

It only knows that they follow a common contract.

---

# 3. Classic Observer Implementation

Let's implement the pattern manually first.

This is useful because it shows exactly what happens internally before using C#'s built-in `event` mechanism.

## Observer

```csharp
public interface IObserver
{
    void Update(ProductStock stock);
}
```

Every observer must implement the `Update` method.

---

## Subject

```csharp
public interface ISubject
{
    void Attach(IObserver observer);

    void Detach(IObserver observer);

    void Notify();
}
```

The subject is responsible for:

```text
Attach   -> Subscribe an observer
Detach   -> Unsubscribe an observer
Notify   -> Notify all subscribed observers
```

---

# 4. Product Stock Example

The `ProductStock` class is our **Concrete Subject**.

```csharp
public class ProductStock : ISubject
{
    private readonly List<IObserver> _observers = [];

    public string ProductName { get; }

    public int Stock { get; private set; }

    public ProductStock(string productName)
    {
        ProductName = productName;
    }

    public void Attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void UpdateStock(int newStock)
    {
        Stock = newStock;

        Notify();
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}
```

Notice what `ProductStock` does **not** know:

```text
Email
Push notification
Dashboard
Analytics
SMS
```

It only knows:

```csharp
IObserver
```

---

# 5. Concrete Observers

## Email Observer

```csharp
public class EmailStockObserver : IObserver
{
    public void Update(ProductStock stock)
    {
        Console.WriteLine(
            $"EMAIL -> {stock.ProductName} stock changed to {stock.Stock}");
    }
}
```

## Mobile Observer

```csharp
public class MobileStockObserver : IObserver
{
    public void Update(ProductStock stock)
    {
        Console.WriteLine(
            $"PUSH -> {stock.ProductName} stock changed to {stock.Stock}");
    }
}
```

## Dashboard Observer

```csharp
public class DashboardStockObserver : IObserver
{
    public void Update(ProductStock stock)
    {
        Console.WriteLine(
            $"DASHBOARD -> {stock.ProductName}: {stock.Stock}");
    }
}
```

---

# 6. Using the Observer Pattern

```csharp
var product = new ProductStock("PlayStation 6");

var emailObserver = new EmailStockObserver();
var mobileObserver = new MobileStockObserver();
var dashboardObserver = new DashboardStockObserver();

product.Attach(emailObserver);
product.Attach(mobileObserver);
product.Attach(dashboardObserver);

product.UpdateStock(10);
```

Output:

```text
EMAIL -> PlayStation 6 stock changed to 10
PUSH -> PlayStation 6 stock changed to 10
DASHBOARD -> PlayStation 6: 10
```

One event happened:

```text
Stock changed
```

But several independent components reacted to it.

---

# 7. Unsubscribing

Observers can also stop listening.

```csharp
product.Detach(emailObserver);

product.UpdateStock(5);
```

Now the email observer will no longer receive notifications.

```text
PUSH -> PlayStation 6 stock changed to 5
DASHBOARD -> PlayStation 6: 5
```

This dynamic subscription mechanism is an important characteristic of the Observer Pattern.

---

# 8. Observer Pattern in C#

The previous implementation teaches us how Observer works internally.

However, C# already provides a very natural mechanism for this idea:

```text
delegate
+
event
```

Instead of manually maintaining:

```text
List<IObserver>
Attach()
Detach()
Notify()
```

we can use an `event`.

Let's see this with another example.

---

# 9. Order Status Example with C# Events

Imagine an order moving through different states:

```text
Pending
   ↓
Paid
   ↓
Shipped
   ↓
Delivered
```

Several components may be interested whenever the status changes.

```text
                  Order
                    |
              StatusChanged
                    |
         +----------+----------+
         |          |          |
         v          v          v
       Email     Analytics    SMS
```

---

# 10. EventArgs

First, we define the information that will be sent to subscribers.

```csharp
public class OrderStatusChangedEventArgs : EventArgs
{
    public int OrderId { get; init; }

    public string OldStatus { get; init; } = string.Empty;

    public string NewStatus { get; init; } = string.Empty;
}
```

---

# 11. Publisher

```csharp
public class Order
{
    public int Id { get; }

    public string Status { get; private set; }

    public event EventHandler<OrderStatusChangedEventArgs>? StatusChanged;

    public Order(int id)
    {
        Id = id;
        Status = "Pending";
    }

    public void ChangeStatus(string newStatus)
    {
        var oldStatus = Status;

        Status = newStatus;

        OnStatusChanged(oldStatus, newStatus);
    }

    protected virtual void OnStatusChanged(
        string oldStatus,
        string newStatus)
    {
        StatusChanged?.Invoke(
            this,
            new OrderStatusChangedEventArgs
            {
                OrderId = Id,
                OldStatus = oldStatus,
                NewStatus = newStatus
            });
    }
}
```

The important part is:

```csharp
public event EventHandler<OrderStatusChangedEventArgs>? StatusChanged;
```

This event defines the contract that subscribers must follow.

---

# 12. EventHandler<T>

`EventHandler<T>` is a delegate.

Conceptually, it looks like this:

```csharp
public delegate void EventHandler<TEventArgs>(
    object? sender,
    TEventArgs e);
```

Therefore:

```csharp
EventHandler<OrderStatusChangedEventArgs>
```

expects methods with this shape:

```csharp
void SomeMethod(
    object? sender,
    OrderStatusChangedEventArgs e)
```

For example:

```csharp
public void OnStatusChanged(
    object? sender,
    OrderStatusChangedEventArgs e)
{
}
```

The method name does not matter.

The **method signature** must be compatible with the delegate.

---

# 13. Email Subscriber

```csharp
public class EmailOrderObserver
{
    public void OnStatusChanged(
        object? sender,
        OrderStatusChangedEventArgs e)
    {
        Console.WriteLine(
            $"EMAIL -> Order {e.OrderId}: " +
            $"{e.OldStatus} -> {e.NewStatus}");
    }
}
```

---

# 14. Analytics Subscriber

```csharp
public class AnalyticsOrderObserver
{
    public void OnStatusChanged(
        object? sender,
        OrderStatusChangedEventArgs e)
    {
        Console.WriteLine(
            $"ANALYTICS -> Order {e.OrderId} changed to {e.NewStatus}");
    }
}
```

---

# 15. Subscribing to an Event

```csharp
var order = new Order(123);

var emailObserver = new EmailOrderObserver();
var analyticsObserver = new AnalyticsOrderObserver();

order.StatusChanged += emailObserver.OnStatusChanged;
order.StatusChanged += analyticsObserver.OnStatusChanged;
```

The `+=` operator subscribes a handler to the event.

Now:

```csharp
order.ChangeStatus("Paid");
```

produces:

```text
EMAIL -> Order 123: Pending -> Paid
ANALYTICS -> Order 123 changed to Paid
```

Then:

```csharp
order.ChangeStatus("Shipped");
```

produces:

```text
EMAIL -> Order 123: Paid -> Shipped
ANALYTICS -> Order 123 changed to Shipped
```

---

# 16. Unsubscribing from an Event

```csharp
order.StatusChanged -= emailObserver.OnStatusChanged;
```

This is equivalent to `Detach` in the manual Observer implementation.

The email subscriber will no longer receive future notifications.

---

# 17. Classic Observer vs C# Events

The relationship is very similar:

| Classic Observer | C# Events |
|---|---|
| `Attach(observer)` | `event += handler` |
| `Detach(observer)` | `event -= handler` |
| `Notify()` | `event?.Invoke(...)` |
| `IObserver.Update()` | Event handler method |
| `List<IObserver>` | Delegate invocation list |

Conceptually:

```text
Classic Observer

Subject
   |
   +--- Attach
   +--- Detach
   +--- Notify
            |
            +---- Observer A
            +---- Observer B
            +---- Observer C
```

With C#:

```text
Publisher
   |
   +--- event
          |
          +---- Handler A
          +---- Handler B
          +---- Handler C
```

The same one-to-many notification idea remains.

---

# 18. What Does `Invoke` Do?

Consider:

```csharp
order.StatusChanged += emailObserver.OnStatusChanged;
order.StatusChanged += analyticsObserver.OnStatusChanged;
```

Later:

```csharp
StatusChanged?.Invoke(this, eventArgs);
```

Conceptually, this results in:

```csharp
emailObserver.OnStatusChanged(this, eventArgs);

analyticsObserver.OnStatusChanged(this, eventArgs);
```

The event internally maintains an invocation list containing all subscribed handlers.

---

# 19. What Is `sender`?

When an event is raised:

```csharp
StatusChanged?.Invoke(this, eventArgs);
```

the first argument becomes:

```csharp
object? sender
```

inside the subscriber.

```text
Invoke(this, eventArgs)
       |       |
       |       +----> e
       |
       +------------> sender
```

A subscriber can therefore identify the object that raised the event.

```csharp
public void OnStatusChanged(
    object? sender,
    OrderStatusChangedEventArgs e)
{
    if (sender is Order order)
    {
        Console.WriteLine(
            $"Order instance {order.Id} raised the event.");
    }
}
```

---

# 20. Lambda Subscribers

A dedicated observer class is not always required.

We can subscribe using a lambda:

```csharp
order.StatusChanged += (_, e) =>
{
    Console.WriteLine(
        $"Dashboard -> Order {e.OrderId}: {e.NewStatus}");
};
```

This is useful for small handlers.

For larger responsibilities, separate classes are usually easier to test and maintain.

---

# 21. Why Observer Improves the Design

Without Observer:

```text
Order
 |
 +--- EmailService
 +--- SmsService
 +--- AnalyticsService
 +--- AuditService
 +--- NotificationService
```

The publisher knows all dependent components.

With Observer:

```text
              Order
                |
          StatusChanged
                |
     +----------+----------+
     |          |          |
    Email    Analytics    Audit
```

`Order` only publishes the change.

The interested components decide whether they want to subscribe.

This provides **looser coupling**.

---

# 22. Open/Closed Principle

Observer also works well with the **Open/Closed Principle**.

Suppose we want to introduce:

```csharp
SlackOrderObserver
```

We can simply subscribe it:

```csharp
order.StatusChanged += slackObserver.OnStatusChanged;
```

The `Order` class does not need to change.

The system can be extended with new reactions without modifying the publisher.

---

# 23. Push vs Pull Models

Observer implementations commonly use two approaches.

## Push Model

The subject sends the required information directly.

```csharp
observer.Update(
    oldStock,
    newStock);
```

The observer receives everything it needs.

---

## Pull Model

The subject sends itself or a reference to itself.

```csharp
observer.Update(this);
```

The observer retrieves the information it needs.

```csharp
public void Update(ProductStock product)
{
    Console.WriteLine(product.Stock);
}
```

Both approaches are valid.

The choice depends on how much information observers need and how strongly they should depend on the subject.

---

# 24. Real-World Examples

Observer appears in many systems.

## Product Availability

```text
Product Back In Stock
        |
        +---- Email customer
        +---- Send push notification
        +---- Update dashboard
```

## Order Status

```text
Order Shipped
     |
     +---- Notify customer
     +---- Track analytics
     +---- Update UI
```

## ATM Transaction

```text
Transaction Status Changed
          |
          +---- UI
          +---- Logger
          +---- Monitoring
          +---- Receipt Service
```

## Stock Market

```text
Price Changed
     |
     +---- Trader Dashboard
     +---- Alert Service
     +---- Mobile Application
```

## Weather Station

```text
Temperature Changed
          |
          +---- Mobile Display
          +---- Web Dashboard
          +---- Alert System
```

---

# 25. Observer and UI Applications

UI frameworks heavily rely on event-based communication.

For example:

```csharp
button.Click += Button_Click;
```

The button acts as the publisher.

The handler acts as the subscriber.

```text
Button
   |
 Click Event
   |
   +---- Button_Click
```

This follows the same general Observer idea.

---

# 26. Observer and Domain Events

The same idea appears in domain-driven applications.

For example:

```text
Order Created
      |
      +---- Send Email
      +---- Reserve Stock
      +---- Create Invoice
      +---- Add Loyalty Points
```

The order creation logic should not necessarily contain all these secondary responsibilities.

Instead, it can publish an event:

```csharp
public sealed record OrderCreatedEvent(
    int OrderId,
    string CustomerEmail,
    decimal TotalAmount);
```

Multiple handlers may react independently.

```text
OrderCreatedEvent
       |
       +---- SendOrderEmailHandler
       +---- ReserveStockHandler
       +---- CreateInvoiceHandler
```

This is conceptually related to Observer, although application-level domain event implementations may introduce additional infrastructure and design decisions.

---

# 27. MediatR Notifications

Libraries such as MediatR also provide one-to-many notification mechanisms.

```csharp
public sealed record OrderCreatedNotification(
    int OrderId,
    string CustomerEmail)
    : INotification;
```

Multiple handlers can subscribe to the same notification:

```csharp
public sealed class SendEmailHandler
    : INotificationHandler<OrderCreatedNotification>
{
    public Task Handle(
        OrderCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Email sent to {notification.CustomerEmail}");

        return Task.CompletedTask;
    }
}
```

Another handler:

```csharp
public sealed class AnalyticsHandler
    : INotificationHandler<OrderCreatedNotification>
{
    public Task Handle(
        OrderCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Analytics recorded for order {notification.OrderId}");

        return Task.CompletedTask;
    }
}
```

Publishing:

```csharp
await mediator.Publish(
    new OrderCreatedNotification(
        order.Id,
        order.CustomerEmail),
    cancellationToken);
```

The conceptual relationship is:

```text
One notification
       ↓
Multiple handlers
```

However, MediatR is infrastructure built around mediator/notification abstractions rather than the literal GoF Observer implementation shown earlier.

---

# 28. Observer vs Pub/Sub

Observer and Publish/Subscribe are related concepts, but they are not exactly the same.

## Observer

The subject usually holds references to its observers.

```text
Subject
 |
 +---- Observer A
 +---- Observer B
 +---- Observer C
```

They are typically inside the same process.

---

## Publish/Subscribe

A broker or event bus sits between publisher and subscriber.

```text
Publisher
    |
    v
Message Broker
    |
    +---- Subscriber A
    +---- Subscriber B
    +---- Subscriber C
```

Examples:

```text
RabbitMQ
Kafka
Azure Service Bus
AWS SNS/SQS
```

The publisher usually does not know the subscribers.

---

## Important Difference

```text
Observer
Publisher --------> Observer

Pub/Sub
Publisher --------> Broker --------> Subscriber
```

Pub/Sub introduces distributed-system concerns such as:

- retries
- eventual consistency
- duplicate delivery
- idempotency
- message ordering
- dead-letter queues
- serialization
- network failures
- Outbox Pattern

Therefore, a message broker should not simply be treated as the classic GoF Observer Pattern.

It is better to think of Pub/Sub as a related evolution of the same one-to-many notification idea in distributed systems.

---

# 29. Observer vs Mediator

These patterns solve different problems.

## Observer

One publisher notifies many listeners.

```text
Publisher
   |
   +---- Subscriber A
   +---- Subscriber B
   +---- Subscriber C
```

## Mediator

Multiple components communicate through a central mediator.

```text
Component A
     |
     v
  Mediator
     |
     v
Component B
```

Observer focuses on **notifications**.

Mediator focuses on **coordinating communication between components**.

---

# 30. Observer vs Decorator

Decorator adds behavior around an object.

```text
Logging
   ↓
Retry
   ↓
PaymentService
```

Observer reacts to an event.

```text
PaymentCompleted
       |
       +---- Email
       +---- Analytics
       +---- Audit
```

They solve completely different problems.

---

# 31. Synchronous Observers

The simplest Observer implementation is synchronous.

```csharp
foreach (var observer in observers)
{
    observer.Update(order);
}
```

If one observer takes five seconds, the publisher waits five seconds.

Also, if an observer throws an exception, notification of later observers may be interrupted depending on the implementation.

---

# 32. Asynchronous Observers

Observers can also execute asynchronously.

```csharp
await Task.WhenAll(
    observers.Select(
        observer =>
            observer.UpdateAsync(
                order,
                cancellationToken)));
```

This may improve throughput but introduces additional concerns:

- thread safety
- exception handling
- cancellation
- ordering
- transaction boundaries
- concurrent state changes

Asynchronous execution should therefore be an intentional design decision rather than an automatic optimization.

---

# 33. Advantages

The Observer Pattern provides several benefits:

- Loose coupling between publisher and subscribers
- One-to-many communication
- Dynamic subscription and unsubscription
- Easy addition of new observers
- Better separation of responsibilities
- Good support for the Open/Closed Principle
- Natural fit for event-driven behavior

---

# 34. Disadvantages

Observer also has trade-offs:

- Program flow can become harder to follow
- Notification order may matter unexpectedly
- Slow observers can delay synchronous publishers
- Exceptions in observers require careful handling
- Forgotten subscriptions may cause memory leaks
- Too many events can make the system difficult to understand
- Circular event chains can create unexpected behavior

---

# 35. Memory Leaks and Events

C# events create a reference from the publisher to the subscriber.

If a long-lived publisher contains a subscription to a short-lived subscriber, the subscriber may remain alive longer than expected.

For this reason:

```csharp
publisher.Event += subscriber.Handler;
```

may eventually require:

```csharp
publisher.Event -= subscriber.Handler;
```

depending on the lifetime of both objects.

Subscription lifetime is an important consideration in event-driven applications.

---

# 36. When Should We Use Observer?

Observer is a good choice when:

- one change should notify multiple components
- subscribers are not known in advance
- subscribers may be added or removed dynamically
- the publisher should not know concrete subscribers
- event-based communication naturally fits the domain
- UI elements need to react to state changes
- multiple independent reactions follow the same event

---

# 37. When Should We Avoid Observer?

Observer may not be appropriate when:

- there is only one mandatory operation
- execution order must be strictly controlled
- all operations must participate in the same transaction
- failures must cause all operations to roll back together
- events would make a simple workflow unnecessarily difficult to understand

For example:

```text
Withdraw Money
     |
     +---- Decrease Balance
     +---- Create Accounting Entry
```

If both actions must succeed or fail together inside the same transaction, separating them into loosely coordinated observers may be dangerous.

Observer should improve separation of concerns, not hide important business dependencies.

---

# 38. Mental Model

The easiest way to remember the pattern is:

```text
        Something changes
               |
               v
            Subject
               |
            Notify
               |
     +---------+---------+
     |         |         |
     v         v         v
 Observer   Observer   Observer
```

Or simply:

> **One event occurs, multiple interested parties react.**

---

# 39. Classic Observer vs Event-Based Observer

## Classic

```csharp
subject.Attach(observer);

subject.Notify();

subject.Detach(observer);
```

## C#

```csharp
publisher.StatusChanged += observer.OnStatusChanged;

publisher.StatusChanged?.Invoke(this, eventArgs);

publisher.StatusChanged -= observer.OnStatusChanged;
```

Same fundamental idea, different implementation mechanism.

---

# 40. Interview Answer

A concise explanation:

> The Observer Pattern is a behavioral design pattern that defines a one-to-many relationship between objects. When the state of the subject changes, all subscribed observers are automatically notified. It reduces coupling because the publisher does not need to know the concrete implementations of its subscribers. In C#, the pattern is naturally represented with delegates and events using `+=`, `-=`, and `Invoke`.

A slightly more advanced answer:

> The classic implementation maintains a collection of observers and exposes subscribe, unsubscribe, and notify operations. C# events provide language-level support for a similar mechanism. Observer is also conceptually related to domain events and event-driven architectures, although distributed Pub/Sub systems introduce brokers, network boundaries, delivery guarantees, retries, idempotency, and other concerns that are outside the classic GoF pattern.

---

# 41. Key Takeaway

The Observer Pattern is not primarily about sending emails, updating dashboards, or raising events.

It is about removing this dependency:

```text
Publisher
   |
   +---- knows Observer A
   +---- knows Observer B
   +---- knows Observer C
```

and moving toward:

```text
Publisher
   |
   +---- publishes a change
              |
              +---- interested subscribers react
```

The publisher focuses on **what happened**.

Observers decide **how they react to it**.

That separation is the real value of the Observer Pattern.
