using System;
using System.Threading.Tasks;
using ToDo.Core.Model;

namespace ToDo.Core.Services
{
    public class MockAuthenticationService : IAuthenticationService
    {
        public Task<AuthenticationResult> LoginAsync(string userName, string password)
        {
            var random = new Random(1231);
            return Task.FromResult(new AuthenticationResult
            {
                IsAuthSucceed = true,
                Message = null,
                User = new User
                {
                    Name = $"{userName} {random.Next() % 50}",
                    Mail = "Mail",
                }
            }); ;
        }

        public Task<bool> LogoutAsync()
        {
            return Task.FromResult(true);
        }

        public Task<AuthenticationResult> SingupAsync(string userName, string mail, string password)
        {
            var random = new Random(1231);
            return Task.FromResult(new AuthenticationResult
            {
                IsAuthSucceed = true,
                Message = null,
                User = new User
                {
                    Name = $"{userName} {random.Next() % 50}",
                    Mail = mail
                }
            }); ;
        }
    }
}