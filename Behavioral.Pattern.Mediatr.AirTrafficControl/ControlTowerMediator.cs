using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.AirTrafficControl;

public class ControlTowerMediator : IAirTrafficControlMediator
{
    private bool _runwayAvailable = true;

    public void RequestLanding(Aircraft aircraft)
    {
        if (!_runwayAvailable)
        {
            Console.WriteLine($"Control Tower: Runway is busy. {aircraft.FlightCode} must wait.");

            aircraft.Wait();
            return;
        }

        Console.WriteLine($"Control Tower: {aircraft.FlightCode} is cleared to land.");

        _runwayAvailable = false;

        aircraft.Land();

        _runwayAvailable = true;
    }

    public void RequestTakeOff(Aircraft aircraft)
    {
        if (!_runwayAvailable)
        {
            Console.WriteLine($"Control Tower: Runway is busy. {aircraft.FlightCode} must wait.");

            aircraft.Wait();
            return;
        }

        Console.WriteLine($"Control Tower: {aircraft.FlightCode} is cleared for takeoff.");

        _runwayAvailable = false;

        aircraft.TakeOff();

        _runwayAvailable = true;
    }
}