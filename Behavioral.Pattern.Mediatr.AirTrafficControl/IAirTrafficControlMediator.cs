using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.AirTrafficControl;

public interface IAirTrafficControlMediator
{
    void RequestLanding(Aircraft aircraft);
    void RequestTakeOff(Aircraft aircraft);
}
