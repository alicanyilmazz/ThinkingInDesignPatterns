using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class CardValidationRequest
{
    public string CardNumber { get; set; }

    public decimal Amount { get; set; }
}
