using System;

namespace TaskRunner
{
    public interface ITask
    {
        string Name { get; set; }
        void Run();
        event Action<ITask> TaskRunning;
        event Action<ITask> TaskSuccess;
        event Action<ITask, Exception> TaskFaulted;
    }
}