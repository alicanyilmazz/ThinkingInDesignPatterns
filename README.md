# Design Patterns

```diff
@@ Command Pattern @@

Command Pattern turns a request or operation into an object.

Instead of directly calling the object that performs the work,
the client creates or sends a Command that represents:

"What should be done?"
```

This makes it possible to:

```text
Queue operations

Delay execution

Retry operations

Log operations

Store operation history

Implement Undo / Redo

Execute commands remotely

Decouple the sender from the actual executor
```

Command-style architectures are common in:

```text
CQRS

MediatR

Background Jobs

Message Queues

ATM Transaction Processing

Desktop Applications

Workflow Systems
```

---

# Understanding the Problem

Suppose we are building ATM software.

The ATM screen initially supports only:

```text
Withdraw
```

A poor design might look like this:

```csharp
public sealed class AtmScreen
{
    private readonly CardService _cardService;
    private readonly BalanceService _balanceService;
    private readonly CashService _cashService;
    private readonly JournalService _journalService;

    public void Withdraw(decimal amount)
    {
        _cardService.Validate();

        _balanceService.Check(amount);

        _cashService.Dispense(amount);

        _journalService.Write(
            $"Withdrawal completed: {amount}");
    }
}
```

At first, this may look acceptable.

Then new operations arrive:

```text
Withdraw

Deposit

Balance Inquiry

PIN Change

Card Return

Cash Transfer

QR Withdrawal
```

The screen starts becoming responsible for every business operation.

Eventually, code like this appears:

```csharp
if (button == "Withdraw")
{
    // Withdrawal logic
}

if (button == "Deposit")
{
    // Deposit logic
}

if (button == "Balance")
{
    // Balance inquiry logic
}

if (button == "PinChange")
{
    // PIN change logic
}
```

```diff
@@ The UI now knows too much about the business logic. @@
```

The ATM screen should know:

```text
The user clicked Withdraw.
```

It should not need to know:

```text
Which service validates the card?

Which service checks the balance?

Which service dispenses the cash?

Which service writes the journal?
```

---

# Command Pattern Solution

Command Pattern says:

```diff
@@ Represent each operation as an object. @@
```

Instead of:

```text
Withdraw()
Deposit()
ChangePin()
ReturnCard()
```

we create:

```text
WithdrawCommand

DepositCommand

ChangePinCommand

ReturnCardCommand
```

Each Command represents an **intent**.

```text
WithdrawCommand
→ Withdraw money.


DepositCommand
→ Deposit money.


ChangePinCommand
→ Change the PIN.
```

---

# Command Interface

```csharp
public interface ICommand
{
    void Execute();
}
```

Every command knows how to initiate one operation.

```text
ICommand
    ↑
 ┌──┼─────────────┐
 ↓  ↓             ↓
WithdrawCommand DepositCommand ChangePinCommand
```

---

# Simple Commands

## Withdraw Command

```csharp
public sealed class WithdrawCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(
            "Withdraw command executed.");
    }
}
```

## Deposit Command

```csharp
public sealed class DepositCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(
            "Deposit command executed.");
    }
}
```

## Balance Inquiry Command

```csharp
public sealed class BalanceInquiryCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(
            "Balance inquiry executed.");
    }
}
```

Usage:

```csharp
ICommand command =
    new WithdrawCommand();

command.Execute();
```

But this example is still too simple.

The real benefit of Command Pattern becomes clearer when we introduce the **Receiver**.

---

# Receiver

The Receiver is the object that actually knows how to perform the business operation.

For example:

```csharp
public sealed class CashService
{
    public void Withdraw(decimal amount)
    {
        Console.WriteLine(
            $"Cash dispensed: {amount}");
    }
}
```

`CashService` knows how to perform the withdrawal.

The Command represents the request.

---

# Command + Receiver

```csharp
public sealed class WithdrawCommand : ICommand
{
    private readonly CashService _cashService;
    private readonly decimal _amount;

    public WithdrawCommand(
        CashService cashService,
        decimal amount)
    {
        _cashService = cashService;
        _amount = amount;
    }

    public void Execute()
    {
        _cashService.Withdraw(_amount);
    }
}
```

Now:

```text
WithdrawCommand
      ↓
represents the request

CashService
      ↓
performs the actual work
```

The Command does not need to contain all business logic itself.

