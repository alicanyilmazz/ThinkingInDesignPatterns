using System;
using System.Collections.Generic;
using System.Text;

namespace BehavioralPatterns.Strategy.Strategies;

public sealed class CommissionCalculator
{
    private readonly ICommissionStrategy _commissionStrategy;

    public CommissionCalculator(ICommissionStrategy commissionStrategy)
    {
        _commissionStrategy = commissionStrategy;
    }

    public decimal Calculate(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Tutar sıfırdan büyük olmalıdır.");
        }

        return _commissionStrategy.Calculate(amount);
    }
}