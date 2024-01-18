using HandleMessage.Data;
using HandleMessage.Model;
using HandleMessage.Services;
using Newtonsoft.Json;

static async Task Main()
{
    // var _context = new Consumer();
    // _context.Consume();
    // _context.HotFix();
    // var producer = new Producer();
    // producer.SendMessage(new Message()
    // {
    //     email = "test@gmail.com",
    //     text = "OKe"
    // });
    // var _context = new DbContext();
    // var userService = new UserService(_context);
    // var accountService = new AccountService(_context);
    //
    // var users = userService.findAll();
    // var accounts = accountService.findAll();
    //
    // foreach (var user in accounts) 
    // {
    //     Console.WriteLine(user.username);
    // }
    
    // consumer MT5
    var contextMT5 = new ConsumerMT5();
    contextMT5.Consume();
    contextMT5.HotFix();


    // Socket message
    // await Socket_Service.Receive_Message();
    //
    // while (true)
    // {
    //     string message = null;
    //
    //     lock (Socket_Service.queueLock)
    //     {
    //         message = Socket_Service.queue.Dequeue();
    //     }
    //
    //     if (message != null)
    //     {
    //         Console.WriteLine($"Processing message: {message}");
    //         Console.WriteLine($"Size: {Socket_Service.queue.Size()}");
    //         await Socket_Service.Send_Message(JsonConvert.DeserializeObject<Message>(message));
    //         await Task.Delay(TimeSpan.FromSeconds(1));    
    //     }
    //     else
    //     {
    //         await Task.Delay(TimeSpan.FromSeconds(1));
    //     }
    //     
    // }
    
    // Socket message MT%
    await Socket_Service.Receive_Message_MT5();
    
    while (true)
    {
        string message = null;
    
        lock (Socket_Service.queueLock)
        {
            message = Socket_Service.queueMT5.Dequeue();
        }
    
        if (message != null)
        {
            Console.WriteLine($"Processing message: {message}");
            Console.WriteLine($"Size: {Socket_Service.queueMT5.Size()}");
            await Socket_Service.Send_Message(JsonConvert.DeserializeObject<Message>(message));
            await Task.Delay(TimeSpan.FromSeconds(1));    
        }
        else
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
    }

}

Main();

Console.ReadKey();