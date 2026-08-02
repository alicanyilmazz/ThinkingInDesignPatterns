# DesignPatterns

```diff
@@ Observer Pattern @@

Bu pattern özellikle şu konuların temelidir:

Event-driven architecture
Domain events
UI event’leri
Notification sistemleri
Message broker mantığı
Microservice event yayınlama
React state değişiklikleri
C# event ve delegate yapısı
```

```diff
@@ Observer Pattern nedir? @@


```
> Bir nesnenin durumu değiştiğinde, ona abone olan diğer nesnelerin otomatik olarak bilgilendirilmesini sağlar.

```diff
- Burada iki ana taraf vardır:
@@ Subject / Publisher: Olayı yayınlayan taraf @@
@@ Observer / Subscriber: Olayı dinleyen taraf @@

Örneğin sipariş oluşturulduğunda:

OrderService
   ↓
OrderCreated olayı
   ↓
EmailNotification
StockService
InvoiceService
LoyaltyPointService

- OrderService, bu servisleri tek tek çağırmak zorunda kalmaz. Olayı yayınlar, aboneler kendi işlerini yapar.

```

__________________________________________


```diff
@@ Observer olmadan problem @@
```

```c#
public sealed class OrderService
{
    private readonly EmailService _emailService;
    private readonly StockService _stockService;
    private readonly InvoiceService _invoiceService;

    public OrderService(
        EmailService emailService,
        StockService stockService,
        InvoiceService invoiceService)
    {
        _emailService = emailService;
        _stockService = stockService;
        _invoiceService = invoiceService;
    }

    public void CreateOrder(Order order)
    {
        Console.WriteLine("Sipariş oluşturuldu.");

        _emailService.Send(order);
        _stockService.Reduce(order);
        _invoiceService.Create(order);
    }
}
```
```diff
Buradaki sorun:

@@ OrderService @@ , bütün yan işlemleri biliyor.

Yeni bir requirement geldiğinde:

SMS gönder
Loyalty puanı ekle
Audit log yaz
Kargo kaydı oluştur

@@ OrderService @@ sürekli değiştirilir.
- Bu hem bağımlılığı artırır hem de OCP açısından kötü bir tasarımdır.
```
__________________________________________
```diff
@@ Observer çözümü @@

@@ OrderService @@ yalnızca olay yayınlar:

OrderCreated

Aboneler bu olayı dinler:
- EmailObserver
- StockObserver
- InvoiceObserver

```
__________________________________________
```diff
@@ Temel C# örneği @@
Observer interface
```
```c#
public interface IOrderObserver
{
    void Update(Order order);
}
```
```diff
@@ Bütün observer’lar bu sözleşmeye uyar.  @@

@@ Subject interface @@
```

```c#
public interface IOrderSubject
{
    void Subscribe(IOrderObserver observer);

    void Unsubscribe(IOrderObserver observer);

    void Notify(Order order);
}
```
```diff
@@ Order modeli @@
```

```c#
public sealed class Order
{
    public int Id { get; init; }

    public string CustomerEmail { get; init; } = string.Empty;

    public decimal TotalAmount { get; init; }
}
```
```diff
@@ Subject implementasyonu @@
```

```c#
public sealed class OrderService : IOrderSubject
{
    private readonly List<IOrderObserver> _observers = [];

    public void Subscribe(IOrderObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IOrderObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Remove(observer);
    }

    public void Notify(Order order)
    {
        foreach (IOrderObserver observer in _observers)
        {
            observer.Update(order);
        }
    }

    public void CreateOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        Console.WriteLine(
            $"Sipariş oluşturuldu. OrderId: {order.Id}");

        Notify(order);
    }
}
```
```diff
@@ Buradaki önemli yer: @@
```

