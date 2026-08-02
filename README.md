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

__________________________________________



```diff
@@ ASP.NET Core örneği: Domain Event @@

ASP.NET Core uygulamalarında Observer mantığı çoğu zaman domain event veya notification handler üzerinden uygulanır.

Örneğin sipariş oluşturulunca:
```

```c#
public sealed record OrderCreatedEvent(
    int OrderId,
    string CustomerEmail,
    decimal TotalAmount);
```
```diff
@@ Handler interface: @@
```

```c#
public interface IDomainEventHandler<in TEvent>
{
    Task HandleAsync(
        TEvent domainEvent,
        CancellationToken cancellationToken);
}
```
```diff
@@ Email handler: @@
```

```c#
public sealed class SendOrderEmailHandler
    : IDomainEventHandler<OrderCreatedEvent>
{
    public Task HandleAsync(
        OrderCreatedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"E-posta gönderildi: " +
            $"{domainEvent.CustomerEmail}");

        return Task.CompletedTask;
    }
}
```
```diff
@@ Stock handler: @@
```

```c#
public sealed class ReduceStockHandler
    : IDomainEventHandler<OrderCreatedEvent>
{
    public Task HandleAsync(
        OrderCreatedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Stok azaltıldı. OrderId: " +
            $"{domainEvent.OrderId}");

        return Task.CompletedTask;
    }
}
```
```diff
@@ Publisher @@
```

```c#
public sealed class DomainEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(
        TEvent domainEvent,
        CancellationToken cancellationToken)
    {
        IEnumerable<IDomainEventHandler<TEvent>> handlers =
            _serviceProvider.GetServices<
                IDomainEventHandler<TEvent>>();

        foreach (IDomainEventHandler<TEvent> handler in handlers)
        {
            await handler.HandleAsync(
                domainEvent,
                cancellationToken);
        }
    }
}
```
```diff
@@ DI kayıtları: @@
```

```c#
builder.Services.AddScoped<
    IDomainEventHandler<OrderCreatedEvent>,
    SendOrderEmailHandler>();

builder.Services.AddScoped<
    IDomainEventHandler<OrderCreatedEvent>,
    ReduceStockHandler>();

builder.Services.AddScoped<DomainEventPublisher>();
```
```diff
@@ Order service: @@
```

```c#
public sealed class OrderApplicationService
{
    private readonly DomainEventPublisher _publisher;

    public OrderApplicationService(
        DomainEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task CreateOrderAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Order kaydedildi: {order.Id}");

        var domainEvent = new OrderCreatedEvent(
            order.Id,
            order.CustomerEmail,
            order.TotalAmount);

        await _publisher.PublishAsync(
            domainEvent,
            cancellationToken);
    }
}
```
```diff
@@ Bu yapıda OrderApplicationService, email ve stock handler’larını doğrudan bilmez. @@
Sadece event yayınlar.
```
__________________________________________
```diff
@@ MediatR ile Observer mantığı @@
Gerçek ASP.NET Core projelerinde MediatR notification yapısı da Observer mantığına benzer.
Event:
```
```c#
public sealed record OrderCreatedNotification(
    int OrderId,
    string CustomerEmail)
    : INotification;
```
```diff
@@ Handler 1: @@
```

```c#
public sealed class SendEmailHandler
    : INotificationHandler<OrderCreatedNotification>
{
    public Task Handle(
        OrderCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"E-posta gönderildi: " +
            $"{notification.CustomerEmail}");

        return Task.CompletedTask;
    }
}
```
```diff
@@ Handler 2: @@
```

```c#
public sealed class CreateInvoiceHandler
    : INotificationHandler<OrderCreatedNotification>
{
    public Task Handle(
        OrderCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Fatura oluşturuldu: {notification.OrderId}");

        return Task.CompletedTask;
    }
}
```
```diff
@@ Yayınlama: @@
```

```c#
await mediator.Publish(
    new OrderCreatedNotification(
        order.Id,
        order.CustomerEmail),
    cancellationToken);
```
```diff
Bir event yayınlanır, birden fazla handler çalışabilir.

Bu yapı Observer Pattern’in modern uygulamalarından biridir.
```
__________________________________________

```diff
@@ Microservice dünyasında Observer @@
Microservice’lerde publisher ve subscriber çoğu zaman aynı process içinde değildir.

Örneğin:
```

```c#
Order Service
   ↓
OrderCreated event
   ↓
RabbitMQ / Kafka
   ↓
Inventory Service
Notification Service
Invoice Service
Shipping Service
```
```diff
@@ Burada Order Service publisher’dır. @@

@@ Diğer servisler subscriber’dır. @@

@@ Bu yapı klasik Observer’ın dağıtık sistem versiyonu gibi düşünülebilir. Fakat message broker kullanıldığı için bazı ek konular gelir: @@

- Eventual consistency
- Retry
- Duplicate message
- Idempotency
- Outbox Pattern
- Dead-letter queue
- Message ordering

Observer mantığı temel olsa da dağıtık sistemde güvenilir mesajlaşma ayrıca çözülmelidir.
```

