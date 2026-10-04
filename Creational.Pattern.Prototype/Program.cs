using Creational.Pattern.Prototype;

var knightPrototype = new GameCharacter
{
    Name = "Knight",
    Level = 10,
    Health = 100,

    Weapon = new Weapon
    {
        Name = "Long Sword",
        Damage = 50
    },

    Armor = new Armor
    {
        Name = "Steel Armor",
        Defense = 75
    },

    Skills =
    [
        "Slash",
        "Shield Block",
        "Charge"
    ]
};

Console.WriteLine("ORIGINAL PROTOTYPE");
Console.WriteLine("------------------");

knightPrototype.Display();


var darkKnight = knightPrototype.Clone();

darkKnight.Name = "Dark Knight";
darkKnight.Level = 15;

darkKnight.Weapon.Name = "Dark Sword";
darkKnight.Weapon.Damage = 80;

darkKnight.Armor.Name = "Dark Armor";
darkKnight.Armor.Defense = 100;

darkKnight.Skills.Add("Dark Strike");


Console.WriteLine("CLONED CHARACTER");
Console.WriteLine("----------------");

darkKnight.Display();


Console.WriteLine("ORIGINAL PROTOTYPE AFTER CLONE MODIFICATION");
Console.WriteLine("-------------------------------------------");

knightPrototype.Display();