```c#
private readonly List<IOrderObserver> _observers = [];
```
```diff
@@ Subject, kendisine abone olan observer’ları tutar. @@
```
```diff
@@ Sipariş oluşturulunca: @@
```
```c#
Notify(order);
```
```diff
@@ çağrılır. @@
```
__________________________________________
```diff
@@ Observer implementasyonları @@
```
```diff
@@ Email Observer @@
```
```c#
public sealed class EmailObserver : IOrderObserver
{
    public void Update(Order order)
    {
        Console.WriteLine(
            $"E-posta gönderildi: {order.CustomerEmail}");
    }
}
```
```diff
@@ Stock Observer @@
```
```c#
public sealed class StockObserver : IOrderObserver
{
    public void Update(Order order)
    {
        Console.WriteLine(
            $"Order {order.Id} için stok azaltıldı.");
    }
}
```
```diff
@@ Invoice Observer @@
```
```c#
public sealed class InvoiceObserver : IOrderObserver
{
    public void Update(Order order)
    {
        Console.WriteLine(
            $"Order {order.Id} için fatura oluşturuldu.");
    }
}
```
```diff
@@ Console App kullanımı @@
```
```c#
var orderService = new OrderService();

var emailObserver = new EmailObserver();
var stockObserver = new StockObserver();
var invoiceObserver = new InvoiceObserver();

orderService.Subscribe(emailObserver);
orderService.Subscribe(stockObserver);
orderService.Subscribe(invoiceObserver);

var order = new Order
{
    Id = 1001,
    CustomerEmail = "ali@example.com",
    TotalAmount = 1_500m
};

orderService.CreateOrder(order);
```
```diff
@@ Çıktı: @@
Sipariş oluşturuldu. OrderId: 1001
E-posta gönderildi: ali@example.com
Order 1001 için stok azaltıldı.
Order 1001 için fatura oluşturuldu.
```

```diff
@@ Unsubscribe @@
Bir observer artık olayı dinlemeyecekse:
```
```c#
orderService.Unsubscribe(invoiceObserver);
```

```diff
@@ Sonraki siparişte fatura observer’ı çalışmaz. @@
```
__________________________________________
```diff
@@ C# event ile gerçekçi kullanım @@
C# dilinde @@ Observer Pattern @@  genellikle @@ event @@  ve @@ delegate @@  kullanılarak uygulanır.

@@ EventArgs modeli @@
```
```c#
public sealed class OrderCreatedEventArgs : EventArgs
{
    public required Order Order { get; init; }
}
```
```diff
@@ Publisher @@
```
```c#
public sealed class OrderService
{
    public event EventHandler<OrderCreatedEventArgs>? OrderCreated;

    public void CreateOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        Console.WriteLine(
            $"Sipariş oluşturuldu: {order.Id}");

        OnOrderCreated(order);
    }

    private void OnOrderCreated(Order order)
    {
        OrderCreated?.Invoke(
            this,
            new OrderCreatedEventArgs
            {
                Order = order
            });
    }
}
```
```diff
@@ Buradaki: @@
```
```c#
OrderCreated?.Invoke(...)
```
```diff
@@ olayı yayınlar. @@
```

```diff
@@ Subscriber sınıfları @@
```
```c#
public sealed class EmailNotificationHandler
{
    public void Handle(
        object? sender,
        OrderCreatedEventArgs eventArgs)
    {
        Console.WriteLine(
            $"E-posta gönderildi: " +
            $"{eventArgs.Order.CustomerEmail}");
    }
}
```
```c#
public sealed class StockHandler
{
    public void Handle(
        object? sender,
        OrderCreatedEventArgs eventArgs)
    {
        Console.WriteLine(
            $"Stok azaltıldı. OrderId: " +
            $"{eventArgs.Order.Id}");
    }
}
```
```diff
@@ Abone olma @@
```
```c#
var orderService = new OrderService();

var emailHandler = new EmailNotificationHandler();
var stockHandler = new StockHandler();

orderService.OrderCreated += emailHandler.Handle;
orderService.OrderCreated += stockHandler.Handle;

orderService.CreateOrder(
    new Order
    {
        Id = 1001,
        CustomerEmail = "ali@example.com",
        TotalAmount = 2_000m
    });
```

```diff
@@ Abonelik kaldırma: @@
```

```c#
orderService.OrderCreated -= emailHandler.Handle;
```
> Bu, C#’taki klasik Observer Pattern kullanımına çok yakındır.
