namespace Monday.WebApi.Domain;

using System.Diagnostics.CodeAnalysis;
using Monday.WebApi.Exceptions;
using Monday.WebApi.Results;

public sealed record UserEntity
{
    public required Email Email { get; init; }
    public required UserId Id { get; init; }
    public required string Name { get; init; }
    public required Password Password { get; init; }

    [SetsRequiredMembers]
    public UserEntity(string name, Email email, UserId id, Password password)
    {
        this.Name = name;
        this.Email = email;
        this.Id = id;
        this.Password = password;
    }
}

public sealed record Password
{
    public string Value { get; private init; }

    public Password(string value)
    {
        if (value == string.Empty)
        {
            throw new InvalidPassword();
        }

        this.Value = value;
    }
}

public sealed record Email
{
    //private readonly string emailRegex = @"^[^@\s]+@[^@\s]+\.$";

    public string Value { get; private init; }

    public Email(string value)
    {
        if (value == string.Empty)
        {
            throw new ValidationEmailException();
        }

        this.Value = value;
    }
}

public sealed record UserId
{
    public Guid Value { get; private init; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityId();
        }

        this.Value = value;
    }
}

public static class UserEntityExtension
{
    public static UserDBEntity ToDbEntity(this UserEntity userEntity)
    {
        var result = new UserDBEntity
        {
            Id = userEntity.Id.Value,
            Email = userEntity.Email.Value,
            PasswordHash = userEntity.Password.Value,
            UserName = userEntity.Name,
        };

        return result;
    }
}
