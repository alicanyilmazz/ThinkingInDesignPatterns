# DesignPatterns

---

```diff
@@ State Pattern @@

If you understand the State Pattern, these concepts become much easier to understand:

ATM transaction flows
Order status flows
Workflow Engines
State Machines
MassTransit Saga
TCP Connections
Document Approval Flows
Music Players
Vending Machines

State Pattern basically means:

Change the behavior of an object according to its current state.
```

---

```diff
@@ First, let's understand the problem @@
```

Imagine an order.

When it is first created:

```text
Pending
```

Then the payment is completed:

```text
Paid
```

Then it is shipped:

```text
Shipped
```

Finally, it is delivered:

```text
Completed
```

We could write something like this:

```csharp
public class Order
{
    public OrderStatus Status { get; set; }

    public void Cancel()
    {
        if (Status == OrderStatus.Pending)
        {
            Console.WriteLine("Order cancelled.");
        }

        if (Status == OrderStatus.Paid)
        {
            Console.WriteLine("Start refund.");
        }

        if (Status == OrderStatus.Shipped)
        {
            Console.WriteLine("Order cannot be cancelled.");
        }

        if (Status == OrderStatus.Completed)
        {
            Console.WriteLine("Order has already been completed.");
        }
    }
}
```

```diff
@@ This works today. But tomorrow... @@

Returned

Refunded

Rejected

Cancelled

Preparing

PaymentFailed

states may be added.

Now our if/switch blocks keep growing.
```

The problem becomes even worse when we add more operations:

```text
Pay()
Ship()
Cancel()
Refund()
Return()
Complete()
```

Then every method starts checking the current state.

```text
Many if/switch statements
        ↓
Complex business rules
        ↓
Hard-to-maintain code
        ↓
Harder to add new states
```

---

```diff
@@ What does the State Pattern say? @@

Represent each state as a separate class.
```

For example:

```text
PendingState

PaidState

ShippedState

CompletedState
```

```diff
@@ Every state implements the same interface. @@
```

---

```diff
@@ State Interface @@
```

```csharp
public interface IOrderState
{
    void Pay(OrderContext context);

    void Ship(OrderContext context);

    void Cancel(OrderContext context);
}
```

The interface defines the operations that can behave differently depending on the current state.

---

```diff
@@ Context @@

The Context is one of the most important parts of the State Pattern.
```

```csharp
public class OrderContext
{
    public IOrderState State { get; set; }

    public OrderContext(IOrderState state)
    {
        State = state;
    }

    public void Pay()
    {
        State.Pay(this);
    }

    public void Ship()
    {
        State.Ship(this);
    }

    public void Cancel()
    {
        State.Cancel(this);
    }
}
```

```diff
@@ Important @@

The Context knows WHICH state it is currently in.

But it does not know HOW that state should behave.

The behavior is delegated to the current State object.
```

For example:

```csharp
public void Cancel()
{
    State.Cancel(this);
}
```

The `OrderContext` does not contain:

```csharp
if (State is PendingState)
{
}

else if (State is PaidState)
{
}

else if (State is ShippedState)
{
}
```

Instead:

```csharp
State.Cancel(this);
```

The current state decides what should happen.

---

```diff
@@ Pending State @@
```

```csharp
public class PendingState : IOrderState
{
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Payment received.");

        context.State = new PaidState();
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Payment has not been received yet.");
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Order cancelled.");
    }
}
```

```diff
@@ Most important line @@
```

```csharp
context.State = new PaidState();
```

```diff
@@ The object's state has changed. @@
```

The object is still the same `OrderContext`.

But its behavior has changed because its current state has changed.

---

```diff
@@ Paid State @@
```

```csharp
public class PaidState : IOrderState
{
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Order has already been paid.");
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Order shipped.");

        context.State = new ShippedState();
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Refund process started.");
    }
}
```

---

```diff
@@ Shipped State @@
```

