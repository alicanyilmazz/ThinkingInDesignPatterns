using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class WebServiceCardServiceStrategy : ICardServiceStrategy
{
    public ServiceType Type => ServiceType.WebService;

    public CardValidationResponse ValidateDebitCardRequest(CardValidationRequest request)
    {
        Console.WriteLine("WebService ValidateDebitCardRequest Called!");

        // Gerçek projede:
        //
        // var wsRequest = MapToWebServiceRequest(request);
        // var wsResponse = webServiceClient.Validate(wsRequest);
        // return MapToResponse(wsResponse);

        return new CardValidationResponse
        {
            IsValid = true,
            Message = "Card validated through WebService."
        };
    }
}