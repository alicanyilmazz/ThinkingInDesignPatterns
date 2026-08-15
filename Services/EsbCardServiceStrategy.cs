using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class EsbCardServiceStrategy : ICardServiceStrategy
{
    public ServiceType Type => ServiceType.Esb;

    public CardValidationResponse ValidateDebitCardRequest(CardValidationRequest request)
    {
        Console.WriteLine("ESB ValidateDebitCardRequest Called!");

        // Gerçek projede:
        //
        // var esbRequest = MapToEsbRequest(request);
        // var esbResponse = esbClient.Validate(esbRequest);
        // return MapToResponse(esbResponse);

        return new CardValidationResponse
        {
            IsValid = true,
            Message = "Card validated through ESB."
        };
    }
}