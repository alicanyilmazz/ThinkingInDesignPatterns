# Mediator Design Pattern

The **Mediator Pattern** is a behavioral design pattern that reduces direct dependencies between objects by introducing a central object responsible for coordinating their interactions.

Instead of multiple objects communicating directly with each other, they communicate through a **Mediator**.

The main idea is simple:

> Objects should not need to know how other objects work. They only need to know how to communicate with the mediator.

---

## The Problem

Imagine a system containing several components:

```text
Component A
Component B
Component C
Component D
```

Without a mediator, the components may communicate directly with each other:

```text
A ───────> B
A ───────> C
B ───────> C
B ───────> D
C ───────> A
D ───────> B
```

As the system grows, more dependencies appear.

A component may need references to several other components:

```csharp
public class ComponentA
{
    private readonly ComponentB _componentB;
    private readonly ComponentC _componentC;
    private readonly ComponentD _componentD;
}
```

This creates **tight coupling**.

Changing one component can affect several others, and understanding the communication flow becomes increasingly difficult.

---

# The Solution

The Mediator Pattern introduces a central communication point.

```text
              Mediator
            /    |    \
           /     |     \
          A      B      C
                  \
                   D
```

Now the components do not communicate directly.

Instead:

```text
Component
    |
    | Notify
    v
Mediator
    |
    | Coordinate
    v
Other Components
```

A component simply says:

```text
"Something happened."
```

The mediator decides:

```text
"What should happen next?"
```

This is the key idea behind the pattern.

---

# Structure

The Mediator Pattern generally contains the following participants:

### Mediator

Defines how components communicate.

```csharp
public interface IMediator
{
    void Notify(object sender, string eventName);
}
```

### Concrete Mediator

Knows the participating components and contains the coordination rules.

```csharp
public class ConcreteMediator : IMediator
{
    public void Notify(object sender, string eventName)
    {
        // Coordinate components
    }
}
```

### Colleague / Component

A component does not communicate directly with another component.

It communicates through the mediator.

```csharp
public abstract class Component
{
    protected IMediator Mediator { get; }

    protected Component(IMediator mediator)
    {
        Mediator = mediator;
    }
}
```

---

# Example 1 — Air Traffic Control

Air Traffic Control is a natural example of the Mediator Pattern.

Imagine several aircraft approaching the same airport.

Without a control tower, aircraft would need to communicate directly with each other:

```text
Plane A ─────> Plane B
Plane A ─────> Plane C
Plane B ─────> Plane C
Plane C ─────> Plane A
```

Each aircraft would need to know:

* Which aircraft is landing
* Which aircraft is taking off
* Whether the runway is available
* Which aircraft should wait
* Which aircraft has priority

This would create unnecessary coupling between aircraft.

Instead, aircraft communicate with the **Control Tower**.

```text
                Control Tower
               /      |      \
              /       |       \
        Plane A    Plane B    Plane C
```

The tower becomes the mediator.

---

## Mediator Interface

```csharp
public interface IAirTrafficControlMediator
{
    void RequestLanding(Aircraft aircraft);

    void RequestTakeOff(Aircraft aircraft);
}
```

---

## Aircraft

```csharp
public abstract class Aircraft
{
    protected readonly IAirTrafficControlMediator Mediator;

    public string FlightCode { get; }

    protected Aircraft(
        string flightCode,
        IAirTrafficControlMediator mediator)
    {
        FlightCode = flightCode;
        Mediator = mediator;
    }

    public abstract void RequestLanding();

    public abstract void RequestTakeOff();

    public virtual void Land()
    {
        Console.WriteLine($"{FlightCode} is landing.");
    }

    public virtual void TakeOff()
    {
        Console.WriteLine($"{FlightCode} is taking off.");
    }

    public virtual void Wait()
    {
        Console.WriteLine($"{FlightCode} is waiting.");
    }
}
```

Notice that `Aircraft` does not know about other aircraft.

It only knows:

```csharp
IAirTrafficControlMediator
```

---

## Passenger Plane

```csharp
public class PassengerPlane : Aircraft
{
    public PassengerPlane(
        string flightCode,
        IAirTrafficControlMediator mediator)
        : base(flightCode, mediator)
    {
    }

    public override void RequestLanding()
    {
        Console.WriteLine(
            $"{FlightCode} requests permission to land.");

        Mediator.RequestLanding(this);
    }

    public override void RequestTakeOff()
    {
        Console.WriteLine(
            $"{FlightCode} requests permission to take off.");

        Mediator.RequestTakeOff(this);
    }
}
```

