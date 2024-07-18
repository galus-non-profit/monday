namespace Monday.WebApi.Auth;

using System.Security.Cryptography;
using System.Text;

internal sealed class PasswordHasher : IPasswordHasher
{
    private const char DELIMITER = ';';
    private const int ITERATIONS = 100000;
    private const int KEY_SIZE = 512 / 8;
    private const int SALT_SIZE = 256 / 8;
    private static readonly HashAlgorithmName HASH_ALGORITHM = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        var base64Salt = Convert.ToBase64String(salt);

        var bytes = Encoding.UTF8.GetBytes(password);

        var hash = Rfc2898DeriveBytes.Pbkdf2(bytes, salt, ITERATIONS, HASH_ALGORITHM, KEY_SIZE);
        var base64Hash = Convert.ToBase64String(hash);

        return string.Join(DELIMITER, base64Salt, base64Hash);
    }
}
