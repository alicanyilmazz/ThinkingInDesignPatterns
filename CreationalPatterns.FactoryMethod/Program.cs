using CreationalPatterns.FactoryMethod;
using CreationalPatterns.FactoryMethod.Notification;
using CreationalPatterns.FactoryMethod.Strategy;
using Microsoft.VisualBasic;

// BAD CODE: This is a bad implementation of the Factory Method pattern. The code is tightly coupled to the concrete implementations of the commission strategies,
// which makes it difficult to add new strategies or change existing ones without modifying the client code.
// A better approach would be to use a factory class or method to create the appropriate strategy based on the card type.

ICommissionStrategy strategy;
var cardType = CardType.Visa; 
if (cardType == CardType.Visa)
{
    strategy = new VisaCommissionStrategy();
}
else if (cardType == CardType.MasterCard)
{
    strategy = new MasterCardCommissionStrategy();
}
else
{
    strategy = new TroyCommissionStrategy();
}

// GOOD CODE 

ICommissionStrategyFactory factory = new CommissionStrategyFactory();

ICommissionStrategy strategie = factory.Create(CardType.MasterCard);

decimal commission = strategie.Calculate(1000);

Console.WriteLine(commission);

// 

NotificationCreator creator = new EmailNotificationCreator();

creator.Notify("İşlem başarılı.");