---

## Cargo Plane

```csharp
public class CargoPlane : Aircraft
{
    public CargoPlane(
        string flightCode,
        IAirTrafficControlMediator mediator)
        : base(flightCode, mediator)
    {
    }

    public override void RequestLanding()
    {
        Console.WriteLine(
            $"{FlightCode} requests permission to land.");

        Mediator.RequestLanding(this);
    }

    public override void RequestTakeOff()
    {
        Console.WriteLine(
            $"{FlightCode} requests permission to take off.");

        Mediator.RequestTakeOff(this);
    }
}
```

---

## Control Tower

```csharp
public class ControlTowerMediator
    : IAirTrafficControlMediator
{
    private bool _runwayAvailable = true;

    public void RequestLanding(Aircraft aircraft)
    {
        if (!_runwayAvailable)
        {
            Console.WriteLine(
                $"Control Tower: Runway is busy. " +
                $"{aircraft.FlightCode} must wait.");

            aircraft.Wait();
            return;
        }

        Console.WriteLine(
            $"Control Tower: {aircraft.FlightCode} " +
            $"is cleared to land.");

        _runwayAvailable = false;

        aircraft.Land();

        _runwayAvailable = true;
    }

    public void RequestTakeOff(Aircraft aircraft)
    {
        if (!_runwayAvailable)
        {
            Console.WriteLine(
                $"Control Tower: Runway is busy. " +
                $"{aircraft.FlightCode} must wait.");

            aircraft.Wait();
            return;
        }

        Console.WriteLine(
            $"Control Tower: {aircraft.FlightCode} " +
            $"is cleared for takeoff.");

        _runwayAvailable = false;

        aircraft.TakeOff();

        _runwayAvailable = true;
    }
}
```

The aircraft does not decide whether it is safe to land.

The **Control Tower coordinates the operation**.

```text
Aircraft
   |
   | RequestLanding()
   v
ControlTower
   |
   | Check runway
   |
   +---- Available ----> Land
   |
   +---- Busy ---------> Wait
```

This is the essence of Mediator.

---

# Example 2 — Smart Home

A Smart Home system can contain many devices:

```text
MotionSensor
DoorLock
Alarm
Light
Thermostat
```

Without a mediator, a sensor could start depending directly on several other devices:

```text
MotionSensor
    |
    +----> Alarm
    |
    +----> Light
    |
    +----> DoorLock
```

For example:

```csharp
public void DetectMotion()
{
    alarm.Trigger();
    light.TurnOn();
}
```

Now `MotionSensor` knows about `Alarm` and `Light`.

If more behavior is added:

```text
MotionSensor
    |
    +----> Alarm
    +----> Light
    +----> Camera
    +----> DoorLock
    +----> NotificationService
```

the component becomes increasingly coupled to the rest of the system.

---

## With Mediator

Instead:

```text
MotionSensor
      |
      | MotionDetected
      v
SmartHomeMediator
      |
      +----> Alarm
      +----> Light
      +----> Thermostat
      +----> DoorLock
```

The sensor simply reports what happened.

The mediator decides what the system should do.

```csharp
Mediator.Notify(
    this,
    SmartHomeEvent.MotionDetected);
```

The sensor does not need to know:

```text
Is Away Mode active?

Should the alarm be triggered?

Should the lights be turned on?

Should the doors be locked?
```

That coordination belongs to the mediator.

---

## Example

```csharp
public enum SmartHomeEvent
{
    MotionDetected,
    DoorOpened
}
```

```csharp
public interface ISmartHomeMediator
{
    void ActivateAwayMode();

    void ActivateHomeMode();

    void Notify(
        object sender,
        SmartHomeEvent eventType);
}
```

A motion sensor can then simply notify the mediator:

```csharp
public class MotionSensor : SmartHomeDevice
{
    public MotionSensor(ISmartHomeMediator mediator)
        : base(mediator)
    {
    }

    public void DetectMotion()
    {
        Console.WriteLine("Motion detected.");

        Mediator.Notify(
            this,
            SmartHomeEvent.MotionDetected);
    }
}
```

The mediator contains the coordination rule:

```csharp
private void HandleMotionDetected()
{
    if (_awayMode)
    {
        _alarm?.Trigger();
        _light?.TurnOn();
    }
    else
    {
        _light?.TurnOn();
    }
}
```

