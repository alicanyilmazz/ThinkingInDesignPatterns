using Behavioral.Patterns.Command.BackgroundJob.Commands.Abstracts;
using Behavioral.Patterns.Command.BackgroundJob.Services;

namespace Behavioral.Patterns.Command.BackgroundJob.Commands;

public class ProcessPaymentCommand : ICommand
{
    private readonly PaymentService _paymentService;

    private readonly int _orderId;

    private readonly decimal _amount;

    public ProcessPaymentCommand(PaymentService paymentService,int orderId,decimal amount)
    {
        _paymentService = paymentService;
        _orderId = orderId;
        _amount = amount;
    }

    public void Execute()
    {
        _paymentService.ProcessPayment(_orderId,_amount);
    }
}
