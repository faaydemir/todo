using System.Threading.Tasks;

namespace EntityFrameworkCoreHelper
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}