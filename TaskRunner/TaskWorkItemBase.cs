using System;

namespace TaskRunner
{
    public abstract class TaskBase : ITask
    {

        public event Action<ITask> TaskRunning;
        public event Action<ITask> TaskSuccess;
        public event Action<ITask,Exception> TaskFaulted;

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
            TaskSuccess?.Invoke(this);
        }

        private void OnTaskRunning()
        {
            
        }


        protected abstract void RunInternal();

    }
}