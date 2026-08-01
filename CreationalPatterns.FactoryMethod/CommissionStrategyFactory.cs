namespace CreationalPatterns.FactoryMethod;

public sealed class CommissionStrategyFactory : ICommissionStrategyFactory
{
    public ICommissionStrategy Create(CardType cardType)
    {
        return cardType switch
        {
            CardType.Visa => new VisaCommissionStrategy(),

            CardType.MasterCard => new MasterCardCommissionStrategy(),

            CardType.Troy => new TroyCommissionStrategy(),

            _ => throw new NotSupportedException($"Desteklenmeyen kart tipi: {cardType}")
        };
    }
}