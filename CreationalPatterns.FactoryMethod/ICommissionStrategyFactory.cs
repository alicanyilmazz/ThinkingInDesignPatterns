using System;
using System.Collections.Generic;
using System.Text;

namespace CreationalPatterns.FactoryMethod;

public interface ICommissionStrategyFactory
{
    ICommissionStrategy Create(CardType cardType);
}