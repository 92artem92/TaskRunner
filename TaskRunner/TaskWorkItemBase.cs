using System;

namespace TaskRunner
{
    public abstract class TaskBase : ITask
    {

        public event Action<ITask> Running;
        public event Action<ITask> Success;
        public event Action<ITask,Exception> Faulted;

        public string Name { get; set; }

        public void Run()
        {
            Running?.Invoke(this);
            try
            {
                RunInternal();
            }
            catch (Exception ex)
            {
                Faulted?.Invoke(this,ex);
                return;
            }
            Success?.Invoke(this);
        }

        private void OnTaskRunning()
        {
            
        }


        protected abstract void RunInternal();

    }
}