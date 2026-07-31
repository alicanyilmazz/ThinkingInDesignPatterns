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

```diff
@@  @@
```

```diff
@@  @@
```

```c#

```
__________________________________________
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
