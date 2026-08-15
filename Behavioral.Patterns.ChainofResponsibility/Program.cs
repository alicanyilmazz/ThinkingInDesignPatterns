using Behavioral.Patterns.ChainofResponsibility;

var card = new CardValidationHandler();

var pin = new PinValidationHandler();

var balance = new BalanceHandler();

var cash = new CashHandler();

card.SetNext(pin);

pin.SetNext(balance);

balance.SetNext(cash);

card.Handle(request);