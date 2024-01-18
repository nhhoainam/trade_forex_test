using HandleMessage.Model;
using Newtonsoft.Json;

namespace HandleMessage.Services;

public class Socket_Service
{
    private static readonly SocketIOClient.SocketIO _client = new SocketIOClient.SocketIO("https://api.dautungoaite.online/");
    public static Queue queue = new Queue();
    public static Queue queueMT5 = new Queue();
    public static readonly object queueLock = new object(); // Lock for thread safety
    public static async Task Receive_Message()
    {
        try
        {
            // var client = ;
            
            _client.On("message-admin-trade", async (response) =>
            {
                
                string text = response.GetValue<string>();
                Console.WriteLine(text);
                // var message = JsonConvert.DeserializeObject<Message>(text);
                // Console.WriteLine($"{message.email}, {message.text}");
                // await Task.Delay(TimeSpan.FromSeconds(5));
                // await Send_Message(new Message()
                // {
                //     email = "test@gmail.com",
                //     text = "Send back"
                // }, _client);
                
                lock (queueLock)
                {
                    queue.Enqueue(text);
                }
            });
            await _client.ConnectAsync();
            Console.WriteLine("Connected");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
    
    public static async Task Receive_Message_MT5()
    {
        try
        {
            // var client = ;
            
            _client.On("message-admin-trade-mt5", async (response) =>
            {
                
                string text = response.GetValue<string>();
                Console.WriteLine(text);
                // var message = JsonConvert.DeserializeObject<Message>(text);
                // Console.WriteLine($"{message.email}, {message.text}");
                // await Task.Delay(TimeSpan.FromSeconds(5));
                // await Send_Message(new Message()
                // {
                //     email = "test@gmail.com",
                //     text = "Send back"
                // }, _client);
                
                lock (queueLock)
                {
                    queueMT5.Enqueue(text);
                }
            });
            await _client.ConnectAsync();
            Console.WriteLine("Connected");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public static async Task Send_Message(Message? message)
    {
        try
        {
            await _client.EmitAsync("data-receive-back", JsonConvert.SerializeObject(message));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}