using Beacon.Application.Providers.Calls;
using Beacon.Domain.Calls;
using MediatR;

namespace Beacon.Application.Features.Calls.Queries
{
    public record GetUserCall(string ConnectionId) : IRequest<UserCall?>;

    public sealed class GetUserCallHandler(ICallsProvider callsProvider) : IRequestHandler<GetUserCall, UserCall?>
    {
        public Task<UserCall?> Handle(GetUserCall request, CancellationToken cancellationToken)
        {
            var connectionId = request.ConnectionId;
            var matchingCall = callsProvider.UserCalls.SingleOrDefault(userCall =>
                userCall.Users.SingleOrDefault(user =>
                    user.ConnectionId == connectionId) is not null);

            return Task.FromResult(matchingCall);
        }
    }
}
