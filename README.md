# DesignPatterns

```diff
@@ Facade Pattern @@
```
```diff
- Önce problemi anlayalım

Bir ATM'de para çekme işlemi düşün.

Bir kullanıcı 1000 TL çek dedi.

Aslında arkada tek işlem olmuyor.

Şunların hepsi çalışıyor:
```
```c#
Kart doğrulama
↓
PIN doğrulama
↓
Bakiye kontrolü
↓
Limit kontrolü
↓
Fraud kontrolü
↓
Kasette para var mı?
↓
Banknot dağıt
↓
Journal yaz
↓
Host'a mesaj gönder
```
```diff
@@ Toplam belki 20 servis. @@
@@ Şimdi Controller'ın bunların hepsini tek tek çağırdığını düşün. @@
```
```c#
public class WithdrawController
{
    public void Withdraw()
    {
        cardValidator.Validate();

        pinService.Validate();

        balanceService.Check();

        limitService.Check();

        fraudService.Check();

        cashService.Dispense();

        journalService.Write();

        receiptService.Print();

        logService.Log();
    }
}
```
```diff
Çalışıyor.
Ama Controller
hepsini biliyor.
Bu iyi mi?
Hayır.
```

```diff
@@ Problem ne? @@  

Controller 20 farklı servise bağımlı.
Yarın Fraud değişti.
Controller değişiyor.
Receipt değişti.
Controller değişiyor.
Journal değişti.
Controller değişiyor.
Yani
Controller çok fazla şey biliyor.
```

```diff
@@ Facade ne diyor? @@
@@ Hepsini tek bir servis altında topla. @@
```

```c#
WithdrawController
↓
WithdrawFacade
↓
CardValidator
↓
PinValidator
↓
BalanceService
↓
FraudService
↓
CashService
↓
JournalService
↓
ReceiptService
```
```diff
@@ Controller artık yalnızca Facade'ı bilir. @@
```
__________________________________________

```diff
@@ İlk örnek @@
```
```diff
@@ Kart servisi @@
```
```c#
public class CardService
{
    public void Validate()
    {
        Console.WriteLine("Card OK");
    }
}
```
```diff
@@ PIN @@
```
```c#
public class PinService
{
    public void Validate()
    {
        Console.WriteLine("PIN OK");
    }
}
```
```diff
@@ Cash @@
```
```c#
public class CashService
{
    public void Dispense(decimal amount)
    {
        Console.WriteLine($"Dispensed {amount}");
    }
}
```
```diff
@@ Receipt @@
```
```c#
public class ReceiptService
{
    public void Print()
    {
        Console.WriteLine("Receipt");
    }
}
```
_________________________________________
```diff
@@ Şimdi Facade @@
```
```c#
public class WithdrawFacade
{
    private readonly CardService _card;

    private readonly PinService _pin;

    private readonly CashService _cash;

    private readonly ReceiptService _receipt;

    public WithdrawFacade(
        CardService card,
        PinService pin,
        CashService cash,
        ReceiptService receipt)
    {
        _card = card;

        _pin = pin;

        _cash = cash;

        _receipt = receipt;
    }

    public void Withdraw(decimal amount)
    {
        _card.Validate();

        _pin.Validate();

        _cash.Dispense(amount);

        _receipt.Print();
    }
}
```
```diff
@@ Kullanımı @@
- Eskiden
```
```diff
@@ controller şunu yapıyordu @@
```
```c#

card.Validate();

pin.Validate();

cash.Dispense();

receipt.Print();
```
```diff
@@ Şimdi @@
```
```c#
_facade.Withdraw(1000);
```
__________________________________________

```diff
@@ En önemli mantık @@
- Facade iş yapmaz.
- Servisleri organize eder.
- Bak
```
```c#
_card.Validate();

_pin.Validate();

_cash.Dispense();

_receipt.Print();
```
```diff
- Kendisi doğrulama yapmıyor.

- Kendisi para dağıtmıyor.

- Sadece orkestra şefi gibi.
```
__________________________________________

```diff
@@ Gerçek ASP.NET Core örneği @@
- Mesela Order oluşturuyoruz.
- Controller eskiden
```
```c#
customerService.Validate();

stockService.Check();

paymentService.Pay();

invoiceService.Create();

mailService.Send();

logService.Log();
```
```diff
@@ Şimdi @@
```
```c#
_orderFacade.CreateOrder(request);
```
```diff
- Facade içeride hepsini çağırıyor.
```
__________________________________________
```diff
@@ Gerçek Framework örneği @@
- ASP.NET Core'da mesela Identity şunu yaparsın
```

```c#
await _signInManager.PasswordSignInAsync(...)
```
```diff
@@ Arkada 20 işlem olur. @@

Cookie 
Claims
Security Stamp
Identity Validation
Log
Hepsini @@ SignInManager @@ yönetir.
Aslında @@ SignInManager @@ Facade gibi davranır.
```
__________________________________________
```diff
@@ Microservice örneği @@
Checkout
```

```c#
Order Service
↓
Payment Service
↓
Inventory Service
↓
Shipping Service
↓
Notification Service
```
```diff
@@ Frontend 5 servisi çağırmak yerine @@
```
```c#
Checkout Facade
↓
5 servis
```
```diff
@@ çağırır @@
```

```diff
@@ Avantajları @@

✅ Controller sadeleşir.
✅ Karmaşıklık azalır.
✅ Servisleri değiştirmek kolay olur.
✅ Client

20 servis yerine

1 servis bilir.

@@ Dezavantaj @@

❌Facade büyüyebilir.

WithdrawFacade
↓
5000 satır

olursa

❌ God Class olur.
```
