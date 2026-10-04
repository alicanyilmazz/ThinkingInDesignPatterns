using System;
using System.Collections.Generic;
using System.Text;

namespace Creational.Pattern.Prototype;

public class Armor
{
    public string Name { get; set; } = string.Empty;

    public int Defense { get; set; }

    public Armor Clone()
    {
        return new Armor
        {
            Name = Name,
            Defense = Defense
        };
    }
}