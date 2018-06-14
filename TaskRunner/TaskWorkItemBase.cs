using System;

namespace TaskRunner
{
    public abstract class TaskWorkItemBase : ITaskWorkItem
    {

        public event Action<ITaskWorkItem> TaskRunning;
        public event Action<ITaskWorkItem> TaskCompleted;
        public event Action<ITaskWorkItem,Exception> TaskFaulted;

        public string Name { get; set; }

        public void Run()
        {
            TaskRunning?.Invoke(this);
            try
            {
                RunInternal();
            }
            catch (Exception ex)
            {
                TaskFaulted?.Invoke(this,ex);
                return;
            }
            TaskCompleted?.Invoke(this);
        }

        protected abstract void RunInternal();

    }
}