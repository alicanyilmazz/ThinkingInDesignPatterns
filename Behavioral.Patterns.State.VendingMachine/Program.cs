using Behavioral.Patterns.State.VendingMachine;

var machine = new VendingMachine();

// Para yatırmadan ürün seçmeye çalış
machine.SelectProduct("Cola");

Console.WriteLine();

// Para yatır
machine.InsertMoney(20);

// Yetersiz bakiye
machine.SelectProduct("Cola");

Console.WriteLine();

// Biraz daha para yatır
machine.InsertMoney(20);

// Cola seç
machine.SelectProduct("Cola");

// Ürünü ver
machine.Dispense();

Console.WriteLine();
Console.WriteLine("--------------------");
Console.WriteLine();

// Yeni işlem
machine.InsertMoney(50);

// İşlemi iptal et
machine.Cancel();

Console.ReadLine();