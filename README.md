# DesignPatterns
```diff
@@ Strategy Pattern @@

- Strategy Pattern şu problemi çözer:

@@ Aynı işi yapan fakat farklı şekilde çalışan birden fazla algoritmamız varsa, her algoritmayı ayrı sınıfa koyarız ve çalışma sırasında hangisinin kullanılacağını seçeriz. @@

```
```
Örneğin ATM’de para çekme komisyonu hesaplanacak:

Visa için %2
MasterCard için %3
Troy için %1
```
```diff
@@ Strategy kullanmadan önce @@
```

```c#
public decimal CalculateCommission(
    string cardType,
    decimal amount)
{
    if (cardType == "Visa")
    {
        return amount * 0.02m;
    }

    if (cardType == "MasterCard")
    {
        return amount * 0.03m;
    }

    if (cardType == "Troy")
    {
        return amount * 0.01m;
    }

    throw new NotSupportedException(
        $"Desteklenmeyen kart tipi: {cardType}");
}
```
```diff
@@ Bu kod çalışır. Fakat yeni bir kart türü geldiğinde bu metodu değiştirmek zorundayız: @@
```

```c#
if (cardType == "Amex")
{
    return amount * 0.04m;
}
```

```diff
@@ Her yeni algoritmada mevcut kodu değiştiriyoruz. Bu durum Open/Closed Principle açısından iyi değildir. @@
```
__________________________________________

```diff
@@ Strategy çözümü @@
@@ İlk olarak bütün komisyon algoritmalarının uyacağı ortak bir interface oluştururuz: @@
```

```c#
public interface ICommissionStrategy
{
    decimal Calculate(decimal amount);
}
```

```diff
@@ Bu interface şunu söyler: @@
@@ Her komisyon stratejisinin Calculate metodu olmak zorundadır. @@
```
```diff
@@ Visa algoritması @@
```
```c#
public sealed class VisaCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.02m;
    }
}
```
```diff
@@ MasterCard algoritması @@
```
```c#
public sealed class MasterCardCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.03m;
    }
}
```
```diff
@@ Troy algoritması @@
```
```c#
public sealed class TroyCommissionStrategy : ICommissionStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.01m;
    }
}
```
```diff
@@ Artık her hesaplama algoritması ayrı bir sınıfta bulunuyor. @@
```
__________________________________________

```diff
@@ Context sınıfı @@
@@ Strategy Pattern’de algoritmayı kullanan sınıfa genellikle Context denir. @@
@@ Bizim örneğimizde bu sınıf CommissionCalculator: @@
```
```c#
public sealed class CommissionCalculator
{
    private readonly ICommissionStrategy _commissionStrategy;

    public CommissionCalculator(
        ICommissionStrategy commissionStrategy)
    {
        _commissionStrategy = commissionStrategy;
    }

    public decimal Calculate(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Tutar sıfırdan büyük olmalıdır.");
        }

        return _commissionStrategy.Calculate(amount);
    }
}
```
```diff
@@ Buradaki önemli nokta: @@
```

```c#
private readonly ICommissionStrategy _commissionStrategy;
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
