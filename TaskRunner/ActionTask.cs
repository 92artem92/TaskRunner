using System;

namespace TaskRunner
{
    public class ActionTask :TaskBase
    {
        private readonly Action _action;

        public ActionTask(Action action)
        {
           _action = action;
        }

        protected override void RunInternal()
        {
            _action?.Invoke();
        }
    }
}