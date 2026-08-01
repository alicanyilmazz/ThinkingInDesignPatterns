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
=> @@ Strategy Pattern’de algoritmayı kullanan sınıfa genellikle "Context" denir. @@
@@ Bizim örneğimizde bu sınıf CommissionCalculator: @@
```
```c#
public sealed class CommissionCalculator
{
    private readonly ICommissionStrategy _commissionStrategy;

    public CommissionCalculator(ICommissionStrategy commissionStrategy)
    {
        _commissionStrategy = commissionStrategy;
    }

    public decimal Calculate(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount),"Tutar sıfırdan büyük olmalıdır.");
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
```diff
- CommissionCalculator, Visa veya MasterCard sınıfını doğrudan bilmiyor.

- Sadece şu interface’i biliyor:
```
```c#
ICommissionStrategy
```

```diff
@@ Ve hesaplama yaparken: @@
```
```c#
_commissionStrategy.Calculate(amount);
```

```diff
@@ diyor. Hesabın nasıl yapıldığını bilmiyor. O işi kendisine verilen strategy yapıyor. @@
```
```diff
@@ Kullanımı @@
@@ Visa için @@
```

```c#
ICommissionStrategy strategy = new VisaCommissionStrategy();

var calculator = new CommissionCalculator(strategy);

decimal commission = calculator.Calculate(1000);

Console.WriteLine(commission); // 20
```
```diff
@@ Akış: @@
```
```
CommissionCalculator.Calculate(1000)
                ↓
VisaCommissionStrategy.Calculate(1000)
                ↓
               20
```
```diff
@@ MasterCard için @@
```

```c#
ICommissionStrategy strategy = new MasterCardCommissionStrategy();

var calculator = new CommissionCalculator(strategy);

decimal commission = calculator.Calculate(1000);

Console.WriteLine(commission); // 30

```

```diff
@@ Burada CommissionCalculator değişmedi. @@
@@ Sadece verdiğimiz strategy değişti @@
```
```c#
new VisaCommissionStrategy()
```
```diff
@@ yerine @@
```
```c#
new MasterCardCommissionStrategy()
```
```diff
@@ verdik. Strategy Pattern’in ana mantığı tam olarak budur.@@
```

```diff
@@ Strategy’nin parçaları @@
@@ Örneğimizde dört ana parça vardır: @@

@@ 1.) Strategy interface @@
@@ Bütün algoritmaların sözleşmesi: @@
```
```c#
ICommissionStrategy
```
```diff
@@ 2.) Concrete Strategy @@
@@ Gerçek algoritmalar: @@
```
```c#
VisaCommissionStrategy
MasterCardCommissionStrategy
TroyCommissionStrategy
```
```diff
@@ 3.) Context @@
@@ Algoritmayı kullanan sınıf: @@
```
```c#
CommissionCalculator
```
```diff
@@ 4.) Client @@
@@ Hangi strategy’nin kullanılacağını seçen taraf: @@
```
```c#
new CommissionCalculator(new VisaCommissionStrategy());
```

```diff
Strategy switch’i tamamen yok eder mi?
Her zaman değil.
Bir yerde kart tipine göre strategy seçmemiz gerekebilir:
```
```c#
ICommissionStrategy strategy = cardType switch
{
    CardType.Visa => new VisaCommissionStrategy(),
    CardType.MasterCard => new MasterCardCommissionStrategy(),
    CardType.Troy => new TroyCommissionStrategy(),
    _ => throw new NotSupportedException()
};
```
```diff
Buradaki fark şudur:

Eskiden bütün iş kuralları tek switch içindeydi:

return amount * 0.02m;
return amount * 0.03m;
return amount * 0.01m;

Şimdi switch yalnızca doğru strategy’yi seçiyor. Algoritmalar ayrı sınıflarda bulunuyor.

Bu seçim işlemi daha sonra Factory veya Dependency Injection ile de yapılabilir.
```
__________________________________________
```diff
@@ Factory ile farkı @@

Strategy:

Hesaplama nasıl yapılacak?

Visa komisyon algoritması
MasterCard komisyon algoritması
Troy komisyon algoritması

Factory:

Hangi strategy oluşturulacak veya seçilecek?

Kart Visa ise VisaCommissionStrategy seç.
Kart Troy ise TroyCommissionStrategy seç.

Kısaca:

Factory seçer.
Strategy işi yapar.
```
__________________________________________
```diff
@@ Decorator ile farkı @@

Strategy, mevcut davranışlardan birini seçer:

Visa veya MasterCard veya Troy

Decorator, mevcut davranışın çevresine ek davranış koyar:

Logging
   ↓
Retry
   ↓
PaymentService

Strategy’de çoğunlukla alternatiflerden biri çalışır.

Decorator’da davranışlar katmanlar halinde birlikte çalışabilir.
```
