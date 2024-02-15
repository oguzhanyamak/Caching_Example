// See https://aka.ms/new-console-template for more information
using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("127.0.0.1:6379");

ISubscriber subscriber = connection.GetSubscriber();

var subscribers = await subscriber.PublishAsync("messageChannel", "lorem ipsum dolor sit amet");
Console.WriteLine("Mesaj : "+subscribers.ToString()+" Kullanıcıya İletildi");
Console.ReadKey();