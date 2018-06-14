using System;

namespace TaskRunner
{
    public interface ITaskFactory
    {
        ITask Create(Action action);
        IAsyncTaskResult<T> Create<T>(Func<T> func);
    }
}