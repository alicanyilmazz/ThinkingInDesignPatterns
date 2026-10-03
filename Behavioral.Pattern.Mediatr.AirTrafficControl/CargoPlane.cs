using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.AirTrafficControl;

public class CargoPlane : Aircraft
{
    public CargoPlane(string flightCode, IAirTrafficControlMediator mediator) : base(flightCode, mediator)
    {
    }

    public override void RequestLanding()
    {
        Console.WriteLine($"{FlightCode} requests permission to land.");

        Mediator.RequestLanding(this);
    }

    public override void RequestTakeOff()
    {
        Console.WriteLine($"{FlightCode} requests permission to take off.");

        Mediator.RequestTakeOff(this);
    }
}