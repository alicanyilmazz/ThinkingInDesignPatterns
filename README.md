# DesignPatterns

```diff
@@ Chain of Responsibility (CoR) @@

Bu pattern'i anlarsan şunları da anlarsın:

✅ ASP.NET Core Middleware
✅ MediatR Pipeline Behavior
✅ Authentication Pipeline
✅ Authorization Pipeline
✅ Exception Middleware
✅ Validation Pipeline
✅ HTTP Pipeline
✅ ATM işlem akışları

Bu yüzden mülakatlarda çok sorulur.
```
__________________________________________

```diff
@@ Önce problemi anlayalım @@

ATM'de para çekme isteği geldi.

İşlem başlamadan önce sırayla şunlar çalışıyor.
```
```
Kart Takıldı
↓
Kart Geçerli mi?
↓
PIN Doğru mu?
↓
Kart Blokeli mi?
↓
Günlük Limit
↓
Bakiye
↓
Fraud Kontrolü
↓
ATM'de Para Var mı?
↓
Para Ver

```

```diff
@@ Şimdi bunu tek method yazsak @@
```

```c#
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
Çalışır. Ama problem şu.

Yarın diyorlar ki

- AML kontrolü de gelsin.

Kod değişti.

Sonra

- Blacklist kontrolü.

Kod yine değişti.

Sonra

- QR Withdraw ise PIN kontrolünü atla.

Kod yine değişti.

Method büyümeye başladı.
```
__________________________________________
```c#
```

```diff
@@ Chain ne diyor? @@

Her kontrol

ayrı sınıf olsun.
```

```
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

```diff
Her biri

işini yapacak.

Sonra

bir sonrakine geçecek.
```
__________________________________________
```diff
@@ Önce Request @@
```
```c#
public class WithdrawRequest
{
    public string CardNumber { get; set; }

    public string Pin { get; set; }

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }
}
```
__________________________________________
```diff
@@ Handler Interface @@
```

```c#
public interface IHandler
{
    void SetNext(IHandler handler);

    void Handle(WithdrawRequest request);
}
```

```diff
@@ Burada iki metod var. @@

SetNext()

Handle()
```
__________________________________________

```diff
@@ Base Handler @@
Her handler aynı kodu yazmasın.
```
```c#
public abstract class Handler : IHandler
{
    private IHandler _next;

    public void SetNext(IHandler handler)
    {
        _next = handler;
    }

    public virtual void Handle(WithdrawRequest request)
    {
        _next?.Handle(request);
    }

```
```diff
@@ Bak burada en önemli satır @@
```
```c#
_next?.Handle(request);
```
```c#
Bu zinciri devam ettiriyor.
```
__________________________________________
```diff
@@ Card Handler @@
```

```c#
public class CardValidationHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("Kart doğrulandı.");

        base.Handle(request);
    }
}
```

> Bak işini yaptı. Sonra

```c#
base.Handle(request);
```

> dedi. Yani zincirin devamı.
__________________________________________
```diff
@@  @@
```
```c#
```
__________________________________________
```diff
@@ PIN Handler @@
```

```c#
public class PinValidationHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("PIN doğrulandı.");

        base.Handle(request);
    }
}
```
__________________________________________
```diff
@@ Balance Handler @@
```

```c#
public class BalanceHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        if(request.Balance < request.Amount)
        {
            Console.WriteLine("Yetersiz bakiye.");

            return;
        }

        Console.WriteLine("Bakiye uygun.");

        base.Handle(request);
    }
}
```

```diff
@@ Bak burada @@
```

```c#
return;
```
dedi.

> Ne oldu? Zincir bitti. Sonraki handler çalışmadı. 

> Bu çok önemli.
__________________________________________
```diff
@@ Cash Handler @@
```

```c#
public class CashHandler : Handler
{
    public override void Handle(WithdrawRequest request)
    {
        Console.WriteLine("ATM parayı verdi.");

        base.Handle(request);
    }
}
```
__________________________________________
```diff
@@ Zinciri kuruyoruz @@
```

```c#
var card = new CardValidationHandler();

var pin = new PinValidationHandler();

var balance = new BalanceHandler();

var cash = new CashHandler();

card.SetNext(pin);

pin.SetNext(balance);

balance.SetNext(cash);
```

```diff
@@ Bak oluşan zincir @@
```

```c#
Card
↓
Pin
↓
Balance
↓
Cash

```
__________________________________________
```diff
@@ Çalıştırıyoruz @@
```

```c#
card.Handle(request);
```
Sadece

ilk handler'ı çağırıyoruz.

Gerisini

handler'lar birbirini çağırıyor.

__________________________________________
```diff
@@ Çalışma sırası @@
```

```c#
Card
↓
Pin
↓
Balance
↓
Cash
```
```diff
@@ Eğer bakiye yetersizse @@
Balance Handler
```

```c#
return;
```
```diff
@@ dedi. Şimdi Cash Handler çalışır mı? Hayır. Zincir orada biter. @@
```

__________________________________________
```diff
@@ İşte CoR'nin mantığı @@
Her handler

