using Beacon.Application.Providers.Users;
using Beacon.Domain.Users;
using MediatR;
using System;

namespace Beacon.Application.Features.Users.Queries
{
    public record GetUser(string ConnectionId) : IRequest<User?>;

    public sealed class GetUserHandler(IUsersProvider usersProvider) : IRequestHandler<GetUser, User?>
    {
        public Task<User?> Handle(GetUser request, CancellationToken cancellationToken)
        {
            var connectionId = request.ConnectionId;
            var user = usersProvider.Users.SingleOrDefault(u => u.ConnectionId == connectionId);

            return Task.FromResult(user);
        }
    }
}
