# DesignPatterns

__________________________________________


```diff
@@ Command Pattern @@

Bence bu, Observer'dan sonra öğrenilmesi gereken en önemli pattern. Çünkü:

CQRS'nin temel mantığını anlamanı sağlar.
MediatR'ın neden IRequest kullandığını anlarsın.
Undo/Redo mantığını anlarsın.
Queue sistemlerini anlarsın.
Hangfire, Quartz, RabbitMQ gibi sistemleri daha iyi kavrarsın.
ATM ve bankacılık projelerinde çok kullanılır.

@@ Önce problemin ne olduğunu anlayalım @@

ATM'de kullanıcı Para Çek butonuna bastı.

Kötü tasarım:
```

```c#
public class AtmScreen
{
    public void Withdraw(decimal amount)
    {
        _cardService.Validate();

        _balanceService.Check();

        _cashService.Dispense();

        _journal.Write();
    }
}
```
```diff
@@ Şimdi başka butonlar geldi. @@

Para Çek
Para Yatır
Bakiye Sorgu
PIN Değiştir
Kart İade

Sonunda
```

```c#
if(button=="Withdraw")
{
   ...
}

if(button=="Deposit")
{
   ...
}

if(button=="Balance")
{
   ...
}
```
```diff
@@ oluşmaya başladı. @@
```

```diff
@@ Command ne diyor? @@
Her buton ayrı bir nesne olsun.

Yani
WithdrawCommand
DepositCommand
BalanceCommand
PinChangeCommand
CardReturnCommand
Hepsi ortak interface'i implement etsin.

@@ Interface @@
```
```c#
public interface ICommand
{
    void Execute();
}
```
```c#
// Hepsi

Execute()

// metoduna sahip.
```
__________________________________________
```diff
@@ Withdraw Command @@
```

```c#
public class WithdrawCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Withdraw executed");
    }
}
```
```diff
@@ Deposit Command @@
```

```c#
public class DepositCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Deposit executed");
    }
}
```
```diff
@@ Balance Command @@
```

```c#
public class BalanceCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Balance executed");
    }
}
```
__________________________________________
```diff
@@ Client @@
```

```c#
ICommand command = new WithdrawCommand();
command.Execute();
```

```diff
@@ Bak dikkat et. Artık Withdraw() çağırmıyoruz. Execute() çağırıyoruz. @@ 
```
__________________________________________
```diff
@@ Ama bunun ne faydası var? @@
Henüz hiçbir şey.
Çünkü gerçek Command burada başlamıyor.
```
__________________________________________
```diff
@@ Receiver geliyor. @@
Mesela
@@ CashService @@
```
```c#
public class CashService
{
    public void Withdraw(decimal amount)
    {
        Console.WriteLine($"Cash : {amount}");
    }
}
```
```diff
@@ WithdrawCommand @@

kendi işi yapmaz.

Receiver'ı çağırır.
```

```c#
public class WithdrawCommand : ICommand
{
    private readonly CashService _cash;

    public WithdrawCommand(
        CashService cash)
    {
        _cash = cash;
    }

    public void Execute()
    {
        _cash.Withdraw(1000);
    }
}
```
```diff
@@ Bak Command iş yapmıyor. İşi Receiver yapıyor. @@
```
__________________________________________
```diff
@@ Neden böyle yaptık? @@

Çünkü artık ATM ekranı CashService'i bilmiyor.

Sadece @@ ICommand @@ biliyor
```
__________________________________________
```diff
@@ Invoker @@
Bir de Invoker var.

Mesela

ATM butonu.
```
```c#
public class Button
{
    private ICommand _command;

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
```diff
@@ Kullanım @@
```

```c#
var button =
    new Button(
        new WithdrawCommand(
            new CashService()));