The important part is that `MotionSensor` does not know what should happen after motion is detected.

It only reports the event.

---

# Mediator vs Observer

Mediator and Observer can look very similar because both patterns reduce direct dependencies between objects.

However, their **intent is different**.

A useful way to remember the difference is:

> **Observer = Notification**

> **Mediator = Coordination**

---

## Observer

Observer is mainly about broadcasting a state change.

```text
                Order
                  |
           StatusChanged
          /       |       \
         v        v        v
      Email    Analytics   SMS
```

The subject says:

```text
"My state changed."
```

Each observer independently decides what to do.

For example:

```csharp
order.StatusChanged += email.OnStatusChanged;
order.StatusChanged += analytics.OnStatusChanged;
order.StatusChanged += notification.OnStatusChanged;
```

When the event occurs:

```csharp
StatusChanged?.Invoke(this, args);
```

the publisher does not coordinate the observers.

Each observer contains its own reaction logic.

```text
EmailObserver
      |
      +----> Send Email

AnalyticsObserver
      |
      +----> Record Analytics

NotificationObserver
      |
      +----> Send Notification
```

The observers are independent.

---

# Mediator

Mediator has a different responsibility.

A component reports something to the mediator:

```text
MotionSensor
      |
      | MotionDetected
      v
SmartHomeMediator
```

The mediator then examines the current situation and decides what should happen.

```text
MotionDetected
       |
       v
SmartHomeMediator
       |
       +---- Is Away Mode enabled?
                  |
             YES  |  NO
              |       |
              v       v
            Alarm    Light
            Light
```

The important difference is that the **decision and coordination logic lives in the mediator**.

---

# Observer vs Mediator — Side by Side

| Observer                                  | Mediator                                             |
| ----------------------------------------- | ---------------------------------------------------- |
| Focuses on notification                   | Focuses on coordination                              |
| Usually one publisher → many subscribers  | Usually many components → one coordinator            |
| Observers decide how to react             | Mediator decides how components should interact      |
| Reaction logic is distributed             | Coordination logic is centralized                    |
| Publisher does not know what observers do | Mediator knows how participating components interact |
| Great for events and state changes        | Great for complex interaction rules                  |

---

## Another Way to Think About It

### Observer

Imagine someone announces:

> "The meeting time has changed."

Everyone who is interested hears the announcement and independently decides what to do.

```text
Announcement
     |
     +----> Ali updates calendar
     |
     +----> Ayşe sends an email
     |
     +----> Mehmet updates the meeting room
```

There is no central coordination.

---

### Mediator

Now imagine a coordinator says:

> "Ali, update the calendar.
> Ayşe, notify the participants.
> Mehmet, prepare the meeting room."

The coordinator decides who should do what.

That is Mediator.

---

# Distributed vs Centralized Intelligence

This is one of the most useful ways to distinguish the patterns.

### Observer

The intelligence is distributed.

```text
Event
 |
 +----> Observer A decides what to do
 |
 +----> Observer B decides what to do
 |
 +----> Observer C decides what to do
```

### Mediator

The intelligence is centralized.

```text
Event
 |
 v
Mediator
 |
 +----> Decide A should run
 |
 +----> Decide B should run
 |
 +----> Decide C should not run
```

This distinction becomes especially important when behavior depends on the state of multiple components.

---

# Can Mediator and Observer Be Used Together?

Yes.

They solve different problems and can complement each other.

For example, a component may publish an event:

```text
MotionSensor
      |
      | event
      v
Mediator
```

The mediator receives that notification and coordinates other components:

```text
Mediator
   |
   +----> Alarm
   +----> Light
   +----> DoorLock
```

In this architecture:

* **Observer/Event** can be used for notification.
* **Mediator** can be used for coordination.

---

# Advantages

## Reduced Coupling

Components do not need references to every other component.

Instead of:

```text
A → B
A → C
A → D
B → C
B → D
C → D
```

we get:

```text
A ──┐
B ──┤
C ──┼──> Mediator
D ──┘
```

---

## Centralized Communication Logic

Complex interaction rules are located in one place.

For example:

```text
Motion detected
+
Away mode enabled
+
Door locked
=
Trigger alarm
```

The rule belongs to the mediator instead of being scattered across several components.

---

## Easier Component Reuse

A `MotionSensor` should detect motion.

It should not need to understand:

* alarms,
* lights,
* security modes,
* cameras,
* notifications.

