using System;
using System.Linq;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
#if DEBUG
    internal class Example
    {
        private class Message
        {
            public DateTime Timestamp { get; set; }
            public string Information { get; set; }
        }
        public void CreateHost()
        {
            var host = new PipeHost();
            host.BytesReceived += (s, bytes) =>
            {
                //do something with the data
                Console.WriteLine($"Received {bytes.AsString()}");
                //then respond
                s.PostAsync(bytes, true).Wait();
            };
            host.Start("PipeName", 20, true);
            host.Dispose();
        }
        public async Task CreateClient()
        {
            var client = PipeClient.Create(".", "PipeName", 5000)?.Result;
            if (client != null)
            {
                //make a request
                var response = await client.RequestAsync("Retrieve user data".AsBytes(), true, 5000);
                //do something with the response
                Console.WriteLine($"{response.AsString()}");
            }
        }

        public void CreateTypedHost()
        {
            var host = new PipeHost<Message>();
            host.MessageReceived += (s, message) =>
            {
                //do something with the message
                Console.WriteLine($"Received {message.ToString()}");
                var response = new Message() { Timestamp = DateTime.Now, Information = "Response" };
                //then respond
                s.PostAsync(response, true).Wait();
            };
            host.Start("PipeName", 20, true);
            host.Dispose();
        }
        public void CreateClientTyped()
        {
            //NOTE:  The server must be of type EasyPipeHost<Message>
            var client = PipeClient.Create(".", "PipeName", 5000)?.Result;
            if (client != null)
            {
                //make a request
                var response = client.RequestAsync<Message>(new Message() { Timestamp = DateTime.Now, Information = "Blah blah blah" }, true, 5000).Result;
                //do something with the response
                Console.WriteLine($"{response.Timestamp} - {response.Information}");
            }
        }
    }
#endif
}
