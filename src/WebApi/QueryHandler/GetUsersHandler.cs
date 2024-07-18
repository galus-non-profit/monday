namespace Monday.WebApi.QueryHandler;

using MediatR;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Query;
using Monday.WebApi.Results;

public class GetUsersHandler : IRequestHandler<GetUsers, IReadOnlyList<UserResult>>
{
    private readonly ILogger<GetUsersHandler> logger;
    private readonly IUserRepository userRepository;

    public GetUsersHandler(ILogger<GetUsersHandler> logger, IUserRepository userRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserResult>> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("try to get users from db");

        var users = await this.userRepository.GetAllAsync(cancellationToken);

        return users;
    }
}