// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;




#region Direct Exchange

//DirectExchange();

#endregion

#region Topic Exchange

//TopicExchange();

#endregion

#region Header Exchange

//HeaderExchange();

#endregion

#region Complex Type Mesaj Göndermek
ComplexTypeMessageType();
#endregion
Console.ReadLine();


#region Complex Type Mesaj Göndermek

void ComplexTypeMessageType()
{
    var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };
    using var connection = factory.CreateConnection();

    var channel = connection.CreateModel();

    channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

    Dictionary<string, object> headers = new Dictionary<string, object>();
    headers.Add("format", "pdf");
    headers.Add("shape2", "a4");

    var properties = channel.CreateBasicProperties();
    properties.Headers = headers;

    //Mesajları kalıcı hale getirmek
    properties.Persistent = true;


    var product = new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 2 };
    var productJsonString = JsonSerializer.Serialize(product);

    channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));
    Console.WriteLine("Mesaj gönderilmiştir.");
}


#endregion

#region Header Exchange

void HeaderExchange()
{
    var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };
    using var connection = factory.CreateConnection();

    var channel = connection.CreateModel();

    channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

    Dictionary<string, object> headers = new Dictionary<string, object>();
    headers.Add("format", "pdf");
    headers.Add("shape2", "a4");

    var properties = channel.CreateBasicProperties();
    properties.Headers = headers;

    //Mesajları kalıcı hale getirmek
    properties.Persistent = true;

    channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("Header Mesajım"));
    Console.WriteLine("Mesaj gönderilmiştir.");
}


#endregion

#region Topic Exchange

void TopicExchange()
{
    var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };
    using var connection = factory.CreateConnection();

    var channel = connection.CreateModel();

    channel.ExchangeDeclare("log-topic", durable: true, type: ExchangeType.Topic);

    int count = 1;
    Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
        LogNames log1 = (LogNames)new Random().Next(1, 5);
        LogNames log2 = (LogNames)new Random().Next(1, 5);
        LogNames log3 = (LogNames)new Random().Next(1, 5);
        var routeKey = $"{log1}.{log2}.{log3}";

        string message = $"{count}. Log Type :  {log1}-{log2}-{log3}";

        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("log-topic", routeKey, null, messageBody);

        Console.WriteLine($"{count}. Log kuyruğa gönderildi. Log : {message}");
        count += 1;
    });
}


#endregion


#region Direct Exchange

void DirectExchange()
{
    var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };
    using var connection = factory.CreateConnection();

    var channel = connection.CreateModel();

    channel.ExchangeDeclare("log-direct", durable: true, type: ExchangeType.Direct);

    //kuyruk oluşturma
    Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
    {
        var queueName = $"direct-queue-{x}";
        channel.QueueDeclare(queueName, true, false, false);
        var routeKey = $"route-{x}";
        channel.QueueBind(queueName, "log-direct", routeKey, null);
    });


    int count = 1;
    Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
        LogNames logName = (LogNames)new Random().Next(1, 5);
        string message = $"log type :  {logName}";
        var messageBody = Encoding.UTF8.GetBytes(message);

        var routeKey = $"route-{logName}";

        channel.BasicPublish("log-direct", routeKey, null, messageBody);

        Console.WriteLine($"{count}. Log kuyruğa gönderildi. Log : {message}");
        count += 1;
    });
}


#endregion





#region DefaultExchange
void DefaultExhange()
{
    //Connection kurulması
    var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };

    using var connection = factory.CreateConnection();

    //Channel
    var channel = connection.CreateModel();

    //Kuruk oluşturma
    //Default exchange
    channel.QueueDeclare("hello-queue", true, false, false);
    Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
        string message = $"Message {x}";
        var messageBody = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
        Console.WriteLine($"Mesaj kuyruğa gönderildi. Mesaj : {message}");

    });

}

#endregion

#region Fanout Exhange

void FanoutExchange()
{
    //Connection kurulması
    var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };

    using var connection = factory.CreateConnection();

    //Channel
    var channel = connection.CreateModel();
    channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

    Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
        string message = $"Logs {x}";
        var messageBody = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("logs-fanout", "", null, messageBody);
        Console.WriteLine($"Mesaj kuyruğa gönderildi. Mesaj : {message}");

    });
}

#endregion










public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4,
}

