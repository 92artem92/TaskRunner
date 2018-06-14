using System;

namespace TaskRunner
{
    public class DefaultTaskFactory:ITaskFactory
    {
        public ITask Create(Action action)
        {
            return new ActionTask(action);
        }

        public IAsyncTaskResult<T> Create<T>(Func<T> func)
        {
            return new AsyncTaskResult<T>(func);
        }
    }
    
}