using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralPatterns.Decorator;

public interface IPaymentService
{
    void Pay(decimal amount);
}