This keeps the component focused on its own responsibility.

---

## Easier Maintenance

Interaction rules can often be changed without modifying the participating components.

For example, changing:

```text
Motion detected in Away Mode
    ↓
Alarm + Light
```

to:

```text
Motion detected in Away Mode
    ↓
Alarm + Light + Camera
```

can be handled inside the mediator.

---

# Disadvantages

Mediator does not remove complexity.

It **moves interaction complexity into one place**.

As the system grows, the mediator itself can become too large.

For example:

```text
SmartHomeMediator
    |
    +---- Motion rules
    +---- Door rules
    +---- Fire rules
    +---- Temperature rules
    +---- Security rules
    +---- Camera rules
    +---- Notification rules
    +---- Energy rules
```

Eventually, the mediator may become a **God Object**.

When this happens, it can be better to divide responsibilities between smaller, focused mediators.

For example:

```text
SecurityMediator
ClimateMediator
LightingMediator
```

instead of one huge:

```text
SmartHomeMediator
```

---

# When Should You Use Mediator?

Mediator is useful when:

* Many objects communicate with each other.
* Objects have too many references to other objects.
* Interaction rules are becoming difficult to understand.
* Components should be reusable independently.
* Communication logic should be centralized.
* The behavior of one component depends on the state of several others.

A common warning sign is code like:

```csharp
class ComponentA
{
    ComponentB componentB;
    ComponentC componentC;
    ComponentD componentD;
    ComponentE componentE;
}
```

especially when the other components contain similar dependencies.

---

# When Should You Not Use Mediator?

Do not introduce a mediator simply because two objects communicate.

For example:

```text
OrderService → OrderRepository
```

does not automatically need a mediator.

If the interaction is already simple and clear, introducing another abstraction may only add unnecessary complexity.

Mediator becomes valuable when **coordination between multiple objects is the actual problem**.

---

# Mediator Pattern vs MediatR

These names are commonly confused.

They are related, but they are not the same thing.

### Mediator

**Mediator** is the behavioral design pattern described in this repository.

```text
Component
    ↓
Mediator
    ↓
Other Components
```

It is a design concept and does not depend on any specific framework or library.

---

### MediatR

**MediatR** is a popular .NET library based on the mediator idea.

It is commonly used to dispatch requests to handlers.

```text
Controller
    |
    v
 MediatR
    |
    v
Command / Query Handler
```

For example:

```csharp
await mediator.Send(
    new CreateOrderCommand());
```

may be routed to:

```text
CreateOrderCommand
        |
        v
CreateOrderCommandHandler
```

MediatR is often seen together with architectures such as:

```text
CQRS
Clean Architecture
Vertical Slice Architecture
Microservices
```

But **MediatR is a library**, while **Mediator is the design pattern**.

---

# Mediator in Microservices

Mediator should also not be confused with communication between microservices.

Inside a service, you might have:

```text
HTTP Request
     |
     v
Controller
     |
     v
MediatR
     |
     v
CommandHandler
```

But communication between separate services typically requires another mechanism:

```text
Order Service
      |
      | HTTP / gRPC / Message Broker
      v
Payment Service
```

For asynchronous communication:

```text
Order Service
      |
      v
RabbitMQ / Kafka / MassTransit
      |
      v
Payment Service
```

So a useful distinction is:

```text
Inside the application
Controller → Mediator/MediatR → Handler
```

versus:

```text
Between services
Service → Message Broker / HTTP / gRPC → Service
```

---

# Key Takeaway

The Mediator Pattern is not simply about forwarding messages.

Its real purpose is to **centralize and manage complex interactions between multiple objects**.

Without Mediator:

```text
A ↔ B
A ↔ C
A ↔ D
B ↔ C
B ↔ D
C ↔ D
```

With Mediator:

```text
        A
        |
B ── Mediator ── C
        |
        D
```

Each component focuses on its own responsibility.

The mediator focuses on **how those components work together**.

A simple rule to remember:

> **Observer says: "Something happened."**

> **Mediator says: "Something happened — now I will decide what should happen next."**

---

## Patterns Compared

```text
Observer
    → Broadcast changes

Mediator
    → Coordinate interactions

Chain of Responsibility
    → Pass a request through handlers

Command
    → Encapsulate an action as an object

State
    → Change behavior based on internal state

Strategy
    → Select interchangeable algorithms
```

Each pattern reduces coupling in a different way.

For Mediator, the key word is:

# Coordination
