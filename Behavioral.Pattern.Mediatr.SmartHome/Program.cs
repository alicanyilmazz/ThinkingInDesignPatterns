using Behavioral.Pattern.Mediatr.SmartHome.Core;
using Behavioral.Pattern.Mediatr.SmartHome.Units;

var mediator = new SmartHomeMediator();

var doorLock = new DoorLock(mediator);
var light = new Light(mediator);
var alarm = new Alarm(mediator);
var thermostat = new Thermostat(mediator);
var motionSensor = new MotionSensor(mediator);

mediator.Configure(doorLock, light, alarm, thermostat);

mediator.ActivateAwayMode();

Console.WriteLine();

motionSensor.DetectMotion();
