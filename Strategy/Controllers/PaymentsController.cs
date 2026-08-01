using Microsoft.AspNetCore.Mvc;
using Strategy.Strategy;

namespace Strategy.Controllers;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResult>> Pay(
        [FromBody] PaymentRequest request,
        CancellationToken cancellationToken)
    {
        PaymentResult result =
            await _paymentService.PayAsync(
                request,
                cancellationToken);

        return Ok(result);
    }
}