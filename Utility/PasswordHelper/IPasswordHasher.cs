namespace Utility.PasswordHelper
{
    public interface IPasswordHasher
    {
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
        string Hash(string password);
    }
}