It can delegate to the appropriate Receiver.

---

# Why Store Data Inside the Command?

Notice this:

```csharp
private readonly decimal _amount;
```

The amount belongs to the request.

Therefore the command can represent:

```text
Withdraw 1000 TRY
```

not just:

```text
Withdraw
```

Usage:

```csharp
var command =
    new WithdrawCommand(
        new CashService(),
        1000);

command.Execute();
```

The Command now contains everything needed to execute that request.

---

# Invoker

Another important participant is the **Invoker**.

The Invoker triggers the Command without knowing how the operation works.

For an ATM, the Invoker may be a button.

```csharp
public sealed class Button
{
    private readonly ICommand _command;

    public Button(ICommand command)
    {
        _command = command;
    }

    public void Click()
    {
        _command.Execute();
    }
}
```

Usage:

```csharp
var cashService =
    new CashService();

var withdrawCommand =
    new WithdrawCommand(
        cashService,
        1000);

var withdrawButton =
    new Button(withdrawCommand);

withdrawButton.Click();
```

Execution flow:

```text
User
 ↓
Button
 ↓
WithdrawCommand
 ↓
CashService
 ↓
Withdraw(1000)
```

The Button does not know:

```text
CashService

BalanceService

ATM SDK

Journal Service
```

It only knows:

```csharp
ICommand
```

---

# Main Components

The classical Command Pattern usually contains these participants:

```text
Client
   ↓
creates/configures
   ↓
Command

Invoker
   ↓
executes
   ↓
Command

Command
   ↓
delegates
   ↓
Receiver
```

---

## 1. Command

Defines the execution contract.

```csharp
public interface ICommand
{
    void Execute();
}
```

---

## 2. Concrete Command

Represents one specific request.

```csharp
WithdrawCommand
```

---

## 3. Receiver

Knows how to perform the actual operation.

```csharp
CashService
```

---

## 4. Invoker

Triggers the Command.

```csharp
Button
```

---

## 5. Client

Creates the Command and connects it to the Receiver / Invoker.

```csharp
var command =
    new WithdrawCommand(
        cashService,
        1000);
```

---

# A Better ATM Example

A real withdrawal rarely uses only `CashService`.

Suppose withdrawal requires:

```text
Validate Card

Check Balance

Dispense Cash

Write Journal
```

Receiver:

```csharp
public sealed class WithdrawalService
{
    private readonly CardService _cardService;
    private readonly BalanceService _balanceService;
    private readonly CashService _cashService;
    private readonly JournalService _journalService;

    public WithdrawalService(
        CardService cardService,
        BalanceService balanceService,
        CashService cashService,
        JournalService journalService)
    {
        _cardService = cardService;
        _balanceService = balanceService;
        _cashService = cashService;
        _journalService = journalService;
    }

    public void Withdraw(decimal amount)
    {
        _cardService.Validate();

        _balanceService.Check(amount);

        _cashService.Dispense(amount);

        _journalService.Write(
            $"Withdrawal completed: {amount}");
    }
}
```

Command:

```csharp
public sealed class WithdrawCommand : ICommand
{
    private readonly WithdrawalService _withdrawalService;
    private readonly decimal _amount;

    public WithdrawCommand(
        WithdrawalService withdrawalService,
        decimal amount)
    {
        _withdrawalService = withdrawalService;
        _amount = amount;
    }

    public void Execute()
    {
        _withdrawalService.Withdraw(_amount);
    }
}
```

Invoker:

```csharp
public sealed class AtmButton
{
    private readonly ICommand _command;

    public AtmButton(ICommand command)
    {
        _command = command;
    }

    public void Press()
    {
        _command.Execute();
    }
}
```

Usage:

```csharp
var withdrawalService =
    new WithdrawalService(
        new CardService(),
        new BalanceService(),
        new CashService(),
        new JournalService());

ICommand withdrawCommand =
    new WithdrawCommand(
        withdrawalService,
        1000);

var button =
    new AtmButton(withdrawCommand);

button.Press();
```

Architecture:

```text
AtmButton
    ↓
ICommand
    ↓
WithdrawCommand
    ↓
WithdrawalService
    ↓
 ┌───────┬────────┬───────┬─────────┐
 ↓       ↓        ↓       ↓
Card   Balance   Cash   Journal
```

---

# Why Is This Better?

