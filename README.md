# Prototype Design Pattern

The **Prototype Pattern** is a **Creational Design Pattern** that creates new objects by copying an existing object instead of constructing a new object from scratch.

The existing object acts as a **prototype**.

```text
Existing Object
      |
      | Clone()
      v
   New Object
```

The main idea is:

> Instead of rebuilding a complex object every time, create a configured prototype once and clone it when a similar object is needed.

---

# The Problem

Imagine a game where creating a character requires many properties:

```text
GameCharacter
 ├── Name
 ├── Level
 ├── Health
 ├── Weapon
 ├── Armor
 └── Skills
```

Creating every character manually could look like this:

```csharp
var knight1 = new GameCharacter
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
```

If we need another similar character, we may end up repeating almost the same initialization code:

```csharp
var knight2 = new GameCharacter
{
    Name = "Dark Knight",
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
```

Most of the configuration is identical.

Prototype solves this by allowing us to create a configured object once and clone it.

```csharp
var darkKnight = knightPrototype.Clone();

darkKnight.Name = "Dark Knight";
```

---

# The Solution

First, we define a Prototype abstraction:

```csharp
public interface IPrototype<T>
{
    T Clone();
}
```

Classes that support cloning implement this interface.

```text
             IPrototype<T>
                   |
                   v
             GameCharacter
                   |
                 Clone()
                   |
                   v
          New GameCharacter
```

The client does not need to know how the object is reconstructed.

The object itself knows how to create its copy.

---

# Example — Game Character

Consider the following character:

```text
Knight Prototype

Name    : Knight
Level   : 10
Health  : 100

Weapon
 └── Long Sword
     └── Damage: 50

Armor
 └── Steel Armor
     └── Defense: 75

Skills
 ├── Slash
 ├── Shield Block
 └── Charge
```

Instead of reconstructing this configuration every time, we can use it as a prototype.

---

## Prototype Interface

```csharp
public interface IPrototype<T>
{
    T Clone();
}
```

---

## Weapon

```csharp
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
```

---

## Armor

```csharp
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
```

---

## GameCharacter

```csharp
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
}
```

Now we can create a prototype:

```csharp
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
```

And create another character by cloning it:

```csharp
var darkKnight = knightPrototype.Clone();

darkKnight.Name = "Dark Knight";
darkKnight.Level = 15;

darkKnight.Weapon.Name = "Dark Sword";
darkKnight.Weapon.Damage = 80;

darkKnight.Armor.Name = "Dark Armor";
darkKnight.Armor.Defense = 100;

darkKnight.Skills.Add("Dark Strike");
```

The result:

```text
Knight Prototype
-----------------
Name: Knight
Level: 10
Health: 100
Weapon: Long Sword
Armor: Steel Armor
Skills:
  - Slash
  - Shield Block
  - Charge


Dark Knight
-----------
Name: Dark Knight
Level: 15
Health: 100
Weapon: Dark Sword
Armor: Dark Armor
Skills:
  - Slash
  - Shield Block
  - Charge
  - Dark Strike
```

The original prototype remains unchanged.

---

# Shallow Copy vs Deep Copy

One of the most important concepts when implementing the Prototype Pattern is understanding the difference between:

```text
Shallow Copy
```

and:

```text
Deep Copy
```

This becomes especially important when an object contains **reference-type properties** such as:

```csharp
Weapon
Armor
List<string>
Address
Configuration
```

---

# Shallow Copy

A **shallow copy** creates a new top-level object but copies references to nested reference-type objects.

Consider:

```csharp
public class GameCharacter
{
    public string Name { get; set; }

    public Weapon Weapon { get; set; }
}
```

Imagine:

```text
Original Character
        |
        v
   Weapon Object
```

After a shallow copy:

```text
Original Character ----\
                        \
                         ---> Weapon Object
                        /
Cloned Character ------/
```

There are two different `GameCharacter` instances, but both reference the **same Weapon object**.

For example:

```csharp
var clone = original.ShallowClone();

clone.Weapon.Name = "Dark Sword";
```

This may unexpectedly also change:

```csharp
original.Weapon.Name
```

because both objects reference the same instance.

---

## Example of Shallow Copy

In C#, `MemberwiseClone()` creates a shallow copy.

```csharp
public GameCharacter ShallowClone()
{
    return (GameCharacter)MemberwiseClone();
}
```

Consider:

```csharp
var original = new GameCharacter
{
    Name = "Knight",

    Weapon = new Weapon
    {
        Name = "Long Sword",
        Damage = 50
    }
};

var clone = original.ShallowClone();

clone.Name = "Dark Knight";

clone.Weapon.Name = "Dark Sword";
```

Changing:

```csharp
clone.Name
```

does not affect the original because the two `GameCharacter` instances are different.

But changing:

```csharp
clone.Weapon.Name
```

does affect the original weapon.

Why?

Because:

