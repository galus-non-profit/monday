namespace Monday.WebApi.Auth
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
    }
}
