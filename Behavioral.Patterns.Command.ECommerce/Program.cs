using Behavioral.Patterns.Command.ECommerce;

Order order = new Order
{
    Id = 1001,
    CustomerName = "Alican",
    ProductName = "iPhone 17",
    Quantity = 1,
    TotalPrice = 60000
};


OrderService orderService = new OrderService();

OrderInvoker orderInvoker = new OrderInvoker();


ICommand createOrderCommand = new CreateOrderCommand(orderService, order);


orderInvoker.ExecuteCommand(createOrderCommand);


Console.WriteLine();

Console.WriteLine($"Sipariş durumu: {order.Status}");


Console.WriteLine("\n---------------------\n");


ICommand cancelOrderCommand = new CancelOrderCommand(orderService, order);

orderInvoker.ExecuteCommand(cancelOrderCommand);


Console.WriteLine();

Console.WriteLine($"Sipariş durumu: {order.Status}");