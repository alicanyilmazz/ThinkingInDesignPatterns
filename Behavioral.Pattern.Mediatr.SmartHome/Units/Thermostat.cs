using Behavioral.Pattern.Mediatr.SmartHome.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Units;

public class Thermostat : SmartHomeDevice
{
    public Thermostat(ISmartHomeMediator mediator) : base(mediator)
    {
    }

    public void EnableEcoMode()
    {
        Console.WriteLine("Thermostat switched to ECO mode.");
    }

    public void EnableComfortMode()
    {
        Console.WriteLine("Thermostat switched to COMFORT mode.");
    }
}