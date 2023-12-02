using Beacon.Application.Providers.Users;
using Beacon.Domain.Users;
using MediatR;

namespace Beacon.Application.Features.Users.Commands
{
    public record RemoveUser(string ConnectionId) : IRequest;

    public sealed class RemoveUserHandler(IUsersProvider usersProvider) : IRequestHandler<RemoveUser>
    {
        public Task Handle(RemoveUser request, CancellationToken cancellationToken)
        {
            var connectionId = request.ConnectionId;
            (usersProvider.Users as List<User>)?.RemoveAll(user => user.ConnectionId == connectionId);

            return Task.CompletedTask;
        }
    }
}
