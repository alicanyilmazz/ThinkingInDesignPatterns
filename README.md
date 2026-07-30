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
@@  @@
```

```c#

```
