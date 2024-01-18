using System;
using System.Collections.Generic;
using System.Text;
using HandleMessage.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;

public class Producer
{
    private static readonly string ConnectionString = "amqps://pzafqbmr:MLtQFJsQm4Qa-Sojmwl0yYOT45IkQyLa@armadillo.rmq.cloudamqp.com/pzafqbmr";
    private static readonly string NameQueue = "sendMessageBack";
    private static readonly string QueueExchange = "sendMessageBack_exchange";
    private static readonly string NameQueueDLX = "sendMessageBack_DLX";
    private static readonly string QueueExchangeDLX = "sendMessageBack_exchange_DLX";
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    
    public Producer()
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
        Console.WriteLine("Connecting to rabbitMQ Producer");
    }
    public async Task SendMessage(Message message)
    {
        try
        {
            // 1. Create exchange
            _channel.ExchangeDeclare(QueueExchange, ExchangeType.Direct, durable: true);

            // 2. Create queue
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("x-dead-letter-exchange", "sendMessageBack_exchange_DLX");
            args.Add("x-dead-letter-routing-key", "sendMessageBack_DLX");
            var queueResult = _channel.QueueDeclare(NameQueue, exclusive: false, arguments: args, durable:true, autoDelete:false);

            // 3. Bind queue
            _channel.QueueBind(queueResult.QueueName, QueueExchange, routingKey: "");
            
            string bodyData = JsonConvert.SerializeObject(message);
            
            Console.WriteLine(bodyData);
            
            var body = Encoding.UTF8.GetBytes(bodyData);
    
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Expiration = "10000";
            
            _channel.BasicPublish(QueueExchange, routingKey: "", basicProperties: properties, body: body);
            
            Console.WriteLine("Send");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
