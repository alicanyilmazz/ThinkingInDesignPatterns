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
