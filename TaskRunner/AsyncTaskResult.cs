using System;
using System.Threading.Tasks;

namespace TaskRunner
{
    public class AsyncTaskResult<T> : TaskBase, IAsyncTaskResult<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();
        private readonly Func<T> _func;

        public AsyncTaskResult(Func<T> func)
        {
            _func = func;
            TaskFaulted += TaskWorkItem_TaskFaulted;
        }

        public Task<T> GetResultAsync()
        {
            return _tcs.Task;
        }

        protected override void RunInternal()
        {
            var result = _func();
            _tcs.SetResult(result);
        }

        private void TaskWorkItem_TaskFaulted(ITask arg, Exception ex)
        {
            _tcs.SetException(ex);
        }
    }
}