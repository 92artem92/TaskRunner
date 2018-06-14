using System;

namespace TaskRunner
{
    public class TaskWorkItemFactory
    {
        public ITask GetInstance(Action action)
        {
            return new ActionTask(action);
        }

        public IFuncTask<T> GetInstance<T>(Func<T> func)
        {
            return new FuncTask<T>(func);
        }
    }
}