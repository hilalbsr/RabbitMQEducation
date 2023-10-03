using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;



#region DefaultConsumer
//Bunu silersek kuyruk yoksa hata alır. Kuyruğun varlığına emin isek silebiliriz.
//Kuyruk yoksa oluşturur.
//Parametreler aynı olmalıdır
//channel.QueueDeclare("hello-queue", true, false, false);

//boyut --herhangi bir boyuttaki mesajı gönderebilirsin
//6 --kaç kaç mesajlar gelsin
//6,true -- tek bir seferde 6 olacak şekilde ayarlar. 3 birine 3 birine olarak ayarlar. Toplamdaki kadar
//6,false --her bir subscribe için kaç tane gönderiliceğini belirtir. 6 6 olacak şekilde ayarlanır.
//channel.BasicQos(0, 10, true);
//channel.BasicQos(0,1,false);

////consumer ya da subscribe
//var consumer = new EventingBasicConsumer(channel);


//hangi kuyruğu dinlicek.
//2.parametre-- true ise kuyruktan direk siliyor.Mesajın başarılı kayıt olması önemli değil. false -- ben sana haberdar edicem. Söylediğimde sil
//channel.BasicConsume("hello-queue", true, consumer);

//Sen silme mesajı. Ben sana haber vericem
//channel.BasicConsume("hello-queue", false, consumer);


//consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//{
//    var message = Encoding.UTF8.GetString(e.Body.ToArray());
//    Console.WriteLine("Gelen Mesaj :" + message);

//    Thread.Sleep(1500);
//    //Silmesi için consumer a haber verilmesi
//    //false olması diğer cache bekleyen mesajlar içinde haber verme. Ben sana dedikçe sen sil demek.
//    channel.BasicAck(e.DeliveryTag, false);
//};
#endregion


#region Fanout
////kuyruk oluşturucaz.Random olarak 3 instance
////var randomQueueName = channel.QueueDeclare().QueueName;

////Böyle yaparsak ilgili kuyruk durur. Sabit diskte kaydedilsin.
////Down olsa da consumer kuyruk kalacak.Kaldığı yerden devam edecek
//var randomQueueName = "log-database-save-queue";
//channel.QueueDeclare(randomQueueName, true, false, false);


////uygulama her ayağa kalktığında bir kuyruk oluşacak. Down olduğunda ilgili kuyruk silinecek.
//channel.QueueBind(randomQueueName, "logs-fanout", "", null);
//channel.BasicQos(0, 1, false);

//channel.BasicConsume(randomQueueName, false, consumer);

//Console.WriteLine("Loglar Yazdırılıyor..");


////Consumerlar aynı dataları alır.
//consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//{
//    var message = Encoding.UTF8.GetString(e.Body.ToArray());
//    Console.WriteLine("Gelen Mesaj :" + message);
//    Thread.Sleep(1500);
//    channel.BasicAck(e.DeliveryTag, false);
//};
#endregion



#region DirectExchange

//var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };

//using var connection = factory.CreateConnection();
//var channel = connection.CreateModel();

//channel.BasicQos(0, 1, false);

//var consumer = new EventingBasicConsumer(channel);
//var queueName = "direct-queue-Info";

//channel.BasicConsume(queueName, false, consumer);

//Console.WriteLine("Loglar dinleniyor..");

////Consumerlar aynı dataları alır.
//consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//{
//    var message = Encoding.UTF8.GetString(e.Body.ToArray());

//    Console.WriteLine("Gelen Mesaj :" + message);
//    //File.AppendAllText("log-critical.txt", message + "\n");

//    channel.BasicAck(e.DeliveryTag, false);
//};

#endregion


#region TopicExchange

//var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };

//using var connection = factory.CreateConnection();
//var channel = connection.CreateModel();

//channel.BasicQos(0, 1, false);

//var consumer = new EventingBasicConsumer(channel);
//var queueName = channel.QueueDeclare().QueueName;

////var routeKey = "*.Error.*";
////var routeKey = "*.*.Warning";
//var routeKey = "Info.#";

//channel.QueueBind(queueName, "log-topic", routeKey, null);

//channel.BasicConsume(queueName, false, consumer);

//Console.WriteLine("Loglar dinleniyor..");

////Consumerlar aynı dataları alır.
//consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//{
//    var message = Encoding.UTF8.GetString(e.Body.ToArray());
//    Thread.Sleep(1000);
//    Console.WriteLine("Gelen Mesaj :" + message);
//    channel.BasicAck(e.DeliveryTag, false);
//};

#endregion


#region HeaderExchange

//var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };

//using var connection = factory.CreateConnection();
//var channel = connection.CreateModel();

//channel.BasicQos(0, 1, false);

//channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

//var consumer = new EventingBasicConsumer(channel);
//var queueName = channel.QueueDeclare().QueueName;

//Dictionary<string,object> headers = new Dictionary<string, object>();
//headers.Add("format", "pdf");
//headers.Add("shape", "a4");
//headers.Add("x-match", "any");


//channel.QueueBind(queueName, "header-exchange", "", headers);
//channel.BasicConsume(queueName, false, consumer);

//Console.WriteLine("Loglar dinleniyor..");

////Consumerlar aynı dataları alır.
//consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//{
//    var message = Encoding.UTF8.GetString(e.Body.ToArray());
//    Thread.Sleep(1000);
//    Console.WriteLine("Gelen Mesaj :" + message);
//    channel.BasicAck(e.DeliveryTag, false);
//};

#endregion


#region ComplexTypeMesajGönderme

//var factory = new ConnectionFactory { Uri = new Uri("amqps://rjdlwutw:nGKtKeOVDhZPWl6A56oPW42F8nUnu8Hg@beaver.rmq.cloudamqp.com/rjdlwutw") };

//using var connection = factory.CreateConnection();
//var channel = connection.CreateModel();

//channel.BasicQos(0, 1, false);

//channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

//var consumer = new EventingBasicConsumer(channel);
//var queueName = channel.QueueDeclare().QueueName;

//Dictionary<string, object> headers = new Dictionary<string, object>();
//headers.Add("format", "pdf");
//headers.Add("shape", "a4");
//headers.Add("x-match", "any");


//channel.QueueBind(queueName, "header-exchange", "", headers);
//channel.BasicConsume(queueName, false, consumer);

//Console.WriteLine("Loglar dinleniyor..");

////Consumerlar aynı dataları alır.
//consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//{
//    var message = Encoding.UTF8.GetString(e.Body.ToArray());
//    Product  product = JsonSerializer.Deserialize<Product>(message);

//    Thread.Sleep(1000);
//    Console.WriteLine($"Gelen Mesaj : Id : {product.Id}, Name: {product.Name}, Price: {product.Price}, Stock: {product.Stock}");
//    channel.BasicAck(e.DeliveryTag, false);
//};

#endregion

Console.ReadLine();


