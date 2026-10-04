using Behavioral.Pattern.Mediatr.SmartHome.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Units;

public class MotionSensor : SmartHomeDevice
{
    public MotionSensor(ISmartHomeMediator mediator) : base(mediator)
    {
    }

    public void DetectMotion()
    {
        Console.WriteLine("Motion detected.");

        Mediator.Notify(this, SmartHomeEvent.MotionDetected);
    }
}