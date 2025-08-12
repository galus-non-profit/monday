namespace Monday.WebApi.Exceptions;

public sealed class InvalidPassword : DomainException
{
    public InvalidPassword() : base("Invalid password")
    {
    }
}
