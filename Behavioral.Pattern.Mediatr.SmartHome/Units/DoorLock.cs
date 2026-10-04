using Behavioral.Pattern.Mediatr.SmartHome.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Units;

public class DoorLock : SmartHomeDevice
{
    public DoorLock(ISmartHomeMediator mediator) : base(mediator)
    {
    }

    public void Lock()
    {
        Console.WriteLine("Doors locked.");
    }

    public void Unlock()
    {
        Console.WriteLine("Doors unlocked.");
    }
}
