using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class EsbSpecialService : IEsbSpecialService
{
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