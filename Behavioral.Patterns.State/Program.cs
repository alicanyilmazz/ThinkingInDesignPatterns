using Behavioral.Patterns.State;

Order order = new Order(id: 1001, productName: "Laptop", amount: 50000);


Console.WriteLine($"Sipariş Durumu: {order.Status}");

Console.WriteLine("--------------------------");


order.Ship();


Console.WriteLine("--------------------------");


order.Pay();


Console.WriteLine($"Sipariş Durumu: {order.Status}");

Console.WriteLine("--------------------------");


order.Pay();


Console.WriteLine("--------------------------");


order.Ship();


Console.WriteLine($"Sipariş Durumu: {order.Status}");

Console.WriteLine("--------------------------");


order.Cancel();


Console.WriteLine("--------------------------");


order.Deliver();


Console.WriteLine($"Sipariş Durumu: {order.Status}");

Console.WriteLine("--------------------------");


order.Cancel();