using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.YouTube;

// OBSERVER
public interface ISubscriber
{
    void Update(string channelName, string videoTitle);
}
