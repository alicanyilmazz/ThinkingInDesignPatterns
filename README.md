# DesignPatterns

__________________________________________


```diff
@@ State Pattern @@

Bu pattern'i anlarsan şunları çok rahat anlarsın:

ATM işlem akışları
Sipariş durumları
Workflow Engine
State Machine (Automatonymous)
Saga
TCP Connection
Document Approval Flow

Aslında State Pattern = Duruma göre davranış değiştirme demektir
```
__________________________________________


```diff
@@ Önce problemi anlayalım @@

Bir sipariş düşün.

İlk oluşturuldu.

- Pending

Sonra ödendi.

- Paid

Sonra kargoya verildi.

- Shipped

Sonra teslim edildi.

- Completed

Şimdi şöyle bir kod yazsak
```

```c#
public class Order
{
    public OrderStatus Status { get; set; }

    public void Cancel()
    {
        if(Status == OrderStatus.Pending)
        {
            Console.WriteLine("Sipariş iptal edildi.");
        }

        if(Status == OrderStatus.Paid)
        {
            Console.WriteLine("Refund başlat.");
        }

        if(Status == OrderStatus.Shipped)
        {
            Console.WriteLine("İptal edilemez.");
        }

        if(Status == OrderStatus.Completed)
        {
            Console.WriteLine("İşlem tamamlandı.");
        }
    }
}
```


```diff
@@ Bugün çalışıyor. Yarın @@

Returned

Refunded

Rejected

Cancelled

geldi. if'ler büyümeye başladı.
```
__________________________________________

```diff
@@ State ne diyor? @@
Her state ayrı class olsun.
```
```c#
PendingState

PaidState

ShippedState

CompletedState
```


```diff
@@ Her biri aynı interface'i implement etsin. @@
```
__________________________________________

```diff
@@ Interface @@
```

```c#
public interface IOrderState
{
    void Pay(OrderContext context);

    void Ship(OrderContext context);

    void Cancel(OrderContext context);
}
```
__________________________________________

```diff
@@ Context @@
State Pattern'in en önemli class'ı.
```

```c#
public class OrderContext
{
    public IOrderState State { get; set; }

    public OrderContext(IOrderState state)
    {
        State = state;
    }

    public void Pay()
    {
        State.Pay(this);
    }

    public void Ship()
    {
        State.Ship(this);
    }

    public void Cancel()
    {
        State.Cancel(this);
    }
}
```

```diff
@@ Bak Context hangi state olduğunu biliyor. Ama nasıl davranacağını bilmiyor. @@
```
```diff
@@ Pending State @@
```
```c#
public class PendingState : IOrderState
{
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Ödeme alındı.");

        context.State = new PaidState();
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Henüz ödeme alınmadı.");
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Sipariş iptal edildi.");
    }
}
```

```diff
@@ En önemli satır @@
```

```c#
context.State = new PaidState();
```

```diff
@@ State değişti. @@
```
```diff
@@ Paid State @@
```
```c#
public class PaidState : IOrderState
{
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Zaten ödendi.");
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Kargoya verildi.");

        context.State = new ShippedState();
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Refund başlatıldı.");
    }
}
```

```diff
@@ Shipped State @@
```

```c#
public class ShippedState : IOrderState
{
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Zaten ödendi.");
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Zaten kargoda.");
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("İptal edilemez.");
    }
}
```

```diff
@@ Kullanımı @@
```

```c#
var order = new OrderContext(new PendingState());

order.Pay();

order.Ship();

order.Cancel();
```

```diff
@@ Çalışma sırası @@
```

```c#
Pending
↓
Pay()
↓
Paid
↓
Ship()
↓
Shipped
↓
Cancel()
↓
İptal edilemez.
```

```diff
@@ Bak dikkat et. Aynı @@
Cancel()
metodu farklı sonuç verdi.

Neden?

Çünkü

State değişti.
```

```c#
```

```diff
@@ TCP Connection @@
Microsoft'un klasik örneği.
```

```
Closed
↓
Listen
↓
Established
↓
Closing
```

```diff
@@ Aynı Send() Closed iken çalışmaz. Established iken çalışır.@@
```
__________________________________________
```diff
@@ MassTransit @@
Saga aslında State Machine'dir.

Bu yüzden State Pattern'i bilirsen

Saga çok kolay gelir.
```
```
Avantajları

✅ if/switch azalır.

✅ Her state tek class.

✅ Yeni state eklemek kolay.

✅ Kod okunur.

Dezavantaj

❌ State çok artarsa

20 state

olabilir.
```


```
Strategy ile fark

En çok gelen soru.

Strategy

Algoritma

kullanıcı seçer.

Visa

Master

Troy
State

Algoritmayı

nesnenin mevcut durumu seçer.

Pending

Paid

Shipped

Bak

kim seçiyor?

State.

Factory ile fark

Factory

hangi nesne?

State

hangi durumda?
Command ile fark

Command

Ne yapılacak?

State

Şu an hangi durumdayım?
Observer ile fark

Observer

Bir olay oldu.

Herkes öğrensin.

State

Durum değişti.

Davranış değişti.
Strategy ile karıştırma

Şunu ezberle.

Strategy
Dışarıdan seçilir.

Mesela

Visa

Master

Sen seçiyorsun.

State
Kendi kendine değişir.
Pending

↓

Paid

↓

Shipped

Sistem

state'i değiştiriyor.
```
> State Pattern, bir nesnenin davranışını bulunduğu duruma göre değiştirmesini sağlar. Durum değiştikçe nesnenin davranışı da değişir.

> State ile Strategy arasındaki fark nedir?
> Strategy'de algoritma dışarıdan seçilir. State Pattern'de ise davranış, nesnenin mevcut durumuna göre otomatik değişir ve state'ler birbirine geçiş yapabilir.

```
1)

State Pattern'in temel amacı nedir?

A) Nesne üretmek

B) Davranışı mevcut duruma göre değiştirmek

C) Algoritma seçmek

D) Interface çevirmek

2)

Sipariş

Pending

↓

Paid

↓

Shipped

hangi pattern'dir?

A) Strategy

B) State

C) Builder

D) Factory

3)

Aşağıdakilerden hangisi State Pattern için uygun örnektir?

A) TCP Connection

B) ATM işlem durumu

C) Sipariş workflow'u

D) Hepsi

4)

Strategy ile State arasındaki temel fark nedir?

A) İkisi aynıdır.

B) Strategy algoritmayı dışarıdan seçer, State mevcut duruma göre davranışı değiştirir.

C) State nesne üretir.

D) Strategy event yayınlar.

5)

MassTransit Saga en çok hangi pattern ile ilişkilidir?

A) Observer

B) Factory

C) State

D) Decorator

Doğru Cevaplar
B
B
D
B
C
```
