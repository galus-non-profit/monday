namespace Monday.WebApi.Results;

public class UserResult
{
    public required string Email { get; init; }
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}
