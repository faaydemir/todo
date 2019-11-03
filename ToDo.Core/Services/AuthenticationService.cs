using System.Linq;
using System.Threading.Tasks;
using ToDo.Core.Model;
using ToDo.DataAccess.DataModels.Entities;
using ToDo.DataAccess.Repositories;

namespace ToDo.Core.Services
{
    /// <summary>
    /// Service for Auth operations
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        #region props

        public IUserRepository UserRepository { get; }
        public IEncryptionService EncryptionService { get; }
        public IAppLogger AppLogger { get; }

        #endregion props

        #region ctor

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="encryptionService"></param>
        /// <param name="appLogger"></param>
        public AuthenticationService(IUserRepository userRepository,
                                        IEncryptionService encryptionService,
                                        IAppLogger appLogger)
        {
            UserRepository = userRepository;
            EncryptionService = encryptionService;
            AppLogger = appLogger;
        }

        #endregion ctor

        #region methods

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>result of operation</returns>
        public async Task<AuthenticationResult> LoginAsync(string userName, string password)
        {
            //encrypt password
            var encryptedPassword = EncryptionService.Encrypt(password);

            //check user exist
            var user = UserRepository.GetAll().FirstOrDefault(x => x.Name == userName && x.Password == encryptedPassword);

            AuthenticationResult result;
            if (user != null)
            {
                result = new AuthenticationResult()
                {
                    IsAuthSucceed = true,
                    User = User.From(user),
                    Message = "Login succeed.",
                };

                await AppLogger.LogInfo($"Login success for :({userName}) ");
            }
            else
            {
                result = new AuthenticationResult()
                {
                    IsAuthSucceed = false,
                    User = null,
                    Message = "Login fail.",
                };

                await AppLogger.LogInfo($"Login fail for :({userName}) ");
            }

            return result;
        }

        public Task<bool> LogoutAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// SingupAsync
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns>result op  singup operation</returns>
        public async Task<AuthenticationResult> SingupAsync(string userName, string mail, string password)
        {
            //check if any user exist with user name or mail
            var user = UserRepository.GetAll().FirstOrDefault(x => x.Name == userName || x.EmailAddress == mail);

            AuthenticationResult result;
            // if user name or mail is taken
            if (user != null)
            {
                result = new AuthenticationResult()
                {
                    IsAuthSucceed = false,
                    User = null,
                    Message = "Someone already has this email or user name  address. Please try another.",
                };
                await AppLogger.LogInfo($"Singup fail user name: \"{userName}\" email : \"{mail}\"");
            }
            else
            {
                //encrypt password
                var encryptedPassword = EncryptionService.Encrypt(password);

                // create new user and login
                var entity = await UserRepository.AddAsync(new UserEntity()
                {
                    EmailAddress = mail,
                    Password = encryptedPassword,
                    Name = userName,
                });
                await UserRepository.SaveChangesAsync();
                await AppLogger.LogInfo($"Singup success user name: \"{userName}\" email : \"{mail}\"");

                result = await LoginAsync(entity.Name, entity.Password);
            }

            return result;
        }

        #endregion methods
    }
}