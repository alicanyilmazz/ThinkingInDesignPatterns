using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Commands
{
    public class BankOperationInvoker
    {
        public void ExecuteCommand(ICommand command)
        {
            Console.WriteLine("Banka işlemi alındı.");

            command.Execute();

            Console.WriteLine("Banka işlemi tamamlandı.");
        }
    }
}
