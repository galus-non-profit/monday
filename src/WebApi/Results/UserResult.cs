namespace Monday.WebApi.Results;

public class UserResult
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}


public class UserDBEntity
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string UserName { get; init; }
    public required string PasswordHash { get; init; }
}