namespace Monday.WebApi.Exceptions;

public sealed class ValidationEmailException : DomainException
{
    public ValidationEmailException() : base("Email is not valid")
    {
    }
}
