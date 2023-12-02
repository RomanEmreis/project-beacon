using Beacon.Application.Providers.Users;
using Beacon.Domain.Users;
using MediatR;

namespace Beacon.Application.Features.Users.Queries
{
    public record GetActiveUsers : IRequest<IEnumerable<User>>;

    public sealed class GetActiveUsersHandler(IUsersProvider usersProvider) : IRequestHandler<GetActiveUsers, IEnumerable<User>>
    {
        public Task<IEnumerable<User>> Handle(GetActiveUsers request, CancellationToken cancellationToken) => Task.FromResult(usersProvider.Users.AsEnumerable());
    }
}
