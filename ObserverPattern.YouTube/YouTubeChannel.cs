using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.YouTube;

// CONCRETE SUBJECT
public class YouTubeChannel : IYouTubeChannel
{
    private readonly List<ISubscriber> _subscribers = new();

    private string _latestVideoTitle;

    public string ChannelName { get; }

    public YouTubeChannel(string channelName)
    {
        ChannelName = channelName;
    }

    public void Subscribe(ISubscriber subscriber)
    {
        _subscribers.Add(subscriber);

        Console.WriteLine("New subscriber added.");
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber);

        Console.WriteLine("Subscriber removed.");
    }

    public void UploadVideo(string videoTitle)
    {
        _latestVideoTitle = videoTitle;

        Console.WriteLine();
        Console.WriteLine($"{ChannelName} uploaded a new video: {videoTitle}");

        NotifySubscribers();
    }

    public void NotifySubscribers()
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Update(ChannelName, _latestVideoTitle);
        }
    }
}
