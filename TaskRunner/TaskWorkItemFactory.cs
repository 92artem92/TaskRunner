using System;

namespace TaskRunner
{
    public class TaskWorkItemFactory
    {
        public ITaskWorkItem GetInstance(Action action)
        {
            return new TaskWorkItem(action);
        }

        public ITaskWorkItem<T> GetInstance<T>(Func<T> func)
        {
            return new TaskWorkItem<T>(func);
        }
    }
}