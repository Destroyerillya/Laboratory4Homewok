using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Blog;
using System.Linq;

namespace Homework4
{
    class Program
    {
        static ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        static bool flager = true;
        static Context context = new Context();
        static async Task Main(string[] args)
        {
           
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();
            Console.WriteLine("1 - First Select, 2 - Second Select");
            Console.WriteLine();
            Task runqueuetask = RunQueueAsync();
            Task runlistentask = RunListenAsync();
            await runqueuetask;
            await runlistentask;
            context.Dispose();
        }
        
        static async Task RunQueueAsync()
        {
            await Task.Run(() => DoWork());
        }

        static async Task RunListenAsync()
        {
            await Task.Run(() => ListenForInput());
        }

        static void ListenForInput()
        {
            while (flager)
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
                    case "3":
                        flager = false;
                        break;
                    default:
                        Console.WriteLine("Understand command");
                        break;
                }
            }
        }

        static void DoWork()
        {
            while (flager)
            {
                Thread.Sleep(5000);
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
                        Console.WriteLine(result);
                        IQueryable<PersonalMessages> users_messages = from usersMessage in context.UsersMessages
                            select usersMessage;
                        List <PersonalMessages> list_1 = users_messages.ToList();
                        foreach (PersonalMessages usersMessage in users_messages)
                        {
                            Console.WriteLine(usersMessage.Id);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        Console.WriteLine(result);
                        IQueryable<Post> posts = from post in context.Posts
                            select post;
                        List <Post> list_2 = posts.ToList();
                        foreach (Post post in posts)
                        {
                            Console.WriteLine(post.Id);
                        }
                    }
                }
            }
        }
    }
}