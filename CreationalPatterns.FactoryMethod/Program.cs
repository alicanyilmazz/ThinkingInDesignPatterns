using Microsoft.VisualBasic;

public interface ICommissionStrategy
{
    decimal Calculate(decimal amount);
}

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