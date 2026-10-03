using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.AirTrafficControl;

public abstract class Aircraft
{
    protected readonly IAirTrafficControlMediator Mediator;

    public string FlightCode { get; }

    protected Aircraft(string flightCode, IAirTrafficControlMediator mediator)
    {
        FlightCode = flightCode;
        Mediator = mediator;
    }

    public abstract void RequestLanding();

    public abstract void RequestTakeOff();

    public virtual void Land()
    {
        Console.WriteLine($"{FlightCode} is landing.");
    }

    public virtual void TakeOff()
    {
        Console.WriteLine($"{FlightCode} is taking off.");
    }

    public virtual void Wait()
    {
        Console.WriteLine($"{FlightCode} is waiting.");
    }
}
