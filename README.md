# DesignPatterns

> Builder Pattern, bankacılık ve enterprise projelerde gerçekten kullanılan bir pattern. Özellikle .NET'in kendisinde (HostBuilder, WebApplicationBuilder, StringBuilder, ConfigurationBuilder) sürekli karşına çıkar.

```diff
- Builder Pattern

Önce problemi anlayalım

Elimizde bir **User** sınıfı olsun.

```

```c#
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
@@ Şimdi bunu oluşturuyoruz. @@
```
 
```c#
var user = new User
{
    Name = "Ali",
    Surname = "Yılmaz",
    Age = 28,
    Email = "ali@test.com",
    Phone = "5551112233",
    Address = "İstanbul",
    IsAdmin = false,
    IsActive = true
};
```

```diff
@@ Şimdilik sorun yok. @@
```

```diff
@@ Peki ya constructor kullanırsak? @@
```

```c#
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
@@ Kullanımı @@
```

```c#
var user = new User(
    "Ali",
    "Yılmaz",
    28,
    "ali@test.com",
    "5551112233",
    "İstanbul",
    false,
    true);
```

```diff
@@ Burada ilk problem geliyor. @@
@@ Şuna bakınca @@
```

```c#
false,
true
```

```diff
@@ hangisi Active? @@
@@ hangisi Admin? @@
@@ Belli değil. @@

@@ Bir de 20 parametre olursa... ? @@
```

```c#
new User(...20 tane parametre...)
```
```diff
@@ okunamaz hale gelir. @@
@@ Buna `Telescoping Constructor` Problem denir. @@
@@ `Builder Pattern` bunun için ortaya çıkmıştır. @@
```

> Builder'ın mantığı

```diff
@@ Nesneyi adım adım olusturur @@

@@ Bunun yerine @@
```

```c#
new User(...)
```

```diff
@@ şöyle yazarsın @@
```


```c#
var user = new UserBuilder()
    .WithName("Ali")
    .WithSurname("Yılmaz")
    .WithAge(28)
    .WithEmail("ali@test.com")
    .Build();
```

```diff
@@ Kod artık okunuyor. @@
```
________________________________________________________


> İlk Builder

```c#
public class User
{
    public string Name { get; set; }

    public string Email { get; set; }
}
```

> Builder

```c#
public class UserBuilder
{
    private User _user = new User();

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
@@ Kullanımı @@
```

```c#
var user =
    new UserBuilder()
        .WithName("Ali")
        .WithEmail("ali@test.com")
        .Build();
```

```diff
@@ Neden return this? @@
@@ Şu yüzden @@
```


```c#
.WithName(...)
.WithEmail(...)
.Build();
```

```diff
@@ zincirleme (Fluent API) yazabilmek için. @@
@@ Mesela @@
```

```c#
builder.WithName("Ali");
```


```diff
@@ buradan @@
@@ builder'ın kendisi geri dönüyor. @@
@@ Sonra @@
```

```c#
.WithEmail(...)
```
```diff
@@ çağrılabiliyor. @@
```

```diff
> @@ Build() @@
@@ Her Builder'ın sonunda vardır. @@
@@ Görevi oluşturulan nesneyi teslim etmektir.@@
```

```c#
public User Build()
{
    return _user;
}
```
__________________________________________

```diff
@@ Validation da yapılabilir @@
@@ Mesela Email zorunlu olsun. @@
```

```c#
public User Build()
{
    if(string.IsNullOrWhiteSpace(_user.Email))
        throw new Exception("Email boş.");

    return _user;
}
```

```diff
@@ Artık @@
```
```c#
new UserBuilder()
    .WithName("Ali")
    .Build();
```
```diff
@@ çalışmaz. @@
```
__________________________________________

```diff
@@ Immutable class oluşturmak için de kullanılır @@
@@ Mesela @@
```
```c#
public class User
{
    public string Name { get; }

    public string Email { get; }

    public User(
        string name,
        string email)
    {
        Name=name;

        Email=email;
    }
}
```

```diff
@@ Setter yok. @@
@@ Ama Builder yine oluşturabilir. @@
```
__________________________________________

```diff
@@ Builder nerelerde kullanılır? @@
@@ ASP.NET Core @@
@@ En büyük örnek @@
```
```c#
var builder = WebApplication.CreateBuilder(args);
```
```diff
@@ sonra @@
```

```c#
builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Configuration.AddJsonFile(...);

builder.Logging.AddConsole();
```
```diff
@@ En sonunda @@
```

```c#
var app = builder.Build();
```

```diff
@@ ConfigurationBuilder @@
```

```diff
@@ ConfigurationBuilder @@
```

```c#
new ConfigurationBuilder()
.AddJsonFile(...)
.AddEnvironmentVariables()
.AddUserSecrets()
.Build();
```

```diff
@@ HostBuilder @@
```

```c#
Host.CreateDefaultBuilder()
.ConfigureServices(...)
.ConfigureLogging(...)
.Build();
```

```diff
@@ StringBuilder @@
```

```c#
var sb = new StringBuilder();

sb.Append("Ali");

sb.Append(" ");

sb.Append("Yılmaz");

Console.WriteLine(sb.ToString());
```
__________________________________________
```diff
@@ Avantajları @@
✅ Constructor karmaşasını kaldırır.
✅ Okunabilirliği artırır.
✅ Fluent API sağlar.
✅ Immutable class'larda çok kullanılır.
✅ Validation eklenebilir.
```
__________________________________________
```diff
@@ Dezavantajları @@
❌ Küçük sınıflar için gereksiz olabilir.
❌ Her model için ekstra Builder sınıfı yazılır.
❌ Çok basit nesnelerde new kullanmak daha pratiktir.
```

```diff
@@ Builder ile Factory arasındaki fark @@
@@ Builder: @@
> "Nesneyi nasıl oluşturacağım?" (Adım adım oluşturma)
@@ Factory: @@
> "Hangi nesneyi oluşturacağım?" (Doğru sınıfı seçme)

CreditCardFactory → Visa mı MasterCard mı dönecek?
JournalBuilder → Journal nesnesinin alanlarını tek tek dolduracak.
```

```diff
@@ Soru: return this neden kullanılır? @@
> Cevap: Fluent API (method chaining) sağlamak için. Böylece ardışık metot çağrıları yapılabilir ve kod daha okunabilir olur.
```
