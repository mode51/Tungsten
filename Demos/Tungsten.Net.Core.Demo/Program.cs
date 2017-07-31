using System;
using System.Linq;
using System.Threading.Tasks;
using W;

namespace W.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test GenericClient/GenericServer");
                Console.WriteLine("2.  Test SecureStringClient");
                Console.WriteLine("3.  Test SecureStringClient with compression");
                
                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        TestGenericClientServer.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        TestSecureStringClient.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        TestCompressedSecureStringClient.Run();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
                Console.Clear();
            }
        }
    }
}