şunu der.
```

```
Benim işim buysa yaparım.

Sonra devam ederim.

İşlem burada bitecekse
devam ettirmem.
```
__________________________________________

```diff
@@ ASP.NET Core Middleware @@
Şimdi en önemli yer.

ASP.NET Core'da
```

```c#
app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.UseRouting();

app.UseEndpoints();
```
```diff
@@ Bu ne? Aslında @@
```

```c#
Middleware1
↓
Middleware2
↓
Middleware3
↓
Middleware4
```
```diff
@@ Her middleware @@
```

```c#
await next(context);
```
```diff
@@ diyor.Bu şunun aynısı. @@
```

```c#
base.Handle(request);
```
__________________________________________
```diff
@@ Mesela Authentication başarısız. @@
```

```c#
Authentication
↓
401 dön
↓
Pipeline biter
```
```diff
@@ Authorization çalışmaz.Bu Chain of Responsibility'dir. @@
```
__________________________________________
```diff
@@ MediatR Pipeline @@
Sen bunu çok kullandın.

```
```c#
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
```diff
@@ Her Behavior @@
```

```c#
await next();
```

der.Bu tam olarak Chain of Responsibility.

__________________________________________
```diff
@@ Exception Middleware @@
```

```c#
Exception Middleware
↓
Authentication
↓
Authorization
↓
Controller
```
Controller exception attı. Exception Middleware yakaladı.

Pipeline orada bitti.

__________________________________________
```diff
@@ Microservice örneği @@
API Gateway
```

```c#
JWT
↓
Rate Limit
↓
IP Filter
↓
Routing
↓
Microservice
```
__________________________________________

> Avantajları

✅ Handler'lar birbirinden bağımsızdır.

✅ Yeni handler eklemek kolaydır.

✅ Handler sırası değişebilir.

✅ Her handler tek sorumluluk taşır.
__________________________________________

> Dezavantaj

Handler sırası yanlış olursa

bug bulmak zor olabilir.

Mesela

Cash
↓
Balance

olursa para verdikten sonra bakiyeye bakmış olursun.

```c#
@@ Strategy ile fark @@

Strategy

bir algoritma seçer.

Visa

veya

Master

Chain

hepsini sırayla çalıştırır.

@@ Observer ile fark @@

Observer

Bir olay oldu

↓

Herkes haberdar olsun

Chain

Bir istek geldi

↓

Sırayla kontrol et
@@ Command ile fark @@

Command

bir işi temsil eder.

Chain

o işi işleyen handler zinciridir.

Mesela

WithdrawCommand
↓
Validation
↓
Fraud
↓
Cash
↓
Journal

@@ Decorator ile fark @@

Bu çok sorulur.

Decorator

Logging
↓
Retry
↓
Payment

Hepsi çalışır.

Chain

Validation

↓

Fraud

↓

Limit

Birisi

STOP

derse

devam etmez.
```

> Chain of Responsibility Pattern, bir isteği birden fazla handler'ın sırayla işlemesini sağlar. Her handler isterse isteği işler ve zinciri devam ettirir, isterse zinciri durdurur.

> ASP.NET Core Middleware neden Chain of Responsibility'dir?
> Çünkü her middleware request'i işler ve await next() çağırarak pipeline'ın devam etmesine karar verir. İsterse next() çağırmayıp pipeline'ı sonlandırabilir.

```
1)

Chain of Responsibility'nin temel amacı nedir?

A) Algoritma değiştirmek

B) İsteği sırayla birden fazla handler'ın işlemesi

C) Nesne üretmek

D) Interface çevirmek

2)

Handler zinciri hangi durumda durur?

A) İlk handler çalışınca

B) Son handler çalışınca

C) Bir handler isteği sonlandırıp devam ettirmezse

D) Her zaman tüm handler'lar çalışır

3)

ASP.NET Core'da aşağıdakilerden hangisi Chain of Responsibility örneğidir?

A) Dependency Injection

B) Middleware Pipeline

C) AutoMapper

D) Entity Framework

4)

await next() hangi anlama gelir?

A) Yeni nesne oluştur.

B) Bir sonraki handler'ı çalıştır.

C) Event yayınla.

D) Transaction başlat.

5)

ATM'de aşağıdaki akış hangi pattern'e örnektir?

Kart Kontrolü
      ↓
PIN Kontrolü
      ↓
Limit Kontrolü
      ↓
Fraud Kontrolü
      ↓
Para Ver

A) Strategy

B) Factory

C) Chain of Responsibility

D) Observer

Doğru Cevaplar
B
C
B
B
C
```
