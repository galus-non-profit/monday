namespace Monday.WebApi.Interfaces;

using Monday.WebApi.Domain;
using Monday.WebApi.Results;

public interface IUserRepository
{
    Task CreateAsync(UserEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserId entity, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserResult>> GetAllAsync(CancellationToken cancellationToken = default);
}