Without Command:

```text
AtmScreen
   ↓
CashService
BalanceService
CardService
JournalService
...
```

The UI becomes coupled to the business workflow.

With Command:

```text
AtmScreen
   ↓
ICommand
```

The UI only triggers an operation.

```diff
@@ The sender does not need to know how the operation is implemented. @@
```

That is one of the central benefits of Command Pattern.

---

# Commands Can Be Stored

Because the operation is represented as an object:

```csharp
ICommand command
```

we can store commands:

```csharp
var commands = new List<ICommand>();

commands.Add(
    new WithdrawCommand(
        withdrawalService,
        1000));

commands.Add(
    new WithdrawCommand(
        withdrawalService,
        500));
```

Then execute them later:

```csharp
foreach (var command in commands)
{
    command.Execute();
}
```

This would be much harder if the operation existed only as an immediate method call.

---

# Command Queue

Commands can be placed in a queue.

```csharp
Queue<ICommand> queue = new();

queue.Enqueue(
    new WithdrawCommand(
        withdrawalService,
        1000));

queue.Enqueue(
    new WithdrawCommand(
        withdrawalService,
        500));
```

Worker:

```csharp
while (queue.Count > 0)
{
    ICommand command =
        queue.Dequeue();

    command.Execute();
}
```

Conceptually:

```text
Producer
   ↓
Command
   ↓
Queue
   ↓
Worker
   ↓
Execute()
```

This is one reason Command Pattern concepts map naturally to job queues and messaging systems.

---

# Delayed Execution

The sender can create the Command now:

```csharp
var command =
    new SendStatementCommand(
        statementService,
        customerId);
```

but execute it later:

```text
Now
 ↓
Create Command
 ↓
Store / Queue
 ↓
10 minutes later
 ↓
Execute
```

This is an important property of treating a request as an object.

---

# Retry

Because a Command represents an executable operation, retry logic can wrap it.

```csharp
public sealed class RetryCommand : ICommand
{
    private readonly ICommand _command;
    private readonly int _maxAttempts;

    public RetryCommand(
        ICommand command,
        int maxAttempts)
    {
        _command = command;
        _maxAttempts = maxAttempts;
    }

    public void Execute()
    {
        for (int attempt = 1;
             attempt <= _maxAttempts;
             attempt++)
        {
            try
            {
                _command.Execute();

                return;
            }
            catch when (attempt < _maxAttempts)
            {
                Console.WriteLine(
                    $"Retrying... Attempt: {attempt + 1}");
            }
        }
    }
}
```

Usage:

```csharp
ICommand command =
    new RetryCommand(
        new WithdrawCommand(
            withdrawalService,
            1000),
        maxAttempts: 3);

command.Execute();
```

Notice that this example also starts resembling **Decorator Pattern** because one Command wraps another.

Patterns can work together.

---

# Undo / Redo

Undo is one of the classic examples of Command Pattern.

Suppose we are building a text editor.

```csharp
public interface IUndoableCommand
{
    void Execute();

    void Undo();
}
```

Receiver:

```csharp
public sealed class TextDocument
{
    public string Text { get; private set; } = string.Empty;

    public void Append(string text)
    {
        Text += text;
    }

    public void RemoveLast(int length)
    {
        Text =
            Text[..^length];
    }
}
```

Command:

```csharp
public sealed class AppendTextCommand
    : IUndoableCommand
{
    private readonly TextDocument _document;
    private readonly string _text;

    public AppendTextCommand(
        TextDocument document,
        string text)
    {
        _document = document;
        _text = text;
    }

    public void Execute()
    {
        _document.Append(_text);
    }

    public void Undo()
    {
        _document.RemoveLast(
            _text.Length);
    }
}
```

Usage:

```csharp
var document =
    new TextDocument();

var command =
    new AppendTextCommand(
        document,
        "Hello");

command.Execute();

Console.WriteLine(
    document.Text);

// Hello

command.Undo();

Console.WriteLine(
    document.Text);

// Empty
```

Flow:

```text
Command
   ↓
Execute()
   ↓
Change State

Command History
   ↓
Undo()
   ↓
Restore Previous State
```

---

# Command History

Undo systems usually store executed Commands.

```csharp
Stack<IUndoableCommand> history = new();

var command =
    new AppendTextCommand(
        document,
        "Hello");

command.Execute();

history.Push(command);
```

