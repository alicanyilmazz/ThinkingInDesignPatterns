namespace Behavioral.Patterns.Command.ECommerce;

public class OrderService
{
    public void CreateOrder(Order order)
    {
        Console.WriteLine("Sipariş oluşturuluyor...");

        order.Status = "Created";

        Console.WriteLine($"Sipariş oluşturuldu. Order Id: {order.Id}");

        Console.WriteLine($"Müşteri: {order.CustomerName}");

        Console.WriteLine($"Ürün: {order.ProductName}");

        Console.WriteLine($"Adet: {order.Quantity}");

        Console.WriteLine($"Toplam: {order.TotalPrice:N2} TL");
    }

    public void CancelOrder(Order order)
    {
        Console.WriteLine("Sipariş iptal ediliyor...");

        order.Status = "Cancelled";

        Console.WriteLine($"{order.Id} numaralı sipariş iptal edildi.");
    }
}