```csharp
public class ShippedState : IOrderState
{
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Order has already been paid.");
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Order has already been shipped.");
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Order cannot be cancelled after shipping.");
    }
}
```

---

```diff
@@ Usage @@
```

```csharp
var order = new OrderContext(new PendingState());

order.Pay();

order.Ship();

order.Cancel();
```

The execution flow is:

```text
Pending
   ↓
 Pay()
   ↓
 Paid
   ↓
 Ship()
   ↓
Shipped
   ↓
Cancel()
   ↓
Order cannot be cancelled.
```

```diff
@@ Notice something important @@

We always call the same method:

Cancel()

But the result may be completely different.

Why?

Because the current State is different.
```

For example:

```text
PendingState.Cancel()

→ Cancel the order
```

```text
PaidState.Cancel()

→ Start refund
```

```text
ShippedState.Cancel()

→ Cancellation is not allowed
```

Same method.

Different behavior.

Different state.

That is the core idea of the State Pattern.

---

# Music Player Example

```diff
@@ Another simple State Pattern example @@
```

Think about a Spotify-like music player.

It can have these states:

```text
Stopped

Playing

Paused
```

And the player exposes these operations:

```text
Play()

Pause()

Stop()
```

The important point is that the same operation behaves differently depending on the current state.

---

```diff
@@ Music Player States @@
```

```text
Stopped
   ↓ Play()
Playing
   ↓ Pause()
Paused
   ↓ Play()
Playing
   ↓ Stop()
Stopped
```

The State interface:

```csharp
public interface IPlayerState
{
    void Play();

    void Pause();

    void Stop();
}
```

The Context:

```csharp
public class MusicPlayer
{
    private IPlayerState _state;

    public MusicPlayer()
    {
        _state = new StoppedState(this);
    }

    public void ChangeState(IPlayerState state)
    {
        _state = state;
    }

    public void Play()
    {
        _state.Play();
    }

    public void Pause()
    {
        _state.Pause();
    }

    public void Stop()
    {
        _state.Stop();
    }
}
```

```diff
@@ Stopped State @@
```

```csharp
public class StoppedState : IPlayerState
{
    private readonly MusicPlayer _player;

    public StoppedState(MusicPlayer player)
    {
        _player = player;
    }

    public void Play()
    {
        Console.WriteLine("Music started.");

        _player.ChangeState(
            new PlayingState(_player));
    }

    public void Pause()
    {
        Console.WriteLine(
            "Cannot pause because the music is already stopped.");
    }

    public void Stop()
    {
        Console.WriteLine(
            "Music is already stopped.");
    }
}
```

```diff
@@ Playing State @@
```

```csharp
public class PlayingState : IPlayerState
{
    private readonly MusicPlayer _player;

    public PlayingState(MusicPlayer player)
    {
        _player = player;
    }

    public void Play()
    {
        Console.WriteLine(
            "Music is already playing.");
    }

    public void Pause()
    {
        Console.WriteLine(
            "Music paused.");

        _player.ChangeState(
            new PausedState(_player));
    }

    public void Stop()
    {
        Console.WriteLine(
            "Music stopped.");

        _player.ChangeState(
            new StoppedState(_player));
    }
}
```

```diff
@@ Paused State @@
```

```csharp
public class PausedState : IPlayerState
{
    private readonly MusicPlayer _player;

    public PausedState(MusicPlayer player)
    {
        _player = player;
    }

    public void Play()
    {
        Console.WriteLine(
            "Music resumed.");

        _player.ChangeState(
            new PlayingState(_player));
    }

    public void Pause()
    {
        Console.WriteLine(
            "Music is already paused.");
    }

    public void Stop()
    {
        Console.WriteLine(
            "Music stopped.");

        _player.ChangeState(
            new StoppedState(_player));
    }
}
```

Usage:

```csharp
var spotify = new MusicPlayer();

spotify.Play();

spotify.Pause();

spotify.Play();

spotify.Stop();
```

Execution:

