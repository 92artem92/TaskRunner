using System;
using System.Threading.Tasks;

namespace TaskRunner
{
    public class TaskWorkItem :TaskWorkItemBase
    {
        public TaskWorkItem(Action action)
        {
            Action = action;
        }

        private Action Action { get; }


        protected override void RunInternal()
        {
            Action?.Invoke();
        }
    }

    public abstract class TaskWorkItemBase : ITaskWorkItem
    {

        public event Action<ITaskWorkItem> TaskRunning;
        public event Action<ITaskWorkItem> TaskCompleted;
        public event Action<ITaskWorkItem,Exception> TaskFaulted;

        public string Name { get; set; }

        public void Run()
        {
            TaskRunning?.Invoke(this);
            try
            {
                RunInternal();
            }
            catch (Exception ex)
            {
                TaskFaulted?.Invoke(this,ex);
                return;
            }
            TaskCompleted?.Invoke(this);
        }

        protected abstract void RunInternal();

    }


    public class TaskWorkItem<T>:TaskWorkItemBase,ITaskWorkItem<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();


        private Func<T> Func { get; }

        public Task<T> GetResultAsync()
        {
            return _tcs.Task;
        }

        protected override void RunInternal()
        {
            try
            {
                var result = Func();
                _tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                _tcs.SetException(ex);
            }
        }

        public TaskWorkItem(Func<T> func)
        {
            Func = func;
        }
    }
}