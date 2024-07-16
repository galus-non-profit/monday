namespace Monday.WebApi.Auth
{
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordHasher : IPasswordHasher
    {
        private readonly int saltSize = 256 / 8;
        private readonly int iterations = 100000;
        private readonly int keySize = 512 / 8;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly char delimiter = ';';

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(this.saltSize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                salt, this.iterations, this.hashAlgorithm, this.keySize);

            return string.Join(this.delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }
    }
}
