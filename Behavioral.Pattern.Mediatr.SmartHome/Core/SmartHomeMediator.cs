using Behavioral.Pattern.Mediatr.SmartHome.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Pattern.Mediatr.SmartHome.Core;

public class SmartHomeMediator : ISmartHomeMediator
{
    private DoorLock? _doorLock;
    private Light? _light;
    private Alarm? _alarm;
    private Thermostat? _thermostat;

    private bool _awayMode;

    public void Configure(DoorLock doorLock, Light light, Alarm alarm, Thermostat thermostat)
    {
        _doorLock = doorLock;
        _light = light;
        _alarm = alarm;
        _thermostat = thermostat;
    }

    public void ActivateAwayMode()
    {
        Console.WriteLine("Away mode activated.");

        _awayMode = true;

        _doorLock?.Lock();
        _light?.TurnOff();
        _alarm?.Arm();
        _thermostat?.EnableEcoMode();
    }

    public void ActivateHomeMode()
    {
        Console.WriteLine("Home mode activated.");

        _awayMode = false;

        _doorLock?.Unlock();
        _alarm?.Disarm();
        _light?.TurnOn();
        _thermostat?.EnableComfortMode();
    }

    public void Notify(object sender, SmartHomeEvent eventType)
    {
        if (eventType == SmartHomeEvent.MotionDetected)
        {
            HandleMotionDetected();
        }
    }

    private void HandleMotionDetected()
    {
        if (_awayMode)
        {
            Console.WriteLine("Mediator: Motion detected while house is empty.");

            _alarm?.Trigger();
            _light?.TurnOn();
        }
        else
        {
            Console.WriteLine("Mediator: Motion detected while user is home.");

            _light?.TurnOn();
        }
    }
}