```text
Stopped
   ↓
Play()
   ↓
Playing
   ↓
Pause()
   ↓
Paused
   ↓
Play()
   ↓
Playing
   ↓
Stop()
   ↓
Stopped
```

```diff
@@ The important point @@
```

We always call:

```csharp
spotify.Play();
```

But `Play()` means different things depending on the current state.

When the current state is `StoppedState`:

```text
Start playing music.
```

When the current state is `PausedState`:

```text
Resume the music.
```

When the current state is `PlayingState`:

```text
The music is already playing.
```

Same operation.

Different behavior.

Because the state is different.

---

# Vending Machine Example

```diff
@@ A more realistic State Pattern example @@
```

Imagine a vending machine.

The machine may have these states:

```text
NoMoneyState

HasMoneyState

ProductSelectedState

DispensingState
```

And it supports these operations:

```text
InsertMoney()

SelectProduct()

Dispense()

Cancel()
```

The behavior of these operations changes according to the current state.

---

```diff
@@ Example flow @@
```

```text
NoMoneyState
      |
      | InsertMoney()
      ↓
HasMoneyState
      |
      | SelectProduct()
      ↓
ProductSelectedState
      |
      | Dispense()
      ↓
DispensingState
      |
      | Product delivered
      ↓
NoMoneyState
```

The State interface:

```csharp
public interface IVendingMachineState
{
    void InsertMoney(decimal amount);

    void SelectProduct(string productName);

    void Dispense();

    void Cancel();
}
```

The Context:

```csharp
public class VendingMachine
{
    private IVendingMachineState _state;

    public decimal Balance { get; private set; }

    public string SelectedProduct { get; private set; }

    public decimal SelectedProductPrice { get; private set; }

    public VendingMachine()
    {
        _state = new NoMoneyState(this);
    }

    public void ChangeState(IVendingMachineState state)
    {
        _state = state;
    }

    public void InsertMoney(decimal amount)
    {
        _state.InsertMoney(amount);
    }

    public void SelectProduct(string productName)
    {
        _state.SelectProduct(productName);
    }

    public void Dispense()
    {
        _state.Dispense();
    }

    public void Cancel()
    {
        _state.Cancel();
    }
}
```

Look at this method:

```csharp
public void SelectProduct(string productName)
{
    _state.SelectProduct(productName);
}
```

There is no:

```csharp
if (_state is NoMoneyState)
{
}

else if (_state is HasMoneyState)
{
}

else if (_state is ProductSelectedState)
{
}
```

The machine simply delegates the operation:

```csharp
_state.SelectProduct(productName);
```

---

```diff
@@ Same method, different state @@
```

Suppose we call:

```csharp
machine.SelectProduct("Cola");
```

When the machine is in:

```text
NoMoneyState
```

the result can be:

```text
Please insert money first.
```

When the machine is in:

```text
HasMoneyState
```

the result can be:

```text
Cola selected.
```

When the machine is in:

```text
ProductSelectedState
```

the result can be:

```text
A product has already been selected.
```

Again:

```text
Same method
+
Different State
=
Different Behavior
```

That is exactly what the State Pattern is designed for.

---

# TCP Connection Example

```diff
@@ TCP Connection @@

A classic example of the State Pattern.
```

A connection can move through states such as:

```text
Closed
   ↓
Listening
   ↓
Established
   ↓
Closing
   ↓
Closed
```

The same operation:

```csharp
Send();
```

may behave differently depending on the connection state.

For example:

```text
ClosedState.Send()

→ Cannot send data.
```

```text
EstablishedState.Send()

→ Send the data.
```

Again, the behavior depends on the current state.

---

# ATM Example

State Pattern is also very useful for ATM transaction flows.

An ATM session may look like this:

```text
Idle
   ↓ InsertCard()
CardInserted
   ↓ EnterPin()
Authenticated
   ↓ Withdraw()
Transaction
   ↓ EjectCard()
Idle
```

The same operation may behave differently depending on the state.

For example:

```csharp
atm.Withdraw();
```

