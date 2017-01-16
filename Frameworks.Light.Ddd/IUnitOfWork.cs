using System.Threading.Tasks;

namespace Frameworks.Light.Ddd
{
    public interface IAsyncUnitOfWork
    {
        Task CommitAsync();
    }
}