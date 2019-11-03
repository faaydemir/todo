namespace ToDo.Core.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string text);

        string Dencrypt(string text);
    }
}