When the ATM is in:

```text
IdleState
```

the result may be:

```text
Please insert your card first.
```

When the ATM is in:

```text
CardInsertedState
```

the result may be:

```text
Please enter your PIN first.
```

When the ATM is in:

```text
AuthenticatedState
```

the result may be:

```text
Withdrawal transaction started.
```

This is a very common real-world use case for State Machines.

---

# State Pattern and State Machines

```diff
@@ State Pattern is closely related to State Machines @@
```

A State Machine usually contains:

```text
States

Events

Transitions
```

For example:

```text
Current State:
Pending

Event:
Pay

Next State:
Paid
```

or:

```text
Current State:
Paid

Event:
Ship

Next State:
Shipped
```

We can represent this as:

```text
Pending --Pay--> Paid

Paid --Ship--> Shipped

Shipped --Deliver--> Completed
```

State Pattern gives us an object-oriented way of implementing this behavior.

---

# MassTransit Saga

```diff
@@ MassTransit Saga @@

Saga State Machines are strongly related to the State Pattern.
```

For example, an order saga may contain:

```text
Submitted

PaymentWaiting

PaymentCompleted

Preparing

Shipped

Completed

Failed
```

Events cause transitions between states:

```text
OrderSubmitted
       ↓
Submitted

PaymentCompleted
       ↓
PaymentCompletedState

OrderShipped
       ↓
Shipped

OrderDelivered
       ↓
Completed
```

This is why understanding the State Pattern makes concepts such as:

```text
Saga

State Machine

Automatonymous

MassTransit

Workflow Engine
```

much easier to understand.

---

# Advantages

```text
✅ Reduces large if/switch blocks.

✅ Each state has its own class.

✅ State-specific behavior is isolated.

✅ Makes state transitions explicit.

✅ Makes complex workflows easier to understand.

✅ New states can be added more easily.

✅ Improves readability.

✅ Follows the Single Responsibility Principle.

✅ Works very well for workflow/state-machine-based systems.
```

---

# Disadvantages

```text
❌ The number of classes can increase.

❌ A system with many states may contain many State classes.

❌ Very simple state logic may become unnecessarily complicated.

❌ State transitions can become difficult to understand if they are poorly designed.
```

For example:

```text
20 States

×

10 Operations

=

Potentially complex state behavior
```

So State Pattern should be used when the object's behavior genuinely depends on its state.

---

# When Should We Use State Pattern?

Use State Pattern when:

```text
The object's behavior changes according to its current state.

There are many if/switch statements checking status.

The system has clear state transitions.

Different operations are allowed in different states.

You are implementing workflows or state machines.
```

Good examples:

```text
Order Workflow

ATM Session

Music Player

Vending Machine

TCP Connection

Document Approval

Download Manager

Payment Workflow

Authentication Session

Saga State Machine
```

---

# State vs Strategy

```diff
@@ One of the most common interview questions @@
```

State and Strategy may look similar because both usually contain:

```text
Interface

Concrete implementations

Context
```

But their intention is different.

---

## Strategy

Strategy is about:

```text
Which algorithm should I use?
```

Example:

```text
VisaPaymentStrategy

MasterCardPaymentStrategy

TroyPaymentStrategy
```

The strategy is usually selected from outside:

```csharp
paymentService.SetStrategy(
    new VisaPaymentStrategy());
```

So:

```text
Strategy
=
Different algorithms
```

---

## State

State is about:

```text
What state am I currently in,
and how should I behave in this state?
```

Example:

```text
PendingState

PaidState

ShippedState
```

State transitions can happen internally:

```csharp
context.State = new PaidState();
```

So:

```text
State
=
Different behavior according to current state
```

---

```diff
@@ Easy way to remember @@

Strategy:

WHO decides the algorithm?

Usually the client / outside code.


State:

WHO decides the behavior?

The current state of the object.
```

Another simple rule:

```text
Strategy
→ Usually selected from outside.

State
→ Usually changes during the object's lifecycle.
```

