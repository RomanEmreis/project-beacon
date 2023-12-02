using Beacon.Application.Providers.Calls;
using Beacon.Domain.Calls;
using MediatR;

namespace Beacon.Application.Features.Calls.Commands
{
    public record CheckActiveCallOffers(
        string CallingUserConnectionId,
        string TargetUserConnectionId) : IRequest<int>;

    public sealed class CheckActiveCallOffersHandler(ICallsProvider callsProvider) : IRequestHandler<CheckActiveCallOffers, int>
    {
        public Task<int> Handle(CheckActiveCallOffers request, CancellationToken cancellationToken)
        {
            var (callingUserConnectionId, targetUserConnectionId) = request;
            var callOffers = callsProvider.CallOffers as List<CallOffer>;

            var offerCount = callOffers?.RemoveAll(c => 
                c.Callee.ConnectionId == callingUserConnectionId && 
                c.Caller.ConnectionId == targetUserConnectionId) ?? 0;

            return Task.FromResult(offerCount);
        }
    }
}
