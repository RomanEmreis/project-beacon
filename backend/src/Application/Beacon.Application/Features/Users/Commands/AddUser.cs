using Beacon.Application.Providers.Users;
using MediatR;

namespace Beacon.Application.Features.Users.Commands
{
    public record AddUser(string UserName, string ConnectionId) : IRequest;

    internal sealed class AddUserProvider(IUsersProvider usersProvider) : IRequestHandler<AddUser>
    {
        public Task Handle(AddUser request, CancellationToken cancellationToken)
        {
            usersProvider.Users.Add(new(request.UserName, request.ConnectionId));
            return Task.CompletedTask;
        }
    }
}
