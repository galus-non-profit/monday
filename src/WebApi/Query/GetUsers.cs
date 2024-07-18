namespace Monday.WebApi.Query;

using MediatR;
using Monday.WebApi.Results;

public class GetUsers : IRequest<IReadOnlyList<UserResult>>;