```csharp
ReferenceEquals(
    original.Weapon,
    clone.Weapon)
```

returns:

```text
True
```

The structure looks like:

```text
Original
   |
   +-- Name = "Knight"
   |
   +------\
           \
            Weapon
           /
   +------/
   |
Clone
   |
   +-- Name = "Dark Knight"
```

This is **Shallow Copy**.

---

# Deep Copy

A **deep copy** creates a completely independent copy of the object and its nested mutable objects.

Instead of:

```text
Original ------\
                ---> Weapon
Clone ---------/
```

we create:

```text
Original
   |
   v
Weapon 1


Clone
   |
   v
Weapon 2
```

Now the cloned object has its own `Weapon`.

Changing it does not affect the original.

---

## Deep Copy Example

Our `GameCharacter.Clone()` performs a deep copy:

```csharp
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
```

Notice these lines:

```csharp
Weapon = Weapon.Clone();
Armor = Armor.Clone();
Skills = new List<string>(Skills);
```

We do **not** write:

```csharp
Weapon = Weapon;
Armor = Armor;
Skills = Skills;
```

because that would copy the references.

Instead, new objects are created.

---

# Proving the Deep Copy

After cloning:

```csharp
var darkKnight = knightPrototype.Clone();
```

we can verify that the nested objects are different:

```csharp
Console.WriteLine(
    ReferenceEquals(
        knightPrototype,
        darkKnight));
```

Result:

```text
False
```

The characters are different objects.

Now check the weapon:

```csharp
Console.WriteLine(
    ReferenceEquals(
        knightPrototype.Weapon,
        darkKnight.Weapon));
```

Result:

```text
False
```

And the skills collection:

```csharp
Console.WriteLine(
    ReferenceEquals(
        knightPrototype.Skills,
        darkKnight.Skills));
```

Result:

```text
False
```

Therefore:

```text
Knight Prototype
    |
    +--> Weapon 1
    +--> Armor 1
    +--> Skills List 1


Dark Knight
    |
    +--> Weapon 2
    +--> Armor 2
    +--> Skills List 2
```

Each character is independent.

---

# Shallow Copy vs Deep Copy

| Shallow Copy                            | Deep Copy                                                      |
| --------------------------------------- | -------------------------------------------------------------- |
| Creates a new top-level object          | Creates a new top-level object                                 |
| Nested references may be shared         | Nested mutable objects are copied                              |
| Usually cheaper                         | Usually more expensive                                         |
| Can cause unexpected shared state       | Objects are independent                                        |
| `MemberwiseClone()` is a common example | Requires explicit cloning logic or another deep-copy mechanism |

A simple rule:

> **Shallow Copy copies references.**

> **Deep Copy copies objects behind those references as well.**

---

# Value Types vs Reference Types

This distinction also explains why some properties are safe during a shallow copy.

Consider:

```csharp
public int Level { get; set; }

public int Health { get; set; }

public Weapon Weapon { get; set; }
```

`int` is a value type.

Therefore:

```text
Original.Level = 10
Clone.Level    = 10
```

each object contains its own value.

But `Weapon` is a reference type:

```text
Original.Weapon
      |
      v
 Weapon Object
```

A shallow copy copies that reference.

Therefore:

```text
Original.Weapon ----\
                     ---> Weapon Object
Clone.Weapon -------/
```

This is why reference-type properties require special attention when implementing Prototype.

---

# String Is a Reference Type — So Why Is It Usually Safe?

`string` is technically a reference type in C#.

However, strings are **immutable**.

For example:

```csharp
clone.Name = "Dark Knight";
```

does not modify the existing string object.

It assigns another string to `clone.Name`.

Therefore strings usually do not create the same shared-state problem as mutable objects such as:

```csharp
Weapon
List<T>
Dictionary<TKey, TValue>
Address
Configuration
```

---

# Why Use Prototype?

Prototype becomes useful when object creation is:

### Expensive

Creating an object may require expensive initialization.

```text
Load configuration
       ↓
Parse data
       ↓
Calculate defaults
       ↓
Initialize complex state
       ↓
Create object
```

Instead, initialize a prototype once:

```text
Expensive initialization
          |
          v
       Prototype
          |
        Clone
       /  |  \
      v   v   v
     A    B    C
```

---

### Complex

An object may require many configuration values:

```text
Character
 ├── Weapon
 ├── Armor
 ├── Skills
 ├── Stats
 ├── Abilities
 ├── Inventory
 └── Configuration
```

Instead of repeating the configuration, clone an existing template.

---

### Mostly Similar

Prototype works especially well when many objects share most of their configuration.

For example:

```text
Base Knight
     |
     +---- Clone ---> Fire Knight
     |
     +---- Clone ---> Dark Knight
     |
     +---- Clone ---> Ice Knight
```

Each variation only changes what is necessary.

---

# Prototype Registry

Prototype can also be combined with a registry.

Instead of manually keeping references to every prototype:

