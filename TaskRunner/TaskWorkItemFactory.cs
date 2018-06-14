using System;

namespace TaskRunner
{
    public class TaskWorkItemFactory
    {
        public ITask GetInstance(Action action)
        {
            return new ActionTask(action);
        }

        public IAsyncTaskResult<T> GetInstance<T>(Func<T> func)
        {
            return new AsyncTaskResult<T>(func);
        }
    }
}