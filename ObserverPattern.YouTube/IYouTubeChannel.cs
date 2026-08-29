using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.YouTube;

// SUBJECT
public interface IYouTubeChannel
{
    void Subscribe(ISubscriber subscriber);

    void Unsubscribe(ISubscriber subscriber);

    void NotifySubscribers();
}