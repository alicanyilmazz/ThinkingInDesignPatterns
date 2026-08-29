using ObserverPattern.YouTube;

var channel = new YouTubeChannel("Design Patterns Academy");

var alican = new Subscriber("Alican");
var john = new Subscriber("John");
var sarah = new Subscriber("Sarah");

channel.Subscribe(alican);
channel.Subscribe(john);
channel.Subscribe(sarah);

channel.UploadVideo("Observer Design Pattern");

Console.WriteLine();

channel.Unsubscribe(john);

channel.UploadVideo("State Design Pattern");

Console.ReadLine();