Undo:

```csharp
IUndoableCommand lastCommand =
    history.Pop();

lastCommand.Undo();
```

This gives us:

```text
Execute Command
      ↓
Store Command
      ↓
History
      ↓
Ctrl + Z
      ↓
Undo()
```

---

# Command Pattern and CQRS

CQRS separates:

```text
Commands

and

Queries
```

A Command represents an intent to change state.

Examples:

```text
CreateProductCommand

UpdateCustomerAddressCommand

WithdrawMoneyCommand

BlockCardCommand
```

A Query represents a request for data.

Examples:

```text
GetProductByIdQuery

GetCustomerBalanceQuery

GetTransactionsQuery
```

A common rule is:

```text
Command
→ Changes state.


Query
→ Reads state.
```

For example:

```csharp
public sealed record CreateProductCommand(
    string Name,
    decimal Price);
```

This object represents:

```text
"Create this Product."
```

It is an intent.

---

# Command vs Request

Why use:

```csharp
CreateProductCommand
```

instead of:

```csharp
CreateProductRequest
```

Because the word **Command** communicates semantics.

```text
CreateProductCommand
        ↓
Please perform an operation that changes state.
```

Whereas:

```text
Request
```

is a generic transport term.

Not every request is a Command.

For example:

```text
GET /products/10
```

is a request, but conceptually it is a Query.

---

# MediatR Example

A common modern .NET approach looks like:

```csharp
public sealed record CreateProductCommand(
    string Name,
    decimal Price)
    : IRequest<Guid>;
```

Handler:

```csharp
public sealed class CreateProductCommandHandler
    : IRequestHandler<
        CreateProductCommand,
        Guid>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product =
            new Product(
                request.Name,
                request.Price);

        await _repository.AddAsync(
            product,
            cancellationToken);

        return product.Id;
    }
}
```

Usage:

```csharp
Guid id =
    await mediator.Send(
        new CreateProductCommand(
            "Laptop",
            2500));
```

Conceptually:

```text
Controller
   ↓
CreateProductCommand
   ↓
Mediator
   ↓
CreateProductCommandHandler
   ↓
Repository
```

This strongly resembles Command Pattern:

```text
Sender
  ↓
Command Object
  ↓
Dispatcher
  ↓
Handler
```

However, an important technical distinction is:

> MediatR and CQRS commands are **Command Pattern-inspired**, but they are not necessarily textbook GoF Command implementations with explicit `Invoker → ICommand.Execute() → Receiver` objects.

That distinction is useful in interviews.

---

# MediatR Mapping

A useful mental model is:

```text
GoF Command                  MediatR

Command Object               IRequest / Command

Invoker                      IMediator

Execute()                    Send()

Receiver / Executor          IRequestHandler

Client                       Controller / Application Service
```

Not perfectly identical, but conceptually very similar.

---

# RabbitMQ Example

A message can represent a Command.

For example:

```json
{
  "command": "GenerateStatement",
  "customerId": "12345",
  "month": "2026-07"
}
```

Flow:

```text
API
 ↓
GenerateStatementCommand
 ↓
RabbitMQ
 ↓
Consumer
 ↓
StatementService
```

The producer says:

```text
"Generate this statement."
```

It does not need to know where or when it will be processed.

This is command-style messaging.

But an important distinction:

```diff
@@ Not every RabbitMQ message is a Command. @@
```

For example:

```text
OrderCreatedEvent
```

is an Event, not a Command.

Difference:

```text
Command
→ Please do this.


Event
→ This already happened.
```

---

# Commands vs Events

This distinction is extremely important in distributed systems.

## Command

```text
WithdrawMoneyCommand
```

means:

```text
"Please withdraw money."
```

Usually sent to a specific logical handler.

---

## Event

```text
MoneyWithdrawnEvent
```

means:

```text
"Money has already been withdrawn."
```

Multiple subscribers may react.

So:

```text
Command
→ Intent


Event
→ Fact
```

---

# Background Jobs

Command-style objects are also useful for background processing.

For example:

```text
SendMonthlyStatementCommand
```

may be:

```text
Created now
 ↓
Scheduled
 ↓
Executed tonight
```

With Hangfire or another background job system, the exact implementation may not be textbook GoF Command, but the same important idea appears:

