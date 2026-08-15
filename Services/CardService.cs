using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class CardService
{
    private readonly ICardServiceStrategyResolver _resolver;

    public CardService(ICardServiceStrategyResolver resolver)
    {
        _resolver = resolver;
    }

    public CardValidationResponse ValidateDebitCardRequest(CardValidationRequest request,ServiceType serviceType)
    {
        ArgumentNullException.ThrowIfNull(request);

        var strategy = _resolver.Resolve(serviceType);

        return strategy.ValidateDebitCardRequest(request);
    }
}