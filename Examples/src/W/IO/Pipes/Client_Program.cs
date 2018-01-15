using System;
using System.Threading.Tasks;
using W.AsExtensions;
using W.IO.Pipes;

namespace LogClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
//#if NET45
//                var client = PipeClient.Create(".", "ConsoleLogger", 3000).Result;
//#else
                var client = PipeClient.Create(".", "ConsoleLogger", System.Security.Principal.TokenImpersonationLevel.Impersonation, 3000).Result;
//#endif
                if (client == null)
                {
                    Console.WriteLine("Unable to connect to the server");
                    return;
                }
                while (true)
                {
                    Console.Write("Log (<return> to exit):");
                    var msg = Console.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                        break;
                    Task.Run(async () =>
                    {
                        await client.PostAsync(msg.AsBytes(), false);
                    }).Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Press Any Key To Exit");
                Console.ReadKey(true);
            }
        }
    }
}
