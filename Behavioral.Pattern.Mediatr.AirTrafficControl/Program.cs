using Behavioral.Pattern.Mediatr.AirTrafficControl;

//Observer:
//"Ben değiştim, ilgilenen herkese haber ver."
//Mediator:
//"Bir olay oldu, şimdi kim ne yapacak merkezi olarak ben koordine edeyim."

var controlTower = new ControlTowerMediator();

var turkishAirlines = new PassengerPlane("TK1923", controlTower);

var cargoPlane = new CargoPlane("CG451", controlTower);

turkishAirlines.RequestLanding();

Console.WriteLine();

cargoPlane.RequestTakeOff();