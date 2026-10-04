using Behavioral.Pattern.Mediatr.SmartHome.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Units;

public class Alarm : SmartHomeDevice
{
    public Alarm(ISmartHomeMediator mediator) : base(mediator)
    {
    }

    public void Arm()
    {
        Console.WriteLine("Alarm armed.");
    }

    public void Disarm()
    {
        Console.WriteLine("Alarm disarmed.");
    }

    public void Trigger()
    {
        Console.WriteLine("ALARM TRIGGERED!");
    }
}
