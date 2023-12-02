using Beacon.Application.Features.Calls.Queries;
using Beacon.Application.Providers.Users;
using Beacon.Domain.Users;
using MediatR;

namespace Beacon.Application.Features.Users.Commands
{
    public record RefreshUserList() : IRequest<IEnumerable<User>>;

    public sealed class RefreshUserListHandler(IMediator mediator, IUsersProvider usersProvider) : IRequestHandler<RefreshUserList, IEnumerable<User>>
    {
        public async Task<IEnumerable<User>> Handle(RefreshUserList request, CancellationToken cancellationToken)
        {
            foreach (var user in usersProvider.Users)
            {
                var userCall = await mediator.Send(new GetUserCall(user.ConnectionId), cancellationToken);
                user.Update(userCall is not null);
            }

            return usersProvider.Users;
        }
    }
}
