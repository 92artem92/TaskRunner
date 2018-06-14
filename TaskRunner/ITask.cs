using System;

namespace TaskRunner
{
    public interface ITask
    {
        string Name { get; set; }
        void Run();
        event Action<ITask> Running;
        event Action<ITask> Success;
        event Action<ITask, Exception> Faulted; 
    }
}