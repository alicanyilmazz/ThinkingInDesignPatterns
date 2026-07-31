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
@@  @@
```

```c#

```

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