Example:

```text
Strategy

Visa
MasterCard
Troy
```

versus:

```text
State

Pending
   ↓
Paid
   ↓
Shipped
```

---

# State vs Factory

```text
Factory
→ Which object should be created?

State
→ How should the object behave in its current state?
```

Easy question:

```text
Factory:
Which object?

State:
Which state?
```

---

# State vs Command

```text
Command
→ What operation should be executed?

State
→ How should the object behave in its current state?
```

Easy question:

```text
Command:
What should be done?

State:
What state am I currently in?
```

---

# State vs Observer

```text
Observer
→ Something happened. Notify interested objects.

State
→ The state changed. Therefore the behavior changed.
```

Easy question:

```text
Observer:
Who needs to know?

State:
How should I behave now?
```

---

# State Pattern Summary

```diff
@@ Remember this @@
```

```text
State Pattern allows an object to change its behavior
when its internal state changes.
```

The client continues using the same object:

```csharp
player.Play();

machine.SelectProduct("Cola");

order.Cancel();
```

But the behavior changes according to:

```text
Current State
```

The basic structure is:

```text
Context
   |
   ↓
Current State
   |
   ↓
Behavior
   |
   ↓
Possible State Transition
```

Or even simpler:

```text
State changes
      ↓
Behavior changes
```

---

# Interview Question

> **What is the State Pattern?**

The State Pattern is a behavioral design pattern that allows an object to change its behavior depending on its internal state. Each state is usually represented by a separate class, and the Context delegates its operations to the current State object.

---

> **What is the difference between State and Strategy?**

Strategy is mainly used to choose between different algorithms, and the strategy is usually selected externally. State Pattern changes an object's behavior according to its internal state, and state objects can transition from one state to another during the object's lifecycle.

---

> **What is the biggest advantage of the State Pattern?**

It removes complex state-based `if/else` and `switch` logic from the Context and moves state-specific behavior into separate classes.

---

> **Where can the State Pattern be used?**

It can be used in order workflows, ATM sessions, TCP connections, music players, vending machines, document approval systems, workflow engines, and Saga State Machines.

---

# Questions

```text
1)

What is the main purpose of the State Pattern?

A) Creating objects

B) Changing behavior according to the current state

C) Selecting an algorithm

D) Converting one interface into another
```

```text
2)

An order follows this flow:

Pending
   ↓
Paid
   ↓
Shipped

Which pattern is most closely related to this behavior?

A) Strategy

B) State

C) Builder

D) Adapter
```

```text
3)

Which of the following is a suitable use case for the State Pattern?

A) TCP Connection

B) ATM transaction flow

C) Order workflow

D) Music Player

E) Vending Machine

F) All of them
```

```text
4)

What is the main difference between Strategy and State?

A) They are exactly the same.

B) Strategy usually selects an algorithm externally, while State changes behavior according to the object's current state.

C) State creates objects.

D) Strategy publishes events.
```

```text
5)

Which pattern is strongly related to MassTransit Saga State Machines?

A) Observer

B) Factory

C) State

D) Decorator
```

```text
6)

A Music Player is currently in PausedState.

Play() is called.

What should normally happen?

A) Nothing can ever happen.

B) The player should transition to PlayingState.

C) A new MusicPlayer should be created.

D) The Strategy should change.
```

```text
7)

A Vending Machine is in NoMoneyState.

SelectProduct("Cola") is called.

What is the best State Pattern behavior?

A) Dispense the product.

B) Create another machine.

C) Reject the operation and ask the user to insert money.

D) Change the payment strategy.
```

```text
Correct Answers

1) B

2) B

3) F

4) B

5) C

6) B

7) C
```

---

# Final Rule

```diff
@@ If you remember only one thing, remember this @@
```

```text
Strategy:

Different algorithm.


State:

Different behavior because the current state is different.
```

And the simplest definition:

> **State Pattern allows an object to change its behavior when its internal state changes.**
