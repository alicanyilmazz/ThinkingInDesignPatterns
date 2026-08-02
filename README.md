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
@@  @@
```

```c#
```

```diff
@@  @@
```

```c#
```
