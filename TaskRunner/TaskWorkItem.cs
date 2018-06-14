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

    public class TaskWorkItem<T> : TaskWorkItemBase, ITaskWorkItem<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();


        private Func<T> Func { get; }

        public Task<T> GetResultAsync()
        {
            return _tcs.Task;
        }

        protected override void RunInternal()
        {
            var result = Func();
            _tcs.SetResult(result);
        }

        public TaskWorkItem(Func<T> func)
        {
            Func = func;
            TaskFaulted += TaskWorkItem_TaskFaulted;
        }

        private void TaskWorkItem_TaskFaulted(ITaskWorkItem arg1, Exception ex)
        {
            _tcs.SetException(ex);
        }
    }
}