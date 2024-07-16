namespace Monday.WebApi.Interfaces;

using Monday.WebApi.Domain;

public interface IUserRepository
{
    Task CreateAsync(UserEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserId entity, CancellationToken cancellationToken = default);
}
