using BehavioralPatterns.Strategy.Strategies;
using Microsoft.VisualBasic;


ICommissionStrategy visaStrategy = new VisaCommissionStrategy();
ICommissionStrategy masterStrategy = new MasterCardCommissionStrategy();

var calculator = new CommissionCalculator(visaStrategy);

decimal commission = calculator.Calculate(1000);

Console.WriteLine(commission);

var cardType = CardType.Visa;   

ICommissionStrategy strategy = cardType switch
{
    CardType.Visa => new VisaCommissionStrategy(),
    CardType.MasterCard => new MasterCardCommissionStrategy(),
    CardType.Troy => new TroyCommissionStrategy(),
    _ => throw new NotSupportedException()
};