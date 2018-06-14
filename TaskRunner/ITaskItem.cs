using System;
using System.Threading.Tasks;

namespace TaskRunner
{
    public interface ITaskWorkItem
    {
        string Name { get; set; }
        void Run();
        event Action<ITaskWorkItem> TaskRunning;
        event Action<ITaskWorkItem> TaskCompleted;
        event Action<ITaskWorkItem, Exception> TaskFaulted;
    }
    public interface ITaskWorkItem<T> : ITaskWorkItem
    {
        Task<T> GetResultAsync();
    }
}