```text
Represent work

Store work

Execute work later
```

---

# Hangfire Example

```csharp
BackgroundJob.Enqueue(
    () =>
        emailService.SendMonthlyStatement(
            customerId));
```

Conceptually:

```text
Request to Send Statement
        ↓
Persist Job
        ↓
Queue
        ↓
Worker
        ↓
Execute Later
```

The job represents work to be executed.

This is **Command-like architecture**, even though Hangfire does not require a GoF `ICommand` interface.

---

# Banking Example — Card Blocking

Suppose a customer reports a stolen card.

We can represent the operation as:

```csharp
public sealed record BlockCardCommand(
    string CardNumber,
    string Reason);
```

Handler:

```csharp
public sealed class BlockCardCommandHandler
{
    private readonly ICardRepository _cardRepository;
    private readonly IJournalService _journalService;

    public BlockCardCommandHandler(
        ICardRepository cardRepository,
        IJournalService journalService)
    {
        _cardRepository = cardRepository;
        _journalService = journalService;
    }

    public async Task Handle(
        BlockCardCommand command)
    {
        var card =
            await _cardRepository.GetAsync(
                command.CardNumber);

        card.Block(command.Reason);

        await _cardRepository.SaveAsync(card);

        await _journalService.WriteAsync(
            $"Card blocked: {command.CardNumber}");
    }
}
```

Flow:

```text
Mobile App
    ↓
BlockCardCommand
    ↓
Handler
    ↓
Card Domain
    ↓
Repository
    ↓
Journal
```

This is a very natural Command scenario because:

```text
Block Card
```

is clearly an **intent to perform an action**.

---

# ATM Example — Remote Command

Suppose operations wants to send a command to an ATM.

Examples:

```text
RestartTerminalCommand

DownloadConfigurationCommand

CollectJournalCommand

UpdateKeysCommand

RunDiagnosticsCommand
```

Command:

```csharp
public sealed record RestartTerminalCommand(
    string TerminalId);
```

The command may be:

```text
Created by ATM Management API
        ↓
Stored in Database
        ↓
Status = WAITING
        ↓
ATM Polls
        ↓
Command Delivered
        ↓
ATM Executes
        ↓
Status = SUCCESS
```

This is an excellent real-world Command architecture.

The command exists independently of when it is executed.

---

# Command State

Because commands are objects, we can attach metadata.

```csharp
public sealed class RestartTerminalCommand
{
    public Guid CommandId { get; init; }

    public string TerminalId { get; init; }

    public DateTime CreatedAt { get; init; }

    public string CreatedBy { get; init; }

    public int RetryCount { get; set; }
}
```

Now we can:

```text
Persist

Audit

Retry

Trace

Schedule

Correlate
```

the operation.

This is one of the biggest practical advantages of representing work as data.

---

# Command Pattern and Serialization

Commands can sometimes be serialized:

```json
{
  "commandId": "0ad433...",
  "terminalId": "ATM001",
  "type": "RestartTerminal",
  "createdAt": "2026-08-16T15:00:00"
}
```

Then transported through:

```text
Database

RabbitMQ

Kafka

Azure Service Bus

HTTP

File
```

However, this applies to **data-oriented command messages**.

A classic GoF Command object may contain runtime references such as:

```csharp
private readonly CashService _cashService;
```

which should not simply be serialized.

In distributed systems, we usually serialize the **Command DTO/message** and resolve the Handler/Receiver on the consumer side.

---

# Command Pattern vs Strategy

These are often confused.

## Strategy

Answers:

```text
HOW should something be done?
```

Example:

```text
Calculate Commission

VisaStrategy
or
MasterCardStrategy
or
TroyStrategy
```

---

## Command

Answers:

```text
WHAT should be done?
```

Example:

```text
WithdrawMoneyCommand

BlockCardCommand

CreateProductCommand
```

Short version:

```text
Strategy
→ HOW?


Command
→ WHAT?
```

---

# Command vs Observer

## Command

```text
Sender
  ↓
"Do this."
  ↓
Handler
```

Usually represents an instruction.

---

## Observer

```text
Something happened
      ↓
Notify Subscribers
      ↓
 ┌────┼─────┐
 ↓    ↓     ↓
A     B     C
```

Short version:

```text
Command
→ Perform an action.


Observer
→ Notify interested parties.
```

---

