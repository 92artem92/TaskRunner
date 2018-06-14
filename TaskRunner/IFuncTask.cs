using System.Threading.Tasks;

namespace TaskRunner
{
    public interface IFuncTask<T> : ITask
    {
        Task<T> GetResultAsync();
    }
}