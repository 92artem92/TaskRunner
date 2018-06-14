using System;
using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRunner
{
    public sealed class TaskRunner
    {
        private  static TaskRunner _instance;
        private static readonly object Lock = new object();
        private readonly ConcurrentQueue<ITask> _taskQueue = new ConcurrentQueue<ITask>();
        public event Action<Exception> ExceptionHandler;

        private TaskRunner()
        {
            var worker = new Task(Worker);
            worker.Start();
        }

        public static TaskRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                            _instance = new TaskRunner();
                    }
                }
                return _instance;
            }
        }

        public void AddTask(ITask task)
        {
            _taskQueue.Enqueue(task);
        }

        private void Worker()
        {
            while (true)
            {
                while (_taskQueue.TryDequeue(out var task))
                {
                    try
                    {
                        task.Run();
                    } 
                    catch (Exception ex)
                    {
                        ExceptionHandler?.Invoke(ex);
                    }
                }
                Thread.Sleep(1);
            } 
        }
    }
}
