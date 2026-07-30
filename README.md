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
@@  @@
```

```c#

```
