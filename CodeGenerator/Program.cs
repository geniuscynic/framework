using System;

namespace CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectStr = "Server=localhost;Database=blog;Trusted_Connection=True;MultipleActiveResultSets=true";
            var provider = "MsSql";

            var tool = new CodeGenerationTools(connectStr, provider);

            Console.WriteLine("Hello World!");
        }
    }
}
