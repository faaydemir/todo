namespace ToDo.Core.Services
{
    /// <summary>
    /// EncryptionService for encrypt password before add to db
    /// !!! Not implemented
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        #region methods

        public string Dencrypt(string text)
        {
            return text;
        }

        public string Encrypt(string text)
        {
            return text;
        }

        #endregion methods
    }
}