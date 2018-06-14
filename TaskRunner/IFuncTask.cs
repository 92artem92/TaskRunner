using System.Threading.Tasks;

namespace TaskRunner
{
    public interface IAsyncTaskResult<T> : ITask
    {
        Task<T> GetResultAsync();
    }
}