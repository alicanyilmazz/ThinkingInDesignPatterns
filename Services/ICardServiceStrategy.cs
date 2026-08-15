using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public interface ICardServiceStrategy
{
    ServiceType Type { get; }

    CardValidationResponse ValidateDebitCardRequest(CardValidationRequest request);
}