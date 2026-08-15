using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public interface ICardServiceStrategyResolver
{
    ICardServiceStrategy Resolve(ServiceType serviceType);
}
