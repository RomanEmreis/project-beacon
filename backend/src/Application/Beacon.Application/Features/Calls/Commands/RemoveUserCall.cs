using Beacon.Application.Providers.Calls;
using Beacon.Domain.Calls;
using MediatR;

namespace Beacon.Application.Features.Calls.Commands
{
    public record RemoveUserCall(UserCall UserCall) : IRequest;

    public sealed class RemoveUserCallHandler(ICallsProvider callsProvider) : IRequestHandler<RemoveUserCall>
    {
        public Task Handle(RemoveUserCall request, CancellationToken cancellationToken)
        {
            if (request.UserCall is not null)
            {
                callsProvider.UserCalls.Remove(request.UserCall);
            }

            return Task.CompletedTask;
        }
    }
}
