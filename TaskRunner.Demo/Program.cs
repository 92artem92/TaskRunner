using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace TaskRunner.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateClients();
            Console.ReadLine();
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


        private static int _taskCounter;


        private static async void Client()
        {
            var task = CreateTask();

            var runner = TaskRunner.GetTaskRunner();

            var second = new Random().Next(1, 10);
            Thread.Sleep(TimeSpan.FromSeconds(second));

            Console.WriteLine($"{task.Name} pushed for processing");
            runner.AddTask(task);


            if (task is ITaskWorkItem<string> taskWithResult)
            {
               var result= await taskWithResult.GetResultAsync();
                Console.WriteLine($"{taskWithResult.Name} completed with result = {result}");
            }
        }


        private static ITaskWorkItem CreateTask()
        {
            var rnd = new Random();
            var typeIndex = rnd.Next(0, 10) % 2;
            ITaskWorkItem task;

            if (typeIndex == 0)
                task = new TaskWorkItem(DoSomething);
            else
                task = new TaskWorkItem<string>(DoSomethingWithResult);

            var index = Interlocked.Increment(ref _taskCounter);
            task.Name = "Task " + index;
            task.TaskCompleted += TaskCompleted;
            return task;
        }


        private static void TaskCompleted(ITaskWorkItem task)
        {
            Console.WriteLine($"{task.Name} completed ");
        }

        private static void DoSomething()
        {
            var second = new Random().Next(1, 500);
            Thread.Sleep(TimeSpan.FromMilliseconds(second));
        }
        private static string DoSomethingWithResult()
        {
            DoSomething();
            return Guid.NewGuid().ToString();
        } 

    }
}
