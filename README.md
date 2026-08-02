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
```
```diff
@@  @@
```
```diff
@@  @@
```
```c#
```
```diff
@@  @@
```
```diff
@@  @@
```
```c#
```
```diff
@@  @@
```
```diff
@@  @@
```
```c#
```
```diff
@@  @@
```
```diff
@@  @@
```
```c#
```
```diff
@@  @@
```
```diff
@@  @@
```
```c#
```
```diff
@@  @@
```

```c#
```
