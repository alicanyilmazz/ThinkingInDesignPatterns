namespace Behavioral.Patterns.Command.ECommerce;

public class CancelOrderCommand : ICommand
{
    private readonly OrderService _orderService;

    private readonly Order _order;

    public CancelOrderCommand(OrderService orderService,Order order)
    {
        _orderService = orderService;
        _order = order;
    }

    public void Execute()
    {
        _orderService.CancelOrder(_order);
    }
}