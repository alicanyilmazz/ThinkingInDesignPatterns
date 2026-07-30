# DesignPatterns

> Builder Pattern, bankacılık ve enterprise projelerde gerçekten kullanılan bir pattern. Özellikle .NET'in kendisinde (HostBuilder, WebApplicationBuilder, StringBuilder, ConfigurationBuilder) sürekli karşına çıkar.

```diff
- Builder Pattern

Önce problemi anlayalım

Elimizde bir **User** sınıfı olsun.

```

```
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
- This is a red colored line
+ This is a green colored line
@@ This is a purple colored line @@
```
