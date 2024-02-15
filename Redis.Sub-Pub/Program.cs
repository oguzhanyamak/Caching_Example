using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("127.0.0.1:6379");

ISubscriber subscriber = connection.GetSubscriber();

await subscriber.SubscribeAsync("messageChannel", (chanel,value)=> {

    Console.WriteLine(string.Format("{0} isimli kanala {1} mesaj gelmiştir.",chanel,value));
    Console.ReadKey();
});