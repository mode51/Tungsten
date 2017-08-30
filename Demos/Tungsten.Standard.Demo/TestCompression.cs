using System;
using W;

namespace W.Demo
{
    public class TestCompression
    {
        public static void Run()
        {
            Console.Clear();
            while (true)
            {
                Console.Write("Compress <Return to Exit>:");
                var msg = Console.ReadLine();
                if (string.IsNullOrEmpty(msg))
                    break;

                var bytes = msg.AsBytes().AsCompressed();
                var compressed = bytes.AsString();
                Console.WriteLine("Compressed: " + compressed);
                var decompressed = bytes.FromCompressed().AsString();
                Console.WriteLine("Decompressed: " + decompressed);

                var bytes2 = msg.AsBase64().AsBytes().AsCompressed();
                Console.WriteLine("Base64 Compressed: " + bytes2.AsString());
                decompressed = bytes2.FromCompressed().FromBase64();
                Console.WriteLine("Base64 Decompressed: " + decompressed);
            }
            Console.Clear();
        }
    }
}