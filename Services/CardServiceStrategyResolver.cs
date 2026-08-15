using System;
using System.Collections.Generic;
using System.Text;

namespace Services;

public sealed class CardServiceStrategyResolver : ICardServiceStrategyResolver
{
    private readonly IReadOnlyDictionary<ServiceType,ICardServiceStrategy> _strategies;

    public CardServiceStrategyResolver(IEnumerable<ICardServiceStrategy> strategies)
    {
        ArgumentNullException.ThrowIfNull(strategies);

        var strategyArray = strategies.ToArray();

        var duplicateStrategy = strategyArray.GroupBy(x => x.Type).FirstOrDefault(x => x.Count() > 1);

        if (duplicateStrategy != null)
        {
            throw new InvalidOperationException($"More than one strategy registered for " + $"{duplicateStrategy.Key}.");
        }

        _strategies = strategyArray.ToDictionary(x => x.Type);
    }

    public ICardServiceStrategy Resolve(ServiceType serviceType)
    {
        if (_strategies.TryGetValue(serviceType,out var strategy))
        {
            return strategy;
        }

        throw new NotSupportedException($"No card service strategy registered for " + $"{serviceType}.");
    }
}
