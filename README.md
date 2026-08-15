# Design Patterns

> The **Builder Pattern** is commonly used in enterprise applications and .NET. You will frequently encounter builder-style APIs such as `WebApplicationBuilder`, `HostBuilder`, and `ConfigurationBuilder`.

```diff
- Builder Pattern

Let's understand the problem first.

Suppose we have a User class.
```

```csharp
public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}
```

```diff
@@ Now let's create a User object. @@
```

```csharp
var user = new User
{
    Name = "Ali",
    Surname = "Yılmaz",
    Age = 28,
    Email = "ali@test.com",
    Phone = "5551112233",
    Address = "Istanbul",
    IsAdmin = false,
    IsActive = true
};
```

```diff
@@ So far, there is no problem. @@
```

```diff
@@ But what if we use a constructor? @@
```

```csharp
public User(
    string name,
    string surname,
    int age,
    string email,
    string phone,
    string address,
    bool isAdmin,
    bool isActive)
{
}
```

```diff
@@ Usage @@
```

```csharp
var user = new User(
    "Ali",
    "Yılmaz",
    28,
    "ali@test.com",
    "5551112233",
    "Istanbul",
    false,
    true);
```

```diff
@@ Here we encounter the first problem. @@

@@ Look at these values: @@
```

```csharp
false,
true
```

```diff
@@ Which one represents IsAdmin? @@

@@ Which one represents IsActive? @@

@@ It is not obvious just by looking at the call. @@

@@ And what happens if the constructor has 20 parameters? @@
```

```csharp
new User(...20 parameters...);
```

```diff
@@ The code becomes difficult to read and maintain. @@

@@ This is commonly associated with the "Telescoping Constructor" problem. @@

@@ The Builder Pattern is one way to solve this problem. @@
```

---

> ## Builder Pattern — Core Idea

```diff
@@ A Builder constructs a complex object step by step. @@

@@ Instead of writing: @@
```

```csharp
new User(...);
```

```diff
@@ We can write: @@
```

```csharp
var user = new UserBuilder()
    .WithName("Ali")
    .WithSurname("Yılmaz")
    .WithAge(28)
    .WithEmail("ali@test.com")
    .Build();
```

```diff
@@ The code is now much easier to understand. @@
```

---

> ## Our First Builder

### User

```csharp
public class User
{
    public string Name { get; set; }

    public string Email { get; set; }
}
```

### UserBuilder

```csharp
public class UserBuilder
{
    private readonly User _user = new User();

    public UserBuilder WithName(string name)
    {
        _user.Name = name;

        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;

        return this;
    }

    public User Build()
    {
        return _user;
    }
}
```

```diff
@@ Usage @@
```

```csharp
var user =
    new UserBuilder()
        .WithName("Ali")
        .WithEmail("ali@test.com")
        .Build();
```

---

```diff
@@ Why do we use "return this"? @@

@@ Because it allows method chaining. @@
```

```csharp
.WithName(...)
.WithEmail(...)
.Build();
```

```diff
@@ This style is commonly called a Fluent API. @@
```

For example:

```csharp
builder.WithName("Ali");
```

```diff
@@ WithName() returns the same builder instance. @@

@@ Therefore, we can immediately call another method: @@
```

```csharp
.WithEmail(...)
```

The important part is:

```csharp
public UserBuilder WithName(string name)
{
    _user.Name = name;

    return this;
}
```

`this` represents the current `UserBuilder` instance.

Therefore:

```csharp
builder
    .WithName("Ali")
    .WithEmail("ali@test.com");
```

is conceptually similar to:

```csharp
builder.WithName("Ali");

builder.WithEmail("ali@test.com");
```

---

> ## Build()

```diff
@@ Build() is typically the final operation of a Builder. @@

@@ Its responsibility is to return the constructed object. @@
```

```csharp
public User Build()
{
    return _user;
}
```

The typical flow is:

```text
UserBuilder
     ↓
WithName()
     ↓
WithEmail()
     ↓
WithAge()
     ↓
Build()
     ↓
User
```

---

> ## Validation

```diff
@@ Validation can also be performed before returning the final object. @@

@@ For example, suppose Email is required. @@
```

```csharp
public User Build()
{
    if (string.IsNullOrWhiteSpace(_user.Email))
    {
        throw new InvalidOperationException("Email is required.");
    }

    return _user;
}
```

```diff
@@ Now this code will fail: @@
```

```csharp
new UserBuilder()
    .WithName("Ali")
    .Build();
```

Because `Email` was never provided.

```text
WithName("Ali")
       ↓
Build()
       ↓
Validate
       ↓
Email missing
       ↓
Exception
```

---

> ## Builder and Immutable Objects

```diff
@@ Builder is also useful when constructing immutable objects. @@
```

For example:

```csharp
public class User
{
    public string Name { get; }

    public string Email { get; }

    public User(
        string name,
        string email)
    {
        Name = name;
        Email = email;
    }
}
```

```diff
@@ There are no public setters. @@

@@ Once the object is created, its state cannot be changed through these properties. @@

@@ A Builder can collect the required values first and then create the immutable object. @@
```

For example:

```csharp
public class UserBuilder
{
    private string _name;
    private string _email;

    public UserBuilder WithName(string name)
    {
        _name = name;

        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;

        return this;
    }

    public User Build()
    {
        if (string.IsNullOrWhiteSpace(_email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        return new User(_name, _email);
    }
}
```

Usage:

```csharp
var user = new UserBuilder()
    .WithName("Ali")
    .WithEmail("ali@test.com")
    .Build();
```

```diff
@@ This is a better approach for immutable objects because the Builder @@
@@ does not need access to public setters. @@
```

---

> ## Where Is Builder Used in .NET?

