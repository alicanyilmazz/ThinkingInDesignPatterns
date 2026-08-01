using Microsoft.VisualBasic;

Console.WriteLine("Hello, World!");
ICommissionStrategy strategy;

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