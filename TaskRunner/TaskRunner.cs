using System;
using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Threading;

namespace TaskRunner
{
    public class TaskRunner
    {
       
        private  static TaskRunner _instance;
        private static readonly object Lock = new object();
        private readonly ConcurrentQueue<ITask> _taskQueue = new ConcurrentQueue<ITask>();
        private readonly TaskWorkItemFactory _factory=new TaskWorkItemFactory();
        public event Action<Exception> ExceptionHandler;

        private TaskRunner()
        {
            var workerThread = new Thread(Worker) {IsBackground = true};
            workerThread.Start();
        }

        public static TaskRunner GetTaskRunner()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if(_instance==null)
                        _instance=new TaskRunner();
                }
            }  
            return _instance;
        }

        public void AddTask(ITask task)
        {
            _taskQueue.Enqueue(task);
        }

        public void AddTask(Action action)
        {
            AddTask(_factory.GetInstance(action));
        }

        public void AddTask<T>(Func<T> func)
        {
           AddTask(_factory.GetInstance(func));
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
                    //prevent tasks execution stopping 
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
