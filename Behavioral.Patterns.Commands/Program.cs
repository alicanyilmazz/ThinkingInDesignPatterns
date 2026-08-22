using Behavioral.Patterns.Commands;

BankAccount sender = new BankAccount("TR001", "Alican", 10000);

BankAccount receiver = new BankAccount("TR002", "Mehmet", 5000);


BankService bankService = new BankService();


ICommand transferCommand = new TransferMoneyCommand(bankService, sender, receiver, 2500);


BankOperationInvoker invoker = new BankOperationInvoker();


Console.WriteLine("Transfer öncesi");

Console.WriteLine($"Alican: {sender.Balance:N2} TL");

Console.WriteLine($"Mehmet: {receiver.Balance:N2} TL");


Console.WriteLine("\n-----------------\n");


invoker.ExecuteCommand(transferCommand);


Console.WriteLine("\n-----------------\n");


Console.WriteLine("Transfer sonrası");

Console.WriteLine($"Alican: {sender.Balance:N2} TL");

Console.WriteLine($"Mehmet: {receiver.Balance:N2} TL");