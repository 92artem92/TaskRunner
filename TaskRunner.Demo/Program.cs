using System;
using System.Threading;

namespace TaskRunner.Demo
{
    class Program
    {
        private static int _taskCounter;

        private static void Main(string[] args)
        {
            CreateClients();
            Console.ReadLine();
        }

        private static void CreateClients()
        {
            var clients = new Thread[100];
            for (var i = 0; i < 100; i++)
            {
                clients[i] = new Thread(Client){IsBackground = true};
                clients[i].Start();
            }
        }

        private static async void Client()
        {
            var task = CreateTask();
            var runner = TaskRunner.Instance;
            var second = new Random().Next(1, 10);
            Thread.Sleep(TimeSpan.FromSeconds(second));
            Console.WriteLine($"{task.Name} pushed for processing");
            runner.AddTask(task);
            if (task is IAsyncTaskResult<string> taskWithResult)
            {
               var result= await taskWithResult.GetResultAsync();
                Console.WriteLine($"{taskWithResult.Name} finished with result = {result}");
            }
        }

        private static ITask CreateTask()
        {
            var rnd = new Random();
            var typeIndex = rnd.Next(0, 10) % 2;
            ITask task;
            if (typeIndex == 0)
                task = new ActionTask(DoSomething);
            else
                task = new AsyncTaskResult<string>(DoSomethingWithResult);
            var index = Interlocked.Increment(ref _taskCounter);
            task.Name = "Task " + index;
            task.Success += TaskSuccess;
            return task;
        }

        private static void TaskSuccess(ITask task)
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
