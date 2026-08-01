using System;
using System.Collections.Generic;
using System.Text;

namespace CreationalPatterns.FactoryMethod.Notification;

public interface INotification
{
    void Send(string message);
}

public sealed class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"E-posta gönderildi: {message}");
    }
}

public sealed class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS gönderildi: {message}");
    }
}

public abstract class NotificationCreator
{
    protected abstract INotification CreateNotification();

    public void Notify(string message)
    {
        INotification notification = CreateNotification();

        notification.Send(message);
    }
}

public sealed class EmailNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification()
    {
        return new EmailNotification();
    }
}

public sealed class SmsNotificationCreator  : NotificationCreator
{
    protected override INotification CreateNotification()
    {
        return new SmsNotification();
    }
}


