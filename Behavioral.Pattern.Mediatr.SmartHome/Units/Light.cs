using Behavioral.Pattern.Mediatr.SmartHome.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Units;

public class Light : SmartHomeDevice
{
    public Light(ISmartHomeMediator mediator) : base(mediator)
    {
    }

    public void TurnOn()
    {
        Console.WriteLine("Lights turned on.");
    }

    public void TurnOff()
    {
        Console.WriteLine("Lights turned off.");
    }
}
