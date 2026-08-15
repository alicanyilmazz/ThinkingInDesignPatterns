using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class EsbCardServiceStrategy : IEsbCardServiceStrategy
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

    public DoPinChangeResponse DoPinChange(DoPinChangeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        Console.WriteLine("ESB DoPinChange Called!");

        return new DoPinChangeResponse
        {
            Success = true
        };
    }

    public DoCashWithDrawalResponse DoCashWithDrawal(DoCashWithDrawalRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        Console.WriteLine("ESB DoCashWithDrawal Called!");

        return new DoCashWithDrawalResponse
        {
            Success = true
        };
    }
}