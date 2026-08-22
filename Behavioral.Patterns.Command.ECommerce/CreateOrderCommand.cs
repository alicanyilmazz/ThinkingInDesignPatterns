namespace Behavioral.Patterns.Command.ECommerce;

public class CreateOrderCommand : ICommand
{
    private readonly OrderService _orderService;

    private readonly Order _order;

    public CreateOrderCommand(OrderService orderService, Order order)
    {
        _orderService = orderService;
        _order = order;
    }

    public void Execute()
    {
        _orderService.CreateOrder(_order);
    }
}