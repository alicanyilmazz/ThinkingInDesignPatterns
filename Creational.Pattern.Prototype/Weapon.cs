using System;
using System.Collections.Generic;
using System.Text;

namespace Creational.Pattern.Prototype;

public class Weapon
{
    public string Name { get; set; } = string.Empty;

    public int Damage { get; set; }

    public Weapon Clone()
    {
        return new Weapon
        {
            Name = Name,
            Damage = Damage
        };
    }
}