```diff
@@ React ile ilişkisi @@

React’te state değiştiğinde component’lerin yeniden render edilmesi Observer mantığına benzer.

Örneğin Context Provider bir değer yayınlar:
```

```c#
const ThemeContext = createContext("light");
```
```diff
@@ Context kullanan component’ler bu değere abonedir: @@
```

```c#
const theme = useContext(ThemeContext);
```
```diff
Provider değeri değiştiğinde subscriber component’ler yeniden render edilir.

Bu birebir klasik GoF implementasyonu değildir ama publisher-subscriber fikri aynıdır.
```

```c#
```
```diff
@@ Observer ile Pub/Sub farkı @@
Çok önemli bir farktır.

@@ Observer @@
Subject, observer referanslarını genelde doğrudan tutar:
```

```c#
Subject
  ├─ Observer A
  ├─ Observer B
  └─ Observer C
```
```diff
Publisher ile subscriber arasında doğrudan veya aynı process içinde ilişki vardır.
@@ Pub/Sub @@
Arada broker veya event bus vardır:
```

```c#
Publisher
   ↓
Message Broker
   ↓
Subscriber
```
```diff
Publisher subscriber’ları bilmez.

Microservice mimarisinde daha çok Pub/Sub kullanılır.

Kısa fark:
- Observer → Doğrudan abonelik
- Pub/Sub  → Broker üzerinden abonelik
```

```diff
@@ Observer ile Mediator farkı @@
Observer’da bir publisher olayı yayınlar, birden fazla observer dinler.

Mediator’da nesneler birbirleriyle doğrudan konuşmak yerine merkezi mediator üzerinden iletişim kurar.
Observer:
Publisher → Birden fazla subscriber

Mediator:
Component A → Mediator → Component B
```
```diff
@@ Observer ile Decorator farkı @@
```
```diff
Decorator bir nesnenin davranışını sararak genişletir:
Logging → Retry → Payment
Observer bir olay olduğunda birden fazla subscriber’ı bilgilendirir:
OrderCreated
  ├─ Email
  ├─ Stock
  └─ Invoice
```
```diff
@@ Avantajları @@
✅ Publisher ve subscriber bağımlılığı azalır.
✅ Yeni subscriber eklemek kolaydır.
✅ Bir olay birden fazla işlem tetikleyebilir.
✅ OCP desteklenir.
✅ Event-driven sistemlerin temelini oluşturur.
✅ Yan işlemler ana servisten ayrılır.

@@ Dezavantajları @@
❌ Çalışma sırası belirsizleşebilir.
❌ Hangi observer’ın çalıştığını takip etmek zorlaşabilir.
❌ Bir observer hata verirse diğerlerini etkileyebilir.
❌ Unsubscribe yapılmazsa memory leak oluşabilir.
❌ Çok fazla event varsa sistemin akışı görünmez hale gelebilir.
❌ Senkron observer’lar ana işlemi yavaşlatabilir.
```

```diff
@@ Senkron ve asenkron Observer @@
@@ Senkron @@
```

```c#
foreach (var observer in observers)
{
    observer.Update(order);
}
```
```diff
@@ Bir observer yavaşsa bütün işlem bekler. @@
@@ Asenkron @@
```

```c#
await Task.WhenAll(
    observers.Select(
        observer => observer.UpdateAsync(
            order,
            cancellationToken)));
```

```
Ancak paralel çalıştırmada:

Thread safety
Hata yönetimi
Transaction bütünlüğü
Sıralama

konularına dikkat edilmelidir.

Ne zaman kullanılır?

Observer şu durumlarda uygundur:

Bir değişiklik birden fazla tarafı ilgilendiriyorsa
Publisher subscriber’ları doğrudan bilmemeliyse
Event-driven yapı kuruluyorsa
UI güncellemeleri yapılacaksa
Domain event kullanılacaksa
Notification sistemi kurulacaksa
Ne zaman kullanılmamalı?
Yalnızca tek ve zorunlu bir işlem varsa
İşlem sırası kesin ve transaction içinde olmalıysa
Hata durumunda bütün işlemler birlikte rollback edilmeli ise
Event zinciri sistemi gereksiz karmaşıklaştırıyorsa

Örneğin bakiye düşme ve muhasebe kaydı aynı transaction içinde kesinlikle birlikte yapılmalıysa bunları gevşek observer’lara bırakmak riskli olabilir.

Mülakat cevabı

Observer Pattern, bir subject’in durumunda değişiklik olduğunda ona abone olan observer’ların otomatik olarak bilgilendirilmesini sağlar. Publisher subscriber’ların concrete implementasyonlarını bilmez. C#’ta event/delegate, ASP.NET Core’da domain event veya MediatR notification, microservice sistemlerinde ise message broker tabanlı Pub/Sub yapıları bu yaklaşıma örnek verilebilir.

Kısa özeti:

Bir olay olur → Birden fazla abone haberdar edilir.
```
