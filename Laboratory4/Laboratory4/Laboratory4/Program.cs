using System;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratory4
{
    class Program
    {
        public static int a = 1;
        public static object lockoption = new Object();
        static void Sum()
        { 
            while (true) 
            {
                Thread.Sleep(2000);
                lock (lockoption)
                {
                    a += 2;
                }
            }
        }
        static async Task SumAsync()
        {
            await Task.Run(() => Sum());
        }
        
        static void Mul()
        { 
            while (true) 
            {
                Thread.Sleep(3000);
                lock (lockoption)
                {
                    a *= 3;
                }
            }
        }
        static async Task MulAsync()
        {
            await Task.Run(() => Mul());
        }
        static void Menu()
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
        static async Task MenuAsync()
        {
            await Task.Run(() => Menu());
        }
        async static Task Main(string[] args)
        {
            Task Task1 = SumAsync();
            Task Task2 = MulAsync();
            Task Task3 = MenuAsync();
            await Task1;
            await Task2;
            await Task3;
        }
    }
}