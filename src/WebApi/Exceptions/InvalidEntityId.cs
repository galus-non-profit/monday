namespace Monday.WebApi.Exceptions;

public sealed class InvalidEntityId : DomainException
{
    public InvalidEntityId() : base("Invalid user id")
    {
    }
}

public sealed class InvalidPassword : DomainException
{
    public InvalidPassword() : base("Invalid passwordHashed")
    {
    }
}