# Command vs Event

Even more important:

```text
Command
→ Please block this card.


Event
→ This card was blocked.
```

Example:

```text
BlockCardCommand

↓ Execute

CardBlockedEvent
```

A Command may cause an Event.

---

# Command vs Facade

## Facade

Provides a simpler interface to a complex subsystem.

```text
ProcessPayment()
     ↓
Fraud
Ledger
Notification
Gateway
```

---

## Command

Represents an operation as an object.

```text
ProcessPaymentCommand
```

These patterns can work together.

A Command Handler could call a Facade.

---

# Command vs Factory

## Factory

Creates objects.

```text
Which object should I create?
```

## Command

Represents work.

```text
What action should be performed?
```

Short version:

```text
Factory
→ Object creation.


Command
→ Operation representation.
```

---

# Command vs Chain of Responsibility

These patterns are commonly used together.

Command:

```text
WithdrawCommand
```

represents the operation.

Then it may pass through:

```text
Validation
   ↓
Authorization
   ↓
Logging
   ↓
Transaction
   ↓
Handler
```

So:

```text
Command
→ WHAT should happen?


Chain of Responsibility
→ WHICH processing steps should handle it?
```

For example:

```text
WithdrawCommand
       ↓
ValidationBehavior
       ↓
FraudBehavior
       ↓
TransactionBehavior
       ↓
WithdrawCommandHandler
```

---

# Command + Decorator

A Command can also be decorated.

```text
LoggingCommand
     ↓
RetryCommand
     ↓
WithdrawCommand
```

For example:

```csharp
ICommand command =
    new LoggingCommand(
        new RetryCommand(
            new WithdrawCommand(
                withdrawalService,
                1000),
            3));
```

This demonstrates an important lesson:

```diff
@@ Design Patterns are not mutually exclusive. @@

@@ They are often combined in real applications. @@
```

---

# When Should We Use Command Pattern?

```diff
@@ Use Command when: @@

+ You want to represent an operation as an object.

+ The sender should not know the object that actually performs the work.

+ Operations need to be queued.

+ Operations need delayed execution.

+ You need Undo / Redo.

+ You need command history.

+ You want to log or audit requests.

+ Operations may be retried.

+ You need remote or asynchronous command execution.

+ You are implementing CQRS-style write operations.

+ You are building workflow or job-processing systems.
```

---

# When Should We NOT Use Command Pattern?

Do not create a Command class for every trivial method without a reason.

For example:

```csharp
public sealed class GetNameCommand
{
    public string Execute()
    {
        return _user.Name;
    }
}
```

may be unnecessary if there is no need for:

```text
Decoupling

Queueing

History

Dispatching

Retry

Scheduling

Undo

Cross-cutting pipeline behavior
```

Command Pattern introduces abstraction and class count.

Use it where the operation itself has architectural value.

---

# Advantages

```diff
@@ Advantages @@

+ Decouples the sender from the executor.

+ Represents operations as first-class objects.

+ Commands can be queued.

+ Commands can be logged or audited.

+ Commands can be scheduled.

+ Commands can be retried.

+ Commands can be stored in history.

+ Makes Undo / Redo possible.

+ Works well with CQRS.

+ Works well with messaging and background processing.

+ Commands can carry metadata such as correlation IDs and timestamps.
```

---

# Disadvantages

```diff
@@ Disadvantages @@

- Can introduce many small Command classes.

- May be unnecessary for simple CRUD applications.

- Command / Handler structures can become repetitive.

- Serialization becomes more complex if Command objects contain runtime dependencies.

- Too many layers can make execution flow harder to follow.
```

---

# Important Interview Detail

A common oversimplification is:

```text
"Command Pattern means every Command must contain Execute()."
```

That is true for the **classical GoF implementation**:

```csharp
ICommand.Execute();
```

But modern architectures often separate:

```text
Command Data

from

Command Handler
```

For example:

```csharp
public record CreateProductCommand(
    string Name,
    decimal Price);
```

and:

```csharp
public class CreateProductCommandHandler
{
    public Task Handle(
        CreateProductCommand command)
    {
        ...
    }
}
```

This still follows the central Command idea:

> **Represent an operation/request as an object and separate the sender from the code that executes it.**

---

# Classical Command vs Modern Command

```text
Classical GoF
```

