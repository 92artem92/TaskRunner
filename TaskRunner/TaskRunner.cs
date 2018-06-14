using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRunner
{
    public class TaskRunner
    {
        private  static TaskRunner _instance;
        private  static readonly object Lock=new object();
        private readonly ConcurrentQueue<ITaskItem> _taskQueue = new ConcurrentQueue<ITaskItem>();

        private TaskRunner()
        {
            var workerThread = new Thread(Worker);
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
        

        public virtual void AddTask(ITaskItem task)
        {
            _taskQueue.Enqueue(task);
        }

        public event Action<ITaskItem> TaskRunning;
        public event Action<ITaskItem> TaskFinished;

        private void Worker()
        {
            while(_taskQueue.TryDequeue(out var task))
            {
                TaskRunning?.Invoke(task);
                task.Run();
                TaskFinished?.Invoke(task);
            }
            Thread.Sleep(1);
        }
    }


}
