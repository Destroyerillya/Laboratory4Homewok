using System;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratory4
{
    class Program
    {
        public static int a = 1;
        static int Sum()
        { 
            while (true) 
            {
                Thread.Sleep(2000);
                a += 2;
            }
        }
        static async Task<int> SumAsync()
        {
            return await Task.Run(() => Sum());
        }
        
        static int Mul()
        { 
            while (true) 
            {
                Thread.Sleep(3000);
                a *= 3;
            }
        }
        static async Task<int> MulAsync()
        {
            return await Task.Run(() => Mul());
        }
        static int Menu()
        {   Console.WriteLine("write command show or stop");
            while (true)
            {
                string caseSwitch = Console.ReadLine();
                
                switch (caseSwitch)
                {
                    case "show":
                        Console.WriteLine(a);
                        break;
                    case "stop":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Understand command");
                        break;
                }
            }
        }
        static async Task<int> MenuAsync()
        {
            return await Task.Run(() => Menu());
        }
        async static Task Main(string[] args)
        {
            Task<int> Task1 = SumAsync();
            Task<int> Task2 = MulAsync();
            Task<int> Task3 = MenuAsync();
            int result1 = await Task1;
            int result2 = await Task2;
            int result3 = await Task3;
        }
    }
}