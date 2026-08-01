# DesignPatterns

```diff
Factory Method ile Abstract Factory farkı
Factory Method

Genellikle tek ürün ailesinden bir nesne oluşturur:
```
```diff
Abstract Factory

Birbiriyle ilişkili birden fazla nesneyi birlikte üretir:
```

```c#
Windows UI Factory
  ├─ WindowsButton
  └─ WindowsCheckbox

Mac UI Factory
  ├─ MacButton
  └─ MacCheckbox
```
```diff
Factory Method → Genellikle tek ürün
Abstract Factory → İlişkili ürün ailesi
```
