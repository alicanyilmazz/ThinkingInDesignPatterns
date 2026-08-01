# DesignPatterns

```diff
@@ Adapter Pattern @@

+ Önce problemi anlayalım

+ Şöyle düşün.

+ Senin uygulaman sadece şu interface'i biliyor.
```

```c#
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

```diff
@@ Uygulamanın her yeri bunu kullanıyor. @@
```

```c#
_paymentService.Pay(100);
```

```diff
@@ Sonra bir firma sana SDK gönderiyor. @@
@@ Adamların kodu şu. @@
```

```c#
public class LegacyBankApi
{
    public void ExecutePayment(decimal amount)
    {
        Console.WriteLine("Legacy payment");
    }
}
```

```diff
@@ Adamların 'ExecutePayment()' diye metodu var.@@
@@ Bizim sistem 'Pay()' bekliyor. Uyumsuz.@@
```
__________________________________________
```diff
@@ Şimdi ne yapacağız? @@

❌ Legacy kodunu değiştiremiyoruz.

+ Çünkü
✅ Nuget paketi olabilir.
✅ DLL olabilir.
✅ Başka firma yazmıştır.
Bizim sistemi de değiştiremiyoruz.

Çünkü binlerce yerde Pay() cagırılıyor. İŞte burada Adapter Pattern devreye giriyor.
```
__________________________________________
```diff
@@ Adapter ne yapıyor? @@
```
```c#
Bizim sistem
↓
Pay()
↓
Adapter
↓
ExecutePayment()
↓
Legacy API
```
```c#
public interface IPaymentService
{
    void Pay(decimal amount);
}

public class LegacyBankApi
{
    public void ExecutePayment(decimal amount)
    {
        Console.WriteLine(
            $"Legacy payment : {amount}");
    }
}

// Adapter
public class LegacyBankAdapter : IPaymentService
{
    private readonly LegacyBankApi _legacyApi;

    public LegacyBankAdapter(LegacyBankApi legacyApi)
    {
        _legacyApi = legacyApi;
    }

    public void Pay(decimal amount)
    {
        _legacyApi.ExecutePayment(amount);
    }
}
```
```diff
@@ Kullanımı @@
```
```c#
IPaymentService paymentService = new LegacyBankAdapter(new LegacyBankApi());

paymentService.Pay(500);
```
