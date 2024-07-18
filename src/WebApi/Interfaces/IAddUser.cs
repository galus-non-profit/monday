namespace Monday.WebApi.Interfaces;

public interface IAddUser
{
    string Email { get; }
    Guid Id { get; }
    string Name { get; }
    string Password { get; }
}
