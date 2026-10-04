using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Core;

public interface ISmartHomeMediator
{
    void ActivateAwayMode();
    void ActivateHomeMode();
    void Notify(object sender, SmartHomeEvent eventType);
}