```text
Invoker
   ↓
Command.Execute()
   ↓
Receiver
```

Modern CQRS / Mediator:

```text
Sender
   ↓
Command Object
   ↓
Mediator / Dispatcher
   ↓
Command Handler
   ↓
Domain / Repository
```

Both revolve around the same important idea:

```text
Represent the operation independently
from the sender.
```

---

# Interview Summary

```text
Client
  ↓
Invoker
  ↓
Command
  ↓
Receiver
```

Example:

```text
User
 ↓
ATM Button
 ↓
WithdrawCommand
 ↓
WithdrawalService
```

The Command answers:

```text
"What should be done?"
```

The Receiver answers:

```text
"How is it actually done?"
```

> **Command Pattern encapsulates a request as an object, allowing the sender of the request to be decoupled from the object that performs the operation.**

Because requests become objects, they can also be:

```text
Queued

Stored

Scheduled

Logged

Retried

Undone

Sent remotely
```

---

# Interview Questions

### Question 1

What is the primary purpose of Command Pattern?

**A)** Change an algorithm

**B)** Encapsulate an operation or request as an object

**C)** Convert incompatible interfaces

**D)** Create object families

**✅ Answer: B**

---

### Question 2

In classical Command Pattern, which participant actually knows how to perform the business operation?

**A)** Invoker

**B)** Client

**C)** Receiver

**D)** Factory

**✅ Answer: C**

---

### Question 3

Which participant typically triggers `Execute()`?

**A)** Invoker

**B)** Receiver

**C)** Factory

**D)** Adapter

**✅ Answer: A**

---

### Question 4

Which is a classic use case for Command Pattern?

**A)** Undo / Redo

**B)** Queued operations

**C)** Delayed execution

**D)** All of the above

**✅ Answer: D**

---

### Question 5

What is the correct classical flow?

**A)**

```text
Invoker
 ↓
Receiver
 ↓
Command
```

**B)**

```text
Invoker
 ↓
Command
 ↓
Receiver
```

**C)**

```text
Receiver
 ↓
Command
 ↓
Invoker
```

**D)**

```text
Factory
 ↓
Receiver
 ↓
Command
```

**✅ Answer: B**

---

### Question 6

What is the main difference between Strategy and Command?

**A)** Strategy creates objects while Command converts interfaces.

**B)** Strategy represents how an operation is performed, while Command represents what operation should be performed.

**C)** They are identical.

**D)** Command can only be used in UI applications.

**✅ Answer: B**

---

### Question 7

Which statement about RabbitMQ is correct?

**A)** Every RabbitMQ message is a Command.

**B)** RabbitMQ cannot be used with Commands.

**C)** A message such as `GenerateStatementCommand` can represent a Command, while `StatementGeneratedEvent` represents an Event.

**D)** Commands and Events are identical.

**✅ Answer: C**

---

### Question 8

Why are MediatR Commands often associated with Command Pattern?

**A)** Because every MediatR request uses `Execute()`.

**B)** Because an operation is represented as an object and dispatched to a Handler, decoupling the sender from execution logic.

**C)** Because MediatR creates database tables.

**D)** Because every Command implements Undo.

**✅ Answer: B**

---

### Question 9

Which statement best describes the relationship between CQRS and Command Pattern?

**A)** CQRS and Command Pattern are exactly the same.

**B)** CQRS often models state-changing intentions as Command objects, but CQRS is a broader architectural pattern.

**C)** Command Pattern requires CQRS.

**D)** CQRS only contains Queries.

**✅ Answer: B**

---

# Final Cheat Sheet

```text
Command
→ WHAT should be done?


Strategy
→ HOW should it be done?


Factory
→ WHICH object should be created?


Observer
→ WHO should be notified?


Chain of Responsibility
→ WHICH handlers should process the request?


Adapter
→ HOW can incompatible interfaces work together?
```

And remember the classical Command flow:

```text
Client
  ↓
Creates Command

Invoker
  ↓
Execute()

Command
  ↓
Calls Receiver

Receiver
  ↓
Performs Business Operation
```

Modern .NET version:

```text
Controller
   ↓
CreateProductCommand
   ↓
IMediator
   ↓
CreateProductCommandHandler
   ↓
Domain / Repository
```

The most important sentence:

> **Command Pattern turns an operation into an object, separating the code that requests an action from the code that actually performs it.**
