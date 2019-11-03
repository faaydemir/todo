using System.Threading.Tasks;
using ToDo.Core.Model;

namespace ToDo.Core.Services
{
    public class AuthenticationResult
    {
        public string Message { get; set; }
        public bool IsAuthSucceed { get; set; }
        public User User { get; set; }
    }

    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string userName, string password);

        Task<AuthenticationResult> SingupAsync(string userName, string mail, string password);

        Task<bool> LogoutAsync();
    }
}