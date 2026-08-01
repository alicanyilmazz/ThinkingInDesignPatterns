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
```diff
@@ Decorator ile farkı @@
```
```
Decorator

davranış ekler.

Logging

↓

Payment

Adapter

davranış eklemez.

Sadece

arayüzü değiştirir.

Pay()

↓

ExecutePayment()
```
```diff
@@ Facade ile farkı @@
```
```
Bu çok sorulur.

Adapter uyumsuz iki sistemi uyumlu hale getirir.

Facade karmaşık sistemi basitleştirir.

Mesela 20 metod var.

Facade tek metod sunar.
```
```diff
@@ Strategy ile farkı @@
```
```
Strategy algoritma değiştirir.

Visa 
Master
Troy

Adapter algoritma değiştirmez.

Sadece interface çevirir.
```
```diff
Avantajları

✅ Legacy sistemi değiştirmeyiz.

✅ Yeni sistemi değiştirmeyiz.

✅ Bağımlılığı azaltır.

✅ Interface uyumsuzluğunu çözer.

Dezavantaj

❌ Çok fazla adapter olursa class sayısı artar.
```
```
1)

Adapter'ın temel amacı nedir?

A) Yeni nesne üretmek

B) Uyumsuz interface'leri uyumlu hale getirmek

C) Algoritmayı değiştirmek

D) Logging eklemek

2)

Hangisi Adapter için en uygun örnektir?

A) SOAP → REST

B) USB-C → HDMI

C) Vendor SDK → Ortak Interface

D) Hepsi

3)

Adapter hangi SOLID prensibini en çok destekler?

A) OCP

B) DIP

C) SRP

D) LSP

4)

Decorator ile Adapter arasındaki temel fark nedir?

A) İkisi aynıdır.

B) Decorator davranış ekler, Adapter interface çevirir.

C) Adapter performans artırır.

D) Decorator nesne üretir.

5)

ATM'de NCR SDK'sında

ReadTrackData()

ama sistem

ReadCard()

bekliyorsa hangi pattern uygundur?

A) Builder

B) Strategy

C) Adapter

D) Factory

Doğru cevaplar
B D B B C
```
