using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRunner.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateClients();
            Console.WriteLine("Hello World!");
        }

        private static void CreateClients()
        {
            var clients = new Thread[100];
            for (var i = 0; i < 100; i++)
            {
                clients[i] = new Thread(Client);
                clients[i].Start();
            }
        }
        private static void Client()
        {
            var runner = TaskRunner.GetTaskRunner();
            var second = new Random().Next(1, 10);
            Thread.Sleep(TimeSpan.FromSeconds(second));
            var task = new TaskItem(DoSomething);
            runner.AddTask(task);
        }

        private static void DoSomething()
        {
            var second = new Random().Next(1, 10);
            Thread.Sleep(TimeSpan.FromSeconds(second));
            Console.WriteLine("Task completed");
        }
    }
}
