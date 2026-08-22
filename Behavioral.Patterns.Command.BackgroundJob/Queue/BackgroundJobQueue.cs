using Behavioral.Patterns.Command.BackgroundJob.Commands.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Command.BackgroundJob.Queue;

public class BackgroundJobQueue
{
    private readonly Queue<ICommand> _commands = new Queue<ICommand>();


    public void Enqueue(ICommand command)
    {
        _commands.Enqueue(command);

        Console.WriteLine($"Job Queue'ya eklendi: {command.GetType().Name}");
    }


    public ICommand? Dequeue()
    {
        if (_commands.Count == 0)
        {
            return null;
        }

        return _commands.Dequeue();
    }


    public int Count
    {
        get
        {
            return _commands.Count;
        }
    }
}
