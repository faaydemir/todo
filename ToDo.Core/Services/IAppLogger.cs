using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ToDo.Core.Services
{
    public interface IAppLogger : ILogger
    {
        Task<bool> LogInfo(string message);

        Task<bool> LogWarning(string message);

        Task<bool> LogBindingStarted(string message);

        Task<bool> LogError(string message);
    }
}