```csharp
var knight = knightPrototype.Clone();
```

we can register prototypes:

```csharp
registry.Register(
    "Knight",
    knightPrototype);

registry.Register(
    "Mage",
    magePrototype);

registry.Register(
    "Archer",
    archerPrototype);
```

Then request a character:

```csharp
var character =
    registry.Create("Knight");
```

Conceptually:

```text
             Prototype Registry
             /       |       \
            /        |        \
        Knight      Mage     Archer
           |
         Clone
           |
           v
      New Character
```

This can be useful when prototypes need to be selected dynamically at runtime.

---

# Prototype vs Creating With `new`

Without Prototype:

```csharp
var character = new GameCharacter
{
    // configure everything again
};
```

With Prototype:

```csharp
var character = knightPrototype.Clone();
```

The difference is not that Prototype eliminates `new`.

Internally, cloning may still use constructors or `new`.

The important difference is:

> The client does not reconstruct the object's entire configuration.

Instead, it asks an existing configured object to produce a copy.

---

# Prototype vs Factory

Prototype and Factory are both **Creational Patterns**, but they solve different problems.

### Factory

Factory decides **which object should be created**.

```text
Client
   |
   v
Factory
   |
   +--> Knight
   +--> Mage
   +--> Archer
```

Example:

```csharp
var character =
    characterFactory.Create(CharacterType.Knight);
```

---

### Prototype

Prototype creates an object by **copying an existing configured object**.

```text
Knight Prototype
       |
     Clone
       |
       v
 New Knight
```

Example:

```csharp
var character =
    knightPrototype.Clone();
```

A simple distinction:

> **Factory creates based on a type or rule.**

> **Prototype creates based on an existing object.**

They can also be used together.

---

# Prototype vs Builder

Builder is useful when a complex object needs to be constructed **step by step**.

```text
Builder
   |
   +--> Set Weapon
   +--> Set Armor
   +--> Set Health
   +--> Add Skills
   |
   v
Character
```

Prototype starts from an object that is already configured:

```text
Configured Character
        |
      Clone
        |
        v
New Character
```

So:

> **Builder builds.**

> **Prototype copies.**

---

# Advantages

### Reduces Repeated Initialization

Complex configurations do not need to be recreated every time.

### Can Reduce Creation Cost

Cloning may be cheaper than repeating expensive initialization.

### Easy Creation of Variations

A prototype can be cloned and customized:

```csharp
var darkKnight = knightPrototype.Clone();

darkKnight.Name = "Dark Knight";
darkKnight.Weapon = darkSword;
```

### Reduces Client Knowledge

The client does not need to understand every detail required to construct the object.

---

# Disadvantages

### Deep Copy Can Become Complicated

Consider:

```text
GameCharacter
    |
    +--> Inventory
    |       |
    |       +--> Items
    |              |
    |              +--> Enchantments
    |
    +--> Skills
    |
    +--> Equipment
```

Correctly deep-cloning a large object graph can become difficult.

### Circular References

Objects may reference each other:

```text
A --> B
^     |
|_____|
```

A naive cloning implementation can cause problems with cyclic object graphs.

### Shared Mutable State Bugs

Incorrect shallow copying can result in clones unexpectedly modifying each other.

### Clone Semantics Must Be Clear

Developers need to know whether `Clone()` means:

```text
Shallow Copy
```

or:

```text
Deep Copy
```

An unclear cloning contract can easily introduce bugs.

---

# When Should You Use Prototype?

Prototype is a good choice when:

* Object creation is expensive.
* Object initialization is complex.
* Many objects share almost the same configuration.
* You need runtime-configurable object templates.
* You want to create variants of an existing object.
* Client code should not depend on complex construction logic.

For example:

```text
Game Character Templates
Document Templates
UI Component Templates
Product Configurations
Report Templates
Workflow Definitions
Simulation Objects
```

---

# When Should You Not Use Prototype?

Prototype may be unnecessary when objects are very simple.

For example:

```csharp
var user = new User("Ali");
```

Introducing:

```csharp
user.Clone();
```

provides little benefit if object construction is already trivial.

Also be careful when the object contains a large and complicated mutable object graph.

In such cases, deep-cloning logic may become more complicated than simply constructing the object correctly.

---

# Key Takeaway

The Prototype Pattern creates new objects by copying existing configured objects.

```text
        Prototype
            |
          Clone
       /     |     \
      v      v      v
   Object1 Object2 Object3
```

The most important implementation detail is deciding whether cloning should be:

```text
Shallow Copy
```

or:

```text
Deep Copy
```

With shallow copy:

```text
Clone ------\
             ---> Shared Object
Original ---/
```

With deep copy:

```text
Original ---> Object A

Clone ------> Object B
```

For objects containing mutable reference types, deep copying is often required if the clone must be completely independent.

The simplest way to remember the pattern is:

> **Do not build the same complex object again — clone a configured prototype and customize the copy.**
