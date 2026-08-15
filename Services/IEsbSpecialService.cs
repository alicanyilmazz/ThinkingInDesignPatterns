using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public interface IEsbSpecialService
{
    DoPinChangeResponse DoPinChange(DoPinChangeRequest request);

    DoCashWithDrawalResponse DoCashWithDrawal(DoCashWithDrawalRequest request);
}