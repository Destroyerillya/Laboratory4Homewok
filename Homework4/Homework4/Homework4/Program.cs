using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Homework4
{
    class Program
    {
        readonly static CancellationTokenSource _cancelTokenSrc = new CancellationTokenSource();
        static ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        static SqlConnection cnn;
        static void Main(string[] args)
        {
            string connetionString;
            connetionString = @"Server=.\SQLEXPRESS;Database=blog;Trusted_Connection=True;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            Console.CancelKeyPress += Console_CancelKeyPress;
            CancellationToken cancelToken = _cancelTokenSrc.Token;
            Console.WriteLine("Type commands followed by 'ENTER'");
            Console.WriteLine("Press CTL+C to Terminate");
            Console.WriteLine();
            try
            {
                Task.Run(() => DoWork(), cancelToken);
                Task.Run(() => ListenForInput(), cancelToken);
                cancelToken.WaitHandle.WaitOne();
                cancelToken.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation Canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ListenForInput()
        {
            while (true)
            {
                string caseSwitch = Console.ReadLine();
                switch (caseSwitch)
                {
                    case "1":
                        queue.Enqueue(1);
                        break;
                    case "2":
                        queue.Enqueue(2);
                        break;
                    default:
                        Console.WriteLine("Understand command");
                        //Environment.Exit(0);
                        break;
                }
            }
        }

        static void DoWork()
        {
            while (true)
            {
                Thread.Sleep(1000);
                int result;
                if (!queue.TryDequeue(out result))
                {
                    Console.WriteLine("CQ: TryPeek failed when it should have succeeded");
                }
                else if (result != 0)
                {
                    if (result == 1)
                    {
                        Thread.Sleep(2000);
                        SqlCommand cmd = new SqlCommand("SELECT * FROM UsersMessages", cnn);
                        Console.WriteLine(result);
                        Console.WriteLine(cmd.ExecuteScalar());
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Posts WHERE PublicationTime >= 12/04/2011 12:00:00 AM AND PublicationTime <= 25/05/2011 3:53:04 AM", cnn);
                        Console.WriteLine(cmd.ExecuteScalar());
                    }
                }
            }
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            cnn.Close();
            e.Cancel = true;
            Console.WriteLine("Cancelling...");
            _cancelTokenSrc.Cancel();
        }
    }
}