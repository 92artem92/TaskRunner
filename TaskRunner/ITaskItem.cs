using System.Threading.Tasks;

namespace TaskRunner
{
    public interface ITaskItem
    {
        void Run();
    }
    public interface ITaskItem<T> : ITaskItem
    {
        Task<T> GetResultAsync();
    }
}