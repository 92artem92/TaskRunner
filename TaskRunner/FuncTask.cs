using System;
using System.Threading.Tasks;

namespace TaskRunner
{
    public class FuncTask<T> : TaskBase, IFuncTask<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();
        private readonly Func<T> _func;

        public FuncTask(Func<T> func)
        {
            _func = func;
            TaskFaulted += TaskWorkItem_TaskFaulted;
        }

        /// <summary>
        /// Gets the result value of this task
        /// </summary>
        /// <returns></returns>
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
            //set exception as task result
            _tcs.SetException(ex);
        }
    }
}