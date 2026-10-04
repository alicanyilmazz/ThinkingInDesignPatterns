using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Core;

public abstract class SmartHomeDevice
{
    protected ISmartHomeMediator Mediator { get; }

    protected SmartHomeDevice(ISmartHomeMediator mediator)
    {
        Mediator = mediator;
    }
}