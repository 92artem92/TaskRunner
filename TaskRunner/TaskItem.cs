using System;
using System.Threading.Tasks;

namespace TaskRunner
{
    public class TaskItem : ITaskItem
    {
        public TaskItem(Action action)
        {
            Action = action;
        }

        private Action Action { get; }

        public void Run()
        {
            Action?.Invoke();
        }
    }
    public class TaskItem<T> : ITaskItem<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();

        private Func<T> Func { get; }

        public Task<T> GetResultAsync()
        {
            return _tcs.Task;
        }

        public void Run()
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

        public TaskItem(Func<T> func)
        {
            Func = func;
        }
    }
}