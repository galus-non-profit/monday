namespace Monday.WebApi.Domain;

using System.Diagnostics.CodeAnalysis;
using Monday.WebApi.Exceptions;

public sealed record UserEntity
{
    public required Email Email { get; init; }
    public required UserId Id { get; init; }
    public required string Name { get; init; }
    public required PasswordHashed PasswordHashed { get; init; }

    [SetsRequiredMembers]
    public UserEntity(string name, Email email, UserId id, PasswordHashed passwordHashed)
    {
        this.Name = name;
        this.Email = email;
        this.Id = id;
        this.PasswordHashed = passwordHashed;
    }
}

public sealed record PasswordHashed
{
    public string Value { get; private init; }

    public PasswordHashed(string value)
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
    public static UserDbEnity ToDbEnity(this UserEntity userEntity)
    {
        var result = new UserDbEnity
        {
            UserId = userEntity.Id.Value,
            Email = userEntity.Email.Value,
            PasswordHash = userEntity.PasswordHashed.Value,
            Name = userEntity.Name,
        };

        return result;
    }
}


public sealed record UserDbEnity
{
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
    public string PasswordHash { get; init; } = string.Empty;
}