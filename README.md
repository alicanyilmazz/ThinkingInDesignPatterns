# DesignPatterns

```diff
- Decorator Pattern
```

```diff
@@ Elimizde bir service var @@
```

```c#
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

```diff
@@ Gerçek implementasyon: @@
```

```c#
public class PaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Payment completed : {amount}");
    }
}
```
```diff
@@ Kullanımı: @@
```
```c#
IPaymentService paymentService = new PaymentService();

paymentService.Pay(100);
```

```diff
@@ Çıktı @@
```
```diff
+ Payment completed : 100
```
```diff
@@Sonra müşteri diyor ki @@

✅ Her ödeme öncesi log atalım.

✅ Tamam.

✅ Sonra

✅ Cache ekleyelim.

✅ Sonra

✅ Authorization olsun.

✅ Sonra

✅ Retry olsun.

✅ Sonra

✅Performance ölçelim.

-Şimdi ne yapacağız?
```
___________________________________
```diff
- Kötü çözüm
- Her şeyi PaymentService'in içine koymak.
```
```c#
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
@@ Bu artık @@
❌ Single Responsibility Principle'yi bozuyor.

Çünkü PaymentService artık
* ödeme yapıyor
* log atıyor
* cache yönetiyor
* retry yapıyor
* authorization yapıyor
```
___________________________________
```diff
@@ Decorator ne diyor? @@
+ Asıl sınıfa dokunma.
+ Üzerine yeni davranış ekle.
+ Yani
```

```c#
Logging
      ↓
Caching
      ↓
Authorization
      ↓
PaymentService
```

```diff
@@ Katman katman. @@
```
__________________________________________

```diff
@@ İlk Decorator @@
```
```diff
- Interface
```

```c#
public interface IPaymentService
{
    void Pay(decimal amount);
}
```
```diff
- Asıl servis
```
```c#
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
```c#
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
- Kullanımı
```
```c#
IPaymentService payment = new LoggingPaymentDecorator(new PaymentService());
payment.Pay(500);
```
```diff
@@ Çıktı @@
```

```log
Log started

Payment : 500

Log finished
```
__________________________________________

```diff
@@ İkinci Decorator @@
+ Mesela Authorization
```

```c#
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
@@ Şimdi @@
```
```c#
IPaymentService payment = new AuthorizationDecorator(new LoggingPaymentDecorator(new PaymentService()));
```
```diff
@@ Çalışma sırası @@
```

```
Authorization

↓

Logging

↓

PaymentService
```
```diff
@@ İstersen @@
```
```
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

```diff
@@ şeklinde 20 tane bile ekleyebilirsin. @@
```
__________________________________________
```diff
@@ Zincir nasıl çalışıyor? @@
```
```
Pay()

↓

AuthorizationDecorator

↓

LoggingDecorator

↓

PaymentService
```
```diff
✅ Decorator'ın en önemli mantığı budur.
✅ Her decorator aynı interface'i implement eder.
✅ İçinde yine aynı interface tutulur.
```

```
Decorator

↓

IPaymentService

↓

PaymentService
```
__________________________________________
```diff
@@ En önemli özellik @@
- Decorator aynı interface'i implement eder.
+ Bak
```
```c#
public class LoggingDecorator : IPaymentService
```
```diff
@@ ve içinde @@
```
```c#
private readonly IPaymentService _paymentService;
```
```diff
@@ vardır. Yani hem IPaymentService gibi davranır. Hem IPaymentService tutar. Bu pattern'in özü budur.@@
```
__________________________________________
```diff
@@  @@
```

```c#

```


```diff
@@ Avantajları @@
✅ Constructor karmaşasını kaldırır.
✅ Okunabilirliği artırır.
✅ Fluent API sağlar.
✅ Immutable class'larda çok kullanılır.
✅ Validation eklenebilir.
```
__________________________________________
```diff
@@ Dezavantajları @@
❌ Küçük sınıflar için gereksiz olabilir.
❌ Her model için ekstra Builder sınıfı yazılır.
❌ Çok basit nesnelerde new kullanmak daha pratiktir.
```
