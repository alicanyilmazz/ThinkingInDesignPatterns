using System;
using System.Collections.Generic;
using System.Text;

namespace Creational.Pattern.Prototype;

public class GameCharacter : IPrototype<GameCharacter>
{
    public string Name { get; set; } = string.Empty;

    public int Health { get; set; }

    public int Level { get; set; }

    public Weapon Weapon { get; set; } = new();

    public Armor Armor { get; set; } = new();

    public List<string> Skills { get; set; } = new();

    public GameCharacter Clone()
    {
        return new GameCharacter
        {
            Name = Name,
            Health = Health,
            Level = Level,

            Weapon = Weapon.Clone(),

            Armor = Armor.Clone(),

            Skills = new List<string>(Skills)
        };
    }

    public void Display()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Level: {Level}");
        Console.WriteLine($"Health: {Health}");
        Console.WriteLine($"Weapon: {Weapon.Name} - Damage: {Weapon.Damage}");
        Console.WriteLine($"Armor: {Armor.Name} - Defense: {Armor.Defense}");
        Console.WriteLine($"Skills: {string.Join(", ", Skills)}");
        Console.WriteLine();
    }
}
