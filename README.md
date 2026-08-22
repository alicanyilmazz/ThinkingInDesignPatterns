# Design Patterns — Command Pattern

> **Command Pattern turns a request or operation into an object.**
>
> Instead of directly calling the object that performs the work, the client creates a **Command** object that represents **what should be done**.

This makes an operation something we can:

- Queue
- Delay
- Retry
- Log
- Audit
- Store in history
- Undo / Redo
- Schedule
- Execute remotely
- Dispatch to another process
- Decouple from the object that actually performs the work

Command-style architectures are common in:

- CQRS
- MediatR
- Background Jobs
- Message Queues
- Banking systems
- ATM transaction processing
- Desktop applications
- Workflow systems

---

# Table of Contents

1. [The Core Idea](#the-core-idea)
2. [The Problem Without Command Pattern](#the-problem-without-command-pattern)
3. [Main Participants](#main-participants)
4. [The Simplest Command Example](#the-simplest-command-example)
5. [Example 1 — Restaurant Order System](#example-1--restaurant-order-system)
6. [Example 2 — Bank Money Transfer](#example-2--bank-money-transfer)
7. [Example 3 — E-Commerce Order System](#example-3--e-commerce-order-system)
8. [Example 4 — Background Job Queue](#example-4--background-job-queue)
9. [ATM Example](#atm-example)
10. [Why Commands Can Be Queued](#why-commands-can-be-queued)
11. [Retry](#retry)
12. [Undo / Redo](#undo--redo)
13. [Command Pattern and CQRS](#command-pattern-and-cqrs)
14. [Command Pattern and MediatR](#command-pattern-and-mediatr)
15. [Command Pattern and RabbitMQ](#command-pattern-and-rabbitmq)
16. [Command vs Event](#command-vs-event)
17. [Command vs Strategy](#command-vs-strategy)
18. [Command vs Other Patterns](#command-vs-other-patterns)
19. [When Should We Use Command Pattern?](#when-should-we-use-command-pattern)
20. [Advantages and Disadvantages](#advantages-and-disadvantages)
21. [Interview Summary](#interview-summary)
22. [Interview Questions](#interview-questions)

---

# The Core Idea

The most important sentence is:

> **Command Pattern encapsulates a request as an object.**

Normally we may write:

```csharp
orderService.CreateOrder(order);
```

That method call executes immediately.

With Command Pattern:

```csharp
ICommand command =
    new CreateOrderCommand(
        orderService,
        order);
```

At this point, the operation has **not** been executed yet.

We created an object that means:

```text
Create this order.
```

Later:

```csharp
command.Execute();
```

executes the request.

That difference is extremely important.

A normal method call is immediate:

```text
Call method
    ↓
Execute work
    ↓
Finish
```

A Command object can exist independently:

```text
Create Command
    ↓
Store / Queue / Log / Schedule
    ↓
Execute later
```

This is why a Command can be treated like data representing work.

---

# The Problem Without Command Pattern

Suppose we are building ATM software.

The ATM screen initially supports:

```text
Withdraw
```

A simple implementation may look like this:

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

At first this looks acceptable.

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

The UI starts knowing too much:

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
```

The screen should only know:

```text
The user requested Withdraw.
```

It should not need to know:

```text
Which service validates the card?
Which service checks the balance?
Which service dispenses cash?
Which service writes the journal?
```

Command Pattern solves this by representing each operation as an object:

```text
WithdrawCommand
DepositCommand
ChangePinCommand
ReturnCardCommand
```

---

# Main Participants

The classical GoF Command Pattern usually contains five participants.

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

Examples:

```text
WithdrawCommand
TransferMoneyCommand
CreateOrderCommand
SendEmailCommand
```

---

## 3. Receiver

The Receiver knows **how the actual work is done**.

Examples:

```text
WithdrawalService
BankService
OrderService
EmailService
```

The Command usually delegates the work to the Receiver.

---

## 4. Invoker

The Invoker triggers the Command.

It knows:

```csharp
ICommand
```

but does not need to know the internal business logic.

Examples:

```text
Button
OrderInvoker
BackgroundJobWorker
```

---

## 5. Client

The Client creates/configures Commands and connects them with Receivers and Invokers.

Usually in Console examples:

```text
Program.cs
```

---

## Classical Flow

```text
Client
   ↓
Creates Command
   ↓
Invoker
   ↓
Execute()
   ↓
Concrete Command
   ↓
Receiver
   ↓
Business Operation
```

A useful mental model:

```text
Command
→ WHAT should be done?

Receiver
→ HOW is it actually done?

Invoker
→ WHEN should it be triggered?

Client
→ Which Command should be created?
```

---

# The Simplest Command Example

## ICommand.cs

```csharp
public interface ICommand
{
    void Execute();
}
```

## CashService.cs

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

`CashService` is the **Receiver**.

## WithdrawCommand.cs

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

The Command contains everything required to represent:

```text
Withdraw 1000 TRY
```

Usage:

```csharp
var command =
    new WithdrawCommand(
        new CashService(),
        1000);

command.Execute();
```

---

# Example 1 — Restaurant Order System

This example is useful because the real-life roles are very easy to understand.

A customer says:

```text
Prepare one pizza.
```

The waiter does not cook the pizza.

The waiter receives the order and sends it to the kitchen.

```text
Customer
   ↓
Waiter
   ↓
Order
   ↓
Kitchen
```

Command Pattern mapping:

| Command Pattern | Restaurant Example |
|---|---|
| Client | `Program.cs` |
| Invoker | `Waiter` |
| Command | `ICommand` |
| Concrete Command | `PizzaOrderCommand` |
| Receiver | `Kitchen` |

## ICommand.cs

```csharp
public interface ICommand
{
    void Execute();
}
```

## Kitchen.cs

```csharp
public sealed class Kitchen
{
    public void MakePizza(string customerName)
    {
        Console.WriteLine(
            $"{customerName} için pizza hazırlanıyor.");
    }

    public void MakeBurger(string customerName)
    {
        Console.WriteLine(
            $"{customerName} için burger hazırlanıyor.");
    }
}
```

`Kitchen` is the Receiver because it performs the actual work.

## PizzaOrderCommand.cs

```csharp
public sealed class PizzaOrderCommand : ICommand
{
    private readonly Kitchen _kitchen;
    private readonly string _customerName;

    public PizzaOrderCommand(
        Kitchen kitchen,
        string customerName)
    {
        _kitchen = kitchen;
        _customerName = customerName;
    }

    public void Execute()
    {
        _kitchen.MakePizza(_customerName);
    }
}
```

## BurgerOrderCommand.cs

```csharp
public sealed class BurgerOrderCommand : ICommand
{
    private readonly Kitchen _kitchen;
    private readonly string _customerName;

    public BurgerOrderCommand(
        Kitchen kitchen,
        string customerName)
    {
        _kitchen = kitchen;
        _customerName = customerName;
    }

    public void Execute()
    {
        _kitchen.MakeBurger(_customerName);
    }
}
```

## Waiter.cs

```csharp
public sealed class Waiter
{
    public void TakeOrder(ICommand command)
    {
        Console.WriteLine("Garson siparişi aldı.");

        command.Execute();
    }
}
```

Notice that `Waiter` does not depend on:

```text
PizzaOrderCommand
BurgerOrderCommand
Kitchen.MakePizza()
Kitchen.MakeBurger()
```

It only knows:

```csharp
ICommand
```

## Program.cs

```csharp
var kitchen =
    new Kitchen();

var waiter =
    new Waiter();

ICommand pizzaOrder =
    new PizzaOrderCommand(
        kitchen,
        "Alican");

ICommand burgerOrder =
    new BurgerOrderCommand(
        kitchen,
        "Mehmet");

waiter.TakeOrder(pizzaOrder);

waiter.TakeOrder(burgerOrder);
```

Flow:

```text
Program
   ↓
Waiter
   ↓
ICommand.Execute()
   ↓
PizzaOrderCommand
   ↓
Kitchen.MakePizza()
```

The key idea:

```csharp
kitchen.MakePizza("Alican");
```

was turned into:

```csharp
ICommand command =
    new PizzaOrderCommand(
        kitchen,
        "Alican");
```

The request is now an object.

---

# Example 2 — Bank Money Transfer

Now let us move to a more realistic backend example.

We want to transfer money from one bank account to another.

Normal approach:

```csharp
bankService.TransferMoney(
    sender,
    receiver,
    2500);
```

Command approach:

```csharp
ICommand command =
    new TransferMoneyCommand(
        bankService,
        sender,
        receiver,
        2500);
```

The object now represents:

```text
Transfer 2500 TL
from Alican
to Mehmet
```

## BankAccount.cs

```csharp
public sealed class BankAccount
{
    public string Iban { get; }

    public string CustomerName { get; }

    public decimal Balance { get; private set; }

    public BankAccount(
        string iban,
        string customerName,
        decimal balance)
    {
        Iban = iban;
        CustomerName = customerName;
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception(
                "Amount must be greater than zero.");
        }

        if (Balance < amount)
        {
            throw new Exception(
                "Insufficient balance.");
        }

        Balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception(
                "Amount must be greater than zero.");
        }

        Balance += amount;
    }
}
```

## BankService.cs

`BankService` is the Receiver.

```csharp
public sealed class BankService
{
    public void TransferMoney(
        BankAccount sender,
        BankAccount receiver,
        decimal amount)
    {
        Console.WriteLine(
            "Transfer started.");

        sender.Withdraw(amount);

        receiver.Deposit(amount);

        Console.WriteLine(
            $"{amount:N2} TL transferred.");

        Console.WriteLine(
            $"Sender: {sender.CustomerName}");

        Console.WriteLine(
            $"Receiver: {receiver.CustomerName}");
    }
}
```

A real banking service could additionally perform:

```text
Account validation
Balance check
Daily limit validation
Fraud control
Ledger update
Transaction persistence
Audit logging
Notification
```

## TransferMoneyCommand.cs

```csharp
public sealed class TransferMoneyCommand : ICommand
{
    private readonly BankService _bankService;
    private readonly BankAccount _sender;
    private readonly BankAccount _receiver;
    private readonly decimal _amount;

    public TransferMoneyCommand(
        BankService bankService,
        BankAccount sender,
        BankAccount receiver,
        decimal amount)
    {
        _bankService = bankService;
        _sender = sender;
        _receiver = receiver;
        _amount = amount;
    }

    public void Execute()
    {
        _bankService.TransferMoney(
            _sender,
            _receiver,
            _amount);
    }
}
```

## BankOperationInvoker.cs

```csharp
public sealed class BankOperationInvoker
{
    public void ExecuteCommand(
        ICommand command)
    {
        Console.WriteLine(
            "Bank operation received.");

        command.Execute();

        Console.WriteLine(
            "Bank operation completed.");
    }
}
```

The Invoker does not know:

```text
Sender
Receiver
Amount
BankService
Transfer workflow
```

It only knows:

```csharp
command.Execute();
```

## Program.cs

```csharp
var sender =
    new BankAccount(
        "TR001",
        "Alican",
        10000);

var receiver =
    new BankAccount(
        "TR002",
        "Mehmet",
        5000);

var bankService =
    new BankService();

ICommand transferCommand =
    new TransferMoneyCommand(
        bankService,
        sender,
        receiver,
        2500);

var invoker =
    new BankOperationInvoker();

invoker.ExecuteCommand(
    transferCommand);
```

Execution flow:

```text
Program
   ↓
BankOperationInvoker
   ↓
ICommand.Execute()
   ↓
TransferMoneyCommand
   ↓
BankService.TransferMoney()
   ↓
BankAccount.Withdraw()
   ↓
BankAccount.Deposit()
```

Again, the most important point:

> Creating `TransferMoneyCommand` does not execute the transfer. It creates an object representing the transfer request.

---

# Example 3 — E-Commerce Order System

Now consider an e-commerce application.

Operations may include:

```text
Create Order
Cancel Order
Ship Order
Refund Order
Return Order
```

Instead of letting the UI/controller directly know every service call, we can model operations as Commands.

## Order.cs

```csharp
public sealed class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = string.Empty;
}
```

## OrderService.cs

```csharp
public sealed class OrderService
{
    public void CreateOrder(Order order)
    {
        Console.WriteLine(
            "Creating order...");

        order.Status = "Created";

        Console.WriteLine(
            $"Order created: {order.Id}");
    }

    public void CancelOrder(Order order)
    {
        Console.WriteLine(
            "Cancelling order...");

        order.Status = "Cancelled";

        Console.WriteLine(
            $"Order cancelled: {order.Id}");
    }
}
```

`OrderService` is the Receiver.

## CreateOrderCommand.cs

```csharp
public sealed class CreateOrderCommand : ICommand
{
    private readonly OrderService _orderService;
    private readonly Order _order;

    public CreateOrderCommand(
        OrderService orderService,
        Order order)
    {
        _orderService = orderService;
        _order = order;
    }

    public void Execute()
    {
        _orderService.CreateOrder(_order);
    }
}
```

## CancelOrderCommand.cs

```csharp
public sealed class CancelOrderCommand : ICommand
{
    private readonly OrderService _orderService;
    private readonly Order _order;

    public CancelOrderCommand(
        OrderService orderService,
        Order order)
    {
        _orderService = orderService;
        _order = order;
    }

    public void Execute()
    {
        _orderService.CancelOrder(_order);
    }
}
```

## OrderInvoker.cs

```csharp
public sealed class OrderInvoker
{
    public void ExecuteCommand(
        ICommand command)
    {
        Console.WriteLine(
            "Operation received.");

        command.Execute();

        Console.WriteLine(
            "Operation completed.");
    }
}
```

## Program.cs

```csharp
var order =
    new Order
    {
        Id = 1001,
        CustomerName = "Alican",
        ProductName = "iPhone",
        Quantity = 1,
        TotalPrice = 60000
    };

var orderService =
    new OrderService();

var invoker =
    new OrderInvoker();

ICommand createCommand =
    new CreateOrderCommand(
        orderService,
        order);

invoker.ExecuteCommand(
    createCommand);

Console.WriteLine(
    order.Status);

ICommand cancelCommand =
    new CancelOrderCommand(
        orderService,
        order);

invoker.ExecuteCommand(
    cancelCommand);

Console.WriteLine(
    order.Status);
```

The same Invoker can execute both operations because both are `ICommand`.

Tomorrow we can add:

```text
ShipOrderCommand
RefundOrderCommand
ReturnOrderCommand
ChangeAddressCommand
```

without changing `OrderInvoker`.

This is an example of **decoupling the sender from the implementation of the operation**.

---

# Example 4 — Background Job Queue

This is one of the most practical Command Pattern examples.

Suppose an application needs to perform background work:

```text
Send Email
Generate Report
Process Payment
```

We do not want to execute every job immediately inside the main request.

Instead:

```text
Application
    ↓
Create Command
    ↓
BackgroundJobQueue
    ↓
Worker
    ↓
Command.Execute()
    ↓
Service
```

The important idea is:

> A Command can be created now and executed later.

---

## Services — Receivers

### EmailService.cs

```csharp
public sealed class EmailService
{
    public void SendEmail(
        string email,
        string message)
    {
        Console.WriteLine(
            $"Sending email to {email}");

        Console.WriteLine(
            message);

        Console.WriteLine(
            "Email sent.");
    }
}
```

### ReportService.cs

```csharp
public sealed class ReportService
{
    public void GenerateReport(
        int customerId)
    {
        Console.WriteLine(
            $"Generating report for CustomerId: {customerId}");

        Console.WriteLine(
            "Report generated.");
    }
}
```

### PaymentService.cs

```csharp
public sealed class PaymentService
{
    public void ProcessPayment(
        int orderId,
        decimal amount)
    {
        Console.WriteLine(
            $"Processing payment for OrderId: {orderId}");

        Console.WriteLine(
            $"Amount: {amount:N2} TL");

        Console.WriteLine(
            "Payment completed.");
    }
}
```

---

## SendEmailCommand.cs

```csharp
public sealed class SendEmailCommand : ICommand
{
    private readonly EmailService _emailService;
    private readonly string _email;
    private readonly string _message;

    public SendEmailCommand(
        EmailService emailService,
        string email,
        string message)
    {
        _emailService = emailService;
        _email = email;
        _message = message;
    }

    public void Execute()
    {
        _emailService.SendEmail(
            _email,
            _message);
    }
}
```

---

## GenerateReportCommand.cs

```csharp
public sealed class GenerateReportCommand : ICommand
{
    private readonly ReportService _reportService;
    private readonly int _customerId;

    public GenerateReportCommand(
        ReportService reportService,
        int customerId)
    {
        _reportService = reportService;
        _customerId = customerId;
    }

    public void Execute()
    {
        _reportService.GenerateReport(
            _customerId);
    }
}
```

---

## ProcessPaymentCommand.cs

```csharp
public sealed class ProcessPaymentCommand : ICommand
{
    private readonly PaymentService _paymentService;
    private readonly int _orderId;
    private readonly decimal _amount;

    public ProcessPaymentCommand(
        PaymentService paymentService,
        int orderId,
        decimal amount)
    {
        _paymentService = paymentService;
        _orderId = orderId;
        _amount = amount;
    }

    public void Execute()
    {
        _paymentService.ProcessPayment(
            _orderId,
            _amount);
    }
}
```

---

## BackgroundJobQueue.cs

```csharp
public sealed class BackgroundJobQueue
{
    private readonly Queue<ICommand> _commands =
        new Queue<ICommand>();

    public void Enqueue(
        ICommand command)
    {
        _commands.Enqueue(command);

        Console.WriteLine(
            $"Queued: {command.GetType().Name}");
    }

    public ICommand? Dequeue()
    {
        if (_commands.Count == 0)
        {
            return null;
        }

        return _commands.Dequeue();
    }

    public int Count =>
        _commands.Count;
}
```

The critical line is:

```csharp
Queue<ICommand>
```

We did **not** create:

```csharp
Queue<SendEmailCommand>
```

because we want the same queue to accept:

```text
SendEmailCommand
GenerateReportCommand
ProcessPaymentCommand
```

All of them implement:

```csharp
ICommand
```

---

## FIFO

A queue works with:

```text
FIFO
First In, First Out
```

For example:

```csharp
queue.Enqueue(emailCommand);
queue.Enqueue(reportCommand);
queue.Enqueue(paymentCommand);
```

Conceptually:

```text
FRONT
  ↓
┌─────────────────────────┐
│ SendEmailCommand        │
├─────────────────────────┤
│ GenerateReportCommand   │
├─────────────────────────┤
│ ProcessPaymentCommand   │
└─────────────────────────┘
```

The first `Dequeue()` returns:

```text
SendEmailCommand
```

The next returns:

```text
GenerateReportCommand
```

---

## BackgroundJobWorker.cs

The Worker acts like the Invoker.

```csharp
public sealed class BackgroundJobWorker
{
    private readonly BackgroundJobQueue _queue;

    public BackgroundJobWorker(
        BackgroundJobQueue queue)
    {
        _queue = queue;
    }

    public void Run()
    {
        Console.WriteLine(
            "Background Worker started.");

        while (_queue.Count > 0)
        {
            ICommand? command =
                _queue.Dequeue();

            if (command is null)
            {
                continue;
            }

            Console.WriteLine(
                $"Executing: {command.GetType().Name}");

            command.Execute();

            Console.WriteLine(
                "Job completed.");

            Console.WriteLine(
                "-------------------------");
        }

        Console.WriteLine(
            "Queue is empty.");
    }
}
```

The heart of the Worker is:

```csharp
command.Execute();
```

The Worker does not know:

```text
EmailService
ReportService
PaymentService
```

It does not know whether the current job is:

```text
Email
Report
Payment
```

It only knows:

```csharp
ICommand
```

This is polymorphism working together with Command Pattern.

---

## Program.cs

```csharp
var emailService =
    new EmailService();

var reportService =
    new ReportService();

var paymentService =
    new PaymentService();

var queue =
    new BackgroundJobQueue();

var worker =
    new BackgroundJobWorker(
        queue);

ICommand emailCommand =
    new SendEmailCommand(
        emailService,
        "alican@test.com",
        "Your order has been created.");

ICommand reportCommand =
    new GenerateReportCommand(
        reportService,
        1001);

ICommand paymentCommand =
    new ProcessPaymentCommand(
        paymentService,
        5001,
        2500);

queue.Enqueue(
    emailCommand);

queue.Enqueue(
    reportCommand);

queue.Enqueue(
    paymentCommand);

Console.WriteLine(
    $"Queue count: {queue.Count}");

worker.Run();
```

Before `worker.Run()`:

```text
No email has been sent.
No report has been generated.
No payment has been processed.
```

The Commands only exist as queued work.

Then:

```csharp
worker.Run();
```

starts consuming them.

Execution:

```text
BackgroundJobWorker
        ↓
Queue.Dequeue()
        ↓
ICommand
        ↓
Concrete Command
        ↓
Execute()
        ↓
Receiver Service
```

For the first job:

```text
Worker
  ↓
SendEmailCommand
  ↓
EmailService.SendEmail()
```

Second:

```text
Worker
  ↓
GenerateReportCommand
  ↓
ReportService.GenerateReport()
```

Third:

```text
Worker
  ↓
ProcessPaymentCommand
  ↓
PaymentService.ProcessPayment()
```

The Worker never changes.

---

# Why Commands Can Be Queued

This is one of the biggest advantages of representing work as an object.

Without a Command:

```csharp
emailService.SendEmail(...);
```

The method executes immediately.

With a Command:

```csharp
ICommand command =
    new SendEmailCommand(
        emailService,
        email,
        message);
```

we can:

```csharp
queue.Enqueue(command);
```

and execute later.

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

This maps naturally to real-world systems such as:

```text
Hangfire
RabbitMQ
Azure Service Bus
AWS SQS
MassTransit
```

However, these systems are not automatically textbook GoF Command implementations.

They use a similar architectural idea:

> Represent work, transport/store it, and execute it later.

---

# Web API + Background Job Example

Suppose this code runs inside an API endpoint.

Poor approach:

```csharp
public void CreateOrder()
{
    SaveOrder();

    SendEmail();

    GenerateInvoice();

    GenerateReport();

    SendNotification();

    UpdateAnalytics();
}
```

The HTTP request waits for all operations.

Instead:

```text
HTTP Request
    ↓
Save Order
    ↓
Return Response
```

and background work can be queued:

```text
SendEmailCommand
GenerateInvoiceCommand
GenerateReportCommand
SendNotificationCommand
```

Conceptually:

```text
Client
   ↓
POST /orders
   ↓
API
   ↓
Save Order
   ↓
Queue Background Commands
   ↓
200 / 201 Response

Meanwhile...

Worker
   ↓
Consumes Queue
   ↓
Executes Jobs
```

This is one reason job queues improve response time and isolate slow or retryable work.

---

# ATM Example

A more realistic ATM withdrawal may require:

```text
Validate Card
Check Balance
Dispense Cash
Write Journal
```

## WithdrawalService.cs

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

## WithdrawCommand.cs

```csharp
public sealed class WithdrawCommand : ICommand
{
    private readonly WithdrawalService _withdrawalService;
    private readonly decimal _amount;

    public WithdrawCommand(
        WithdrawalService withdrawalService,
        decimal amount)
    {
        _withdrawalService =
            withdrawalService;

        _amount =
            amount;
    }

    public void Execute()
    {
        _withdrawalService.Withdraw(
            _amount);
    }
}
```

## AtmButton.cs

```csharp
public sealed class AtmButton
{
    private readonly ICommand _command;

    public AtmButton(
        ICommand command)
    {
        _command = command;
    }

    public void Press()
    {
        _command.Execute();
    }
}
```

Flow:

```text
User
 ↓
ATM Button
 ↓
ICommand
 ↓
WithdrawCommand
 ↓
WithdrawalService
 ↓
Card / Balance / Cash / Journal
```

The button only knows:

```csharp
ICommand
```

It does not need to know the withdrawal workflow.

---

# ATM Remote Command Example

Command architecture becomes even more interesting when commands are not executed immediately.

Examples:

```text
RestartTerminalCommand
DownloadConfigurationCommand
CollectJournalCommand
UpdateKeysCommand
RunDiagnosticsCommand
```

A remote ATM command could flow like this:

```text
ATM Management API
        ↓
Create Command
        ↓
Store in Database
        ↓
Status = WAITING
        ↓
ATM Polls
        ↓
Command Delivered
        ↓
ATM Executes
        ↓
Status = SUCCESS / ERROR
```

The Command exists independently from when it is executed.

This is a very practical example of:

```text
Represent work as data.
```

Commands may also contain metadata:

```csharp
public sealed class RestartTerminalCommand
{
    public Guid CommandId { get; init; }

    public string TerminalId { get; init; } =
        string.Empty;

    public DateTime CreatedAt { get; init; }

    public string CreatedBy { get; init; } =
        string.Empty;

    public int RetryCount { get; set; }
}
```

This allows us to:

```text
Persist
Audit
Retry
Trace
Schedule
Correlate
```

---

# Retry

Because a Command represents an executable operation, we can wrap it with retry behavior.

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
            catch when (
                attempt < _maxAttempts)
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
        new SendEmailCommand(
            emailService,
            "test@test.com",
            "Hello"),
        maxAttempts: 3);

command.Execute();
```

Notice that this also resembles the **Decorator Pattern** because one Command wraps another.

Patterns can work together.

---

# Undo / Redo

Undo is one of the classic Command Pattern use cases.

## IUndoableCommand.cs

```csharp
public interface IUndoableCommand
{
    void Execute();

    void Undo();
}
```

## TextDocument.cs

```csharp
public sealed class TextDocument
{
    public string Text { get; private set; } =
        string.Empty;

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

## AppendTextCommand.cs

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

Command history:

```csharp
Stack<IUndoableCommand> history =
    new();

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

Flow:

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

A Command expresses an intent to change state.

Examples:

```text
CreateProductCommand
UpdateCustomerAddressCommand
WithdrawMoneyCommand
BlockCardCommand
```

A Query asks for data.

Examples:

```text
GetProductByIdQuery
GetCustomerBalanceQuery
GetTransactionsQuery
```

A useful rule:

```text
Command
→ Changes state.

Query
→ Reads state.
```

Example Command:

```csharp
public sealed record CreateProductCommand(
    string Name,
    decimal Price);
```

This object means:

```text
Create this product.
```

It expresses intent.

CQRS and Command Pattern are related, but they are not the same thing.

> CQRS is a broader architectural pattern. Command objects are often used to represent state-changing intentions inside CQRS.

---

# Command Pattern and MediatR

A common modern .NET approach:

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

Flow:

```text
Controller
   ↓
CreateProductCommand
   ↓
IMediator
   ↓
CreateProductCommandHandler
   ↓
Repository
```

Useful mental mapping:

| Classical GoF | MediatR |
|---|---|
| Command Object | `IRequest` / Command |
| Invoker | `IMediator` |
| `Execute()` | `Send()` |
| Receiver / Executor | `IRequestHandler` |
| Client | Controller / Application Service |

Important interview detail:

> MediatR and CQRS Commands are **Command Pattern-inspired**, but they are not necessarily textbook GoF Command implementations with explicit `Invoker → ICommand.Execute() → Receiver`.

Classical GoF:

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
Mediator
   ↓
Command Handler
   ↓
Domain / Repository
```

Both share the central idea:

> Represent the operation independently from the sender.

---

# Command Pattern and RabbitMQ

A message can represent a Command.

Example:

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
Generate this statement.
```

It does not need to know exactly when or where the work is processed.

Important:

> **Not every RabbitMQ message is a Command.**

For example:

```text
OrderCreatedEvent
```

is an Event.

---

# Command vs Event

This distinction is extremely important in distributed systems.

## Command

```text
BlockCardCommand
```

means:

```text
Please block this card.
```

It represents **intent**.

---

## Event

```text
CardBlockedEvent
```

means:

```text
The card has already been blocked.
```

It represents a **fact**.

Short version:

```text
Command
→ Intent

Event
→ Fact
```

A Command can cause an Event:

```text
BlockCardCommand
       ↓
Execute
       ↓
CardBlockedEvent
```

---

# Command Pattern and Serialization

In distributed systems, Commands are often data-oriented messages:

```json
{
  "commandId": "0ad433...",
  "terminalId": "ATM001",
  "type": "RestartTerminal",
  "createdAt": "2026-08-16T15:00:00"
}
```

They may be transported through:

```text
Database
RabbitMQ
Kafka
Azure Service Bus
HTTP
File
```

However, a classical GoF Command may contain runtime object references:

```csharp
private readonly CashService _cashService;
```

That object should not simply be serialized.

In distributed architectures we usually serialize:

```text
Command DTO / Message
```

and resolve the:

```text
Handler / Receiver
```

on the consumer side.

---

# Command vs Strategy

These patterns are commonly confused.

## Strategy

Strategy answers:

```text
HOW should something be done?
```

Example:

```text
Calculate Commission

VisaStrategy
MasterCardStrategy
TroyStrategy
```

---

## Command

Command answers:

```text
WHAT should be done?
```

Examples:

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

# Command vs Other Patterns

## Command vs Observer

Command:

```text
Sender
  ↓
Do this.
  ↓
Handler
```

Observer:

```text
Something happened
      ↓
Notify subscribers
      ↓
A   B   C
```

Short version:

```text
Command
→ Perform an action.

Observer
→ Notify interested parties.
```

---

## Command vs Facade

Facade:

```text
Simplifies access to a complex subsystem.
```

Example:

```text
ProcessPayment()
   ↓
Fraud
Ledger
Gateway
Notification
```

Command:

```text
Represents an operation as an object.
```

Example:

```text
ProcessPaymentCommand
```

They can work together.

A Command Handler may call a Facade.

---

## Command vs Factory

Factory:

```text
Which object should be created?
```

Command:

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

## Command vs Chain of Responsibility

Command represents the operation:

```text
WithdrawCommand
```

A Chain of Responsibility may process it through:

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

Short version:

```text
Command
→ WHAT should happen?

Chain of Responsibility
→ WHICH processing steps should handle it?
```

Example:

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

## Command + Decorator

A Command can be decorated:

```text
LoggingCommand
     ↓
RetryCommand
     ↓
WithdrawCommand
```

Example:

```csharp
ICommand command =
    new LoggingCommand(
        new RetryCommand(
            new WithdrawCommand(
                withdrawalService,
                1000),
            3));
```

Design Patterns are not mutually exclusive.

They are often combined in real applications.

---

# When Should We Use Command Pattern?

Use Command Pattern when:

- You want to represent an operation as an object.
- The sender should not know the object that performs the work.
- Operations need to be queued.
- Operations need delayed execution.
- You need Undo / Redo.
- You need command history.
- You want to log or audit requests.
- Operations may be retried.
- You need remote/asynchronous execution.
- You are implementing CQRS-style write operations.
- You are building workflow or job-processing systems.
- You need scheduling.
- You want commands to carry metadata such as correlation IDs, timestamps, retry counts or user information.

---

# When Should We NOT Use Command Pattern?

Do not create a Command class for every trivial method without a real reason.

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

may be unnecessary when there is no need for:

```text
Decoupling
Queueing
History
Dispatching
Retry
Scheduling
Undo
Cross-cutting behavior
```

Command Pattern introduces:

```text
More abstractions
More classes
More indirection
```

Use it when the operation itself has architectural value.

---

# Advantages and Disadvantages

## Advantages

- Decouples the sender from the executor.
- Represents operations as first-class objects.
- Commands can be queued.
- Commands can be delayed.
- Commands can be logged or audited.
- Commands can be scheduled.
- Commands can be retried.
- Commands can be stored in history.
- Makes Undo / Redo possible.
- Works naturally with CQRS concepts.
- Works well with messaging and background processing.
- Commands can carry metadata such as correlation IDs and timestamps.
- Invokers/workers can execute different commands through the same interface.

---

## Disadvantages

- Can introduce many small Command classes.
- May be unnecessary for simple CRUD applications.
- Command/Handler structures can become repetitive.
- More layers can make execution flow harder to follow.
- Serialization is more complex when classic Command objects contain runtime dependencies.
- Poorly designed Commands may become large service wrappers instead of simple operation representations.

---

# Important Interview Detail

A common oversimplification is:

```text
Command Pattern means every Command must contain Execute().
```

That is true for a **classical GoF implementation**:

```csharp
ICommand.Execute();
```

But modern architectures often separate:

```text
Command Data
from
Command Handler
```

Example:

```csharp
public record CreateProductCommand(
    string Name,
    decimal Price);
```

and:

```csharp
public sealed class CreateProductCommandHandler
{
    public Task Handle(
        CreateProductCommand command)
    {
        // ...
        return Task.CompletedTask;
    }
}
```

The central idea is still:

> **Represent an operation/request as an object and separate the sender from the code that executes it.**

---

# Interview Summary

The classical Command Pattern:

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

Another example:

```text
Program
 ↓
BackgroundJobWorker
 ↓
SendEmailCommand
 ↓
EmailService
```

The Command answers:

```text
WHAT should be done?
```

The Receiver answers:

```text
HOW is it actually done?
```

The Invoker answers:

```text
WHEN should the Command be triggered?
```

The most important sentence:

> **Command Pattern turns an operation into an object, separating the code that requests an action from the code that actually performs it.**

Because requests become objects, they can be:

```text
Queued
Stored
Scheduled
Logged
Retried
Undone
Sent remotely
Executed later
```

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

Classical Command flow:

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

Background Job version:

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
   ↓
Receiver
```

---

# Interview Questions

## Question 1

What is the primary purpose of Command Pattern?

**A)** Change an algorithm  
**B)** Encapsulate an operation or request as an object  
**C)** Convert incompatible interfaces  
**D)** Create object families  

**Answer: B**

---

## Question 2

In classical Command Pattern, which participant actually knows how to perform the business operation?

**A)** Invoker  
**B)** Client  
**C)** Receiver  
**D)** Factory  

**Answer: C**

---

## Question 3

Which participant typically triggers `Execute()`?

**A)** Invoker  
**B)** Receiver  
**C)** Factory  
**D)** Adapter  

**Answer: A**

---

## Question 4

Which is a classic use case for Command Pattern?

**A)** Undo / Redo  
**B)** Queued operations  
**C)** Delayed execution  
**D)** All of the above  

**Answer: D**

---

## Question 5

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

**Answer: B**

---

## Question 6

What is the main difference between Strategy and Command?

**A)** Strategy creates objects while Command converts interfaces.  
**B)** Strategy represents how an operation is performed, while Command represents what operation should be performed.  
**C)** They are identical.  
**D)** Command can only be used in UI applications.  

**Answer: B**

---

## Question 7

Which statement about RabbitMQ is correct?

**A)** Every RabbitMQ message is a Command.  
**B)** RabbitMQ cannot be used with Commands.  
**C)** `GenerateStatementCommand` can represent a Command while `StatementGeneratedEvent` represents an Event.  
**D)** Commands and Events are identical.  

**Answer: C**

---

## Question 8

Why are MediatR Commands often associated with Command Pattern?

**A)** Every MediatR request uses `Execute()`.  
**B)** An operation is represented as an object and dispatched to a Handler, decoupling the sender from execution logic.  
**C)** MediatR creates database tables.  
**D)** Every Command implements Undo.  

**Answer: B**

---

## Question 9

Which statement best describes the relationship between CQRS and Command Pattern?

**A)** CQRS and Command Pattern are exactly the same.  
**B)** CQRS often models state-changing intentions as Command objects, but CQRS is a broader architectural pattern.  
**C)** Command Pattern requires CQRS.  
**D)** CQRS only contains Queries.  

**Answer: B**

---

# One-Sentence Definition

> **Command Pattern encapsulates a request as an object so the sender can be decoupled from the code that performs the operation, while also enabling queueing, delayed execution, retry, logging, scheduling and Undo/Redo scenarios.**