```diff
@@ ASP.NET Core @@

@@ One of the most familiar examples is WebApplicationBuilder. @@
```

```csharp
var builder = WebApplication.CreateBuilder(args);
```

Then we configure different parts of the application:

```csharp
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Logging.AddConsole();
```

And eventually:

```csharp
var app = builder.Build();
```

The general idea is:

```text
CreateBuilder()
      ↓
Configure Services
      ↓
Configure Logging
      ↓
Configure Configuration
      ↓
Build()
      ↓
WebApplication
```

---

> ## ConfigurationBuilder

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();
```

```diff
@@ Again, configuration is built step by step. @@
```

```text
ConfigurationBuilder
        ↓
AddJsonFile()
        ↓
AddEnvironmentVariables()
        ↓
AddUserSecrets()
        ↓
Build()
        ↓
IConfiguration
```

---

> ## HostBuilder

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register services
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .Build();
```

Again:

```text
CreateDefaultBuilder()
        ↓
ConfigureServices()
        ↓
ConfigureLogging()
        ↓
Build()
        ↓
IHost
```

---

> ## What About StringBuilder?

```csharp
var sb = new StringBuilder();

sb.Append("Ali");

sb.Append(" ");

sb.Append("Yılmaz");

Console.WriteLine(sb.ToString());
```

```diff
@@ StringBuilder also incrementally constructs a result, @@
@@ so it demonstrates a builder-like idea. @@

@@ However, it should not be treated as a textbook GoF Builder Pattern implementation. @@

@@ WebApplicationBuilder, ConfigurationBuilder, and HostBuilder @@
@@ are better examples when discussing Builder-style APIs in modern .NET. @@
```

---

> ## Advantages

```diff
@@ Advantages @@

✅ Makes complex object construction easier to understand.

✅ Avoids constructors with too many parameters.

✅ Improves readability.

✅ Works naturally with Fluent APIs.

✅ Can be useful when creating immutable objects.

✅ Validation can be performed before the final object is returned.

✅ Complex construction logic can be separated from the object itself.
```

---

> ## Disadvantages

```diff
@@ Disadvantages @@

❌ Can be unnecessary for small and simple classes.

❌ Introduces additional classes and code.

❌ Creating a Builder for every model can create unnecessary complexity.

❌ For very simple objects, using "new" or an object initializer is usually easier.
```

---

> ## Builder vs Factory

```diff
@@ Builder @@

> "How should I construct this complex object?"

The object is usually constructed step by step.

@@ Factory @@

> "Which object should I create?"

The Factory usually decides which concrete implementation should be created.
```

For example:

```text
CreditCardFactory
       ↓
Visa or MasterCard?
       ↓
Select concrete implementation
```

Whereas:

```text
JournalBuilder
       ↓
Set TransactionId
       ↓
Set TerminalId
       ↓
Set Amount
       ↓
Set TransactionDate
       ↓
Build()
       ↓
Journal
```

So the key distinction is:

```text
Factory
   ↓
Which object?

Builder
   ↓
How do I construct the object?
```

> **Important:** Builder and Factory are both creational design patterns, but they solve different object-creation problems.

---

> ## Interview Question — Why `return this`?

```diff
@@ Question @@

Why is "return this" commonly used in Builder methods?

@@ Answer @@

To support method chaining / Fluent API syntax.

Each configuration method returns the current Builder instance,
allowing another Builder method to be called immediately.
```

Example:

```csharp
return this;
```

allows us to write:

```csharp
new UserBuilder()
    .WithName("Ali")
    .WithEmail("ali@test.com")
    .Build();
```

instead of:

```csharp
var builder = new UserBuilder();

builder.WithName("Ali");

builder.WithEmail("ali@test.com");

var user = builder.Build();
```

---

# Interview Questions

### Question 1

What is one of the main problems that the Builder Pattern helps solve?

**A)** Reducing the number of objects

**B)** Simplifying the construction of complex objects and avoiding constructors with too many parameters

**C)** Improving database performance

**D)** Reducing memory consumption

**✅ Answer: B**

---

### Question 2

Which of the following best describes the Builder Pattern?

**A)** It always creates an object in a single operation.

**B)** It constructs a complex object step by step.

**C)** It guarantees that only one instance exists.

**D)** It eliminates the need for interfaces.

**✅ Answer: B**

---

### Question 3

Why is `return this` commonly used in Builder methods?

**A)** To improve runtime performance

**B)** To trigger the Garbage Collector

**C)** To enable method chaining and Fluent API syntax

**D)** To make the Builder thread-safe

**✅ Answer: C**

---

### Question 4

Which of the following is a well-known Builder-style API in modern .NET?

**A)** `WebApplicationBuilder`

**B)** `SqlConnection`

**C)** `HttpClient`

**D)** `List<T>`

**✅ Answer: A**

---

### Question 5

What is the main conceptual difference between Builder and Factory?

**A)** They are exactly the same pattern.

**B)** Builder focuses on constructing an object step by step, while Factory focuses on deciding which object to create.

**C)** Factory is only used for databases.

**D)** Builder can only be used with immutable classes.

**✅ Answer: B**

---

# Summary

```diff
@@ BUILDER PATTERN @@

Complex Object
      ↓
Builder
      ↓
Step 1
      ↓
Step 2
      ↓
Step 3
      ↓
Build()
      ↓
Final Object
```

> **Builder Pattern separates the construction of a complex object from its final representation and allows the object to be constructed step by step.**

Remember these three points for interviews:

```text
1. Step-by-step object construction

2. Avoid complex / long constructors

3. Fluent API through method chaining
```

And the classic comparison:

```text
Builder → How do I construct it?

Factory → Which object do I create?
```
