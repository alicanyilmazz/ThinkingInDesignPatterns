using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.YouTube;

// CONCRETE OBSERVER
public class Subscriber : ISubscriber
{
    public string Name { get; }

    public Subscriber(string name)
    {
        Name = name;
    }

    public void Update(string channelName, string videoTitle)
    {
        Console.WriteLine($"{Name} received notification: " + $"{channelName} uploaded '{videoTitle}'.");
    }
}