button.Click();
```
```diff
@@ Akış @@
```
```diff
Button
↓
WithdrawCommand
↓
CashService
↓
Withdraw() 
```
__________________________________________
```diff
@@ Command Pattern'in parçaları @@
```
```c#
Client
↓
Invoker
↓
Command
↓
Receiver
```
__________________________________________
```diff
@@ ASP.NET Core örneği @@
```
```diff
- Şimdi geldik en önemli yere.
- Sen bunu çok gördün.
- MediatR.
- Mesela
```
```c#
public class CreateProductCommand : IRequest<Guid>
{
    public string Name { get; set; }

    public decimal Price { get; set; }
}
```
```diff
Bu aslında

Command'dır.

Handler
```

```c#
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand,Guid>
{
    public async Task<Guid> Handle(...)
    {
        ...
    }
}
```
```diff
@@ Burada @@
```
```diff
CreateProductCommand
↓
Handler
↓
Repository
↓
Save

Bu aslında GoF Command Pattern'in modern hali.
```
__________________________________________


```diff
@@ CQRS  @@
Neden
```
```c#
CreateProductCommand
```
```diff
@@ diyoruz? Niye @@
```
```c#
CreateProductRequest
```
```diff
@@ demiyoruz? Çünkü Command bir niyeti temsil eder. Ürün oluştur. @@
```
```diff
@@ RabbitMQ @@

Mesela

WithdrawCommand
↓
RabbitMQ
↓
Consumer
↓
CashService

Bu da Command.

@@ Hangfire @@
SendMailCommand
↓
Queue
↓
1 saat sonra Execute()

Yine Command.
```
```c#
Undo

En meşhur örnek.

Word.

Ctrl + Z

Her işlem

bir command.

BoldCommand

ItalicCommand

DeleteCommand

Hepsinde

Undo()

olabilir.

public interface ICommand
{
    void Execute();

    void Undo();
}
```
```diff
@@ Avantajları @@

✅ İş isteğini nesne haline getirir.

✅ Queue'ya koyabilirsin.

✅ Loglayabilirsin.

✅ Retry yapabilirsin.

✅ Undo yapabilirsin.

✅ Serialize edebilirsin.

@@ Dezavantaj @@

Çok fazla command oluşabilir.
```
```diff
@@  @@
```
```c#
```
```diff
@@ Strategy ile fark @@

Strategy

Nasıl yapılacak?

Command

Ne yapılacak?
@@ Observer ile fark @@

Observer

Bir olay oldu.

Herkes haberdar olsun.

Command

Git bunu yap.
@@ Facade ile fark @@

Facade

Birçok servisi tek metod altında toplar.

Command

Bir işi nesne haline getirir.
@@ Factory ile fark @@

Factory

nesne üretir.

Command

iş isteğini temsil eder.
```

> Command Pattern, yapılacak işi (request) bir nesne haline getirerek, bu isteğin farklı zamanlarda, farklı yerlerde çalıştırılmasını sağlar.

> Mülakatta çok sorulan soru

> MediatR neden Command Pattern olarak kabul edilir?

> Cevap:

> Çünkü CreateOrderCommand gibi istekler bir işi temsil eden nesnelerdir. Bu nesneler handler tarafından çalıştırılır. Command ile işi isteyen (sender) ile işi yapan (handler) birbirinden ayrılır.

__________________________________________
1)

Command Pattern'in temel amacı nedir?

A) Algoritma değiştirmek

B) Yapılacak işi nesne haline getirmek

C) Interface çevirmek

D) Nesne üretmek

2)

Command Pattern'de işi gerçekten yapan kimdir?

A) Invoker

B) Client

C) Receiver

D) Factory

3)

MediatR'daki CreateProductCommand hangi pattern'e örnektir?

A) Strategy

B) Observer

C) Command

D) Adapter

4)

Hangisi Command Pattern için uygun örnektir?

A) Undo/Redo

B) Hangfire Job

C) RabbitMQ Consumer

D) Hepsi

5)

Command Pattern'de butona basıldığında tipik akış hangisidir?

A)

Button
↓

Receiver
↓

Command

B)

Button
↓

Command
↓

Receiver

C)

Receiver
↓

Button
↓

Command

D)

Factory
↓

Command
↓

Receiver

__________________________________________
Doğru cevaplar
B
C
C
D
B
