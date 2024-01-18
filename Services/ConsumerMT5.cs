using System.Text;
using HandleMessage.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HandleMessage.Services;

public class ConsumerMT5
{
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    private Producer _producer;
    
    public ConsumerMT5()
    {
           // Uri = new Uri("amqps://pzafqbmr:MLtQFJsQm4Qa-Sojmwl0yYOT45IkQyLa@armadillo.rmq.cloudamqp.com/pzafqbmr")
               _factory = new ConnectionFactory
               {
                   // Uri = new Uri("amqp://admin:admin123@103.74.100.190:5672")
                   HostName = "103.74.100.190",
                   UserName = "admin",
                   Password = "admin123"
               };
               _connection = _factory.CreateConnection();
               _channel = _connection.CreateModel();
               _producer = new Producer();
               Console.WriteLine("Connected to RabbitMQ");
    }

    public async Task Consume()
    {
        var queueName = "sendMessageMT5"; // Queue name
        
        // await Task.Delay(TimeSpan.FromSeconds(15));
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            // await Task.Delay(TimeSpan.FromSeconds(15));
            // Xử lý dữ liệu trong đây
            Console.WriteLine($"Normal MT5 Service: {message}");
            var json = JsonConvert.DeserializeObject<Message>(message);
            
            await _producer.SendMessage(json);
            
            // Xử lý dữ liệu trong đây
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(queue: queueName,
            consumer: consumer, autoAck: false);

        Console.WriteLine("Start receive normal");
    }

    public void HotFix()
    {
        // var nameQueueDLX = "test_DLX"; // Queue name DLX
        // var queueExchangeDLX = "test_exchange_DLX"; // Exchange name DLX
        // var notiQueueHotFix = "testNotificationQueueHotFix";
        
        var nameQueueDLX = "sendMessage_DLX_MT5"; // Queue name DLX
        var queueExchangeDLX = "sendMessage_exchange_DLX_MT5"; // Exchange name DLX
        var notiQueueHotFix = "sendMessageNotificationQueueHotFixMT5";
    
        _channel.ExchangeDeclare(exchange: queueExchangeDLX, type: "direct", durable: true);
        _channel.QueueDeclare(queue: notiQueueHotFix, durable: true, exclusive: false, autoDelete: false, arguments: null);

        // Bind the notification queue to the DLX exchange
        _channel.QueueBind(queue: notiQueueHotFix, exchange: queueExchangeDLX, routingKey: nameQueueDLX);
    
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            // Xử lý dữ liệu trong đây
            Console.WriteLine($"HotFix MT5 Service: {message}");
            var json = JsonConvert.DeserializeObject<Message>(message);
            await _producer.SendMessage(json);
            // Xử lý dữ liệu trong đây

        };
        _channel.BasicConsume(queue: notiQueueHotFix,
            autoAck: true,
            consumer: consumer);

        Console.WriteLine("Start Receive Hot Fix");
    }
}