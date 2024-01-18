using System.Text;
using HandleMessage.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HandleMessage.Services;

public class Consume_Publish
{
    public Consume_Publish()
    {
    }

    public void Consume()
    {
        var factory = new ConnectionFactory {
            HostName = "103.232.55.72",
            UserName = "admin",
            Password = "admin123",
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(exchange: "sendMessage", type: ExchangeType.Fanout);

        // Declare a new queue and bind it to the exchange
        var queueName = "sendMessage";
        //channel.QueueBind(queue: queueName,
        //                  exchange: "sendMessage",
        //                  routingKey: "");

        // Start consuming messages from the queue
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var json = JsonConvert.DeserializeObject<Message>(message);
            HandleCommand(json, channel);

        };
        channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
    }
    
    void HandleCommand(Message message, IModel channel)
    {
        var newMessage = new Message()
        {
            email = message.email,
            text = "Response from C#: " + message.text
        };
        string nameExchange = "fanout.ex";
        channel.ExchangeDeclare(exchange: nameExchange, type: ExchangeType.Fanout, durable: true);
        string bodyData = JsonConvert.SerializeObject(newMessage);
        var body = Encoding.UTF8.GetBytes(bodyData);
        channel.BasicPublish(exchange: nameExchange,
            routingKey: string.Empty,
            basicProperties: null,
            body: body);
        Console.WriteLine($" [x] Sent");

        Console.WriteLine(" Press [enter] to exit.");

        Console.WriteLine("Message Sent");
    }
}