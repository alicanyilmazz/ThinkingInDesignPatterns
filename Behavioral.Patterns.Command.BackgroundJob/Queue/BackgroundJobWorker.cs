using Behavioral.Patterns.Command.BackgroundJob.Commands.Abstracts;

namespace Behavioral.Patterns.Command.BackgroundJob.Queue;

public class BackgroundJobWorker
{
    private readonly BackgroundJobQueue _queue;

    public BackgroundJobWorker(BackgroundJobQueue queue)
    {
        _queue = queue;
    }

    public void Run()
    {
        Console.WriteLine();
        Console.WriteLine("Background Worker başladı.");

        Console.WriteLine();


        while (_queue.Count > 0)
        {
            ICommand? command = _queue.Dequeue();


            if (command == null)
            {
                continue;
            }


            Console.WriteLine($"Çalıştırılıyor: {command.GetType().Name}");

            command.Execute();

            Console.WriteLine("Job tamamlandı.");

            Console.WriteLine("----------------------------");
        }


        Console.WriteLine("Queue boş. Worker durdu.");
    }
}