using Beacon.Application.Providers.Calls;
using Beacon.Domain.Calls;
using MediatR;

namespace Beacon.Application.Features.Calls.Commands
{
    public record RemoveCallOffers(string ConnectionId) : IRequest;

    public sealed class RemoveCallOffersHandler(ICallsProvider callsProvider) : IRequestHandler<RemoveCallOffers>
    {
        public Task Handle(RemoveCallOffers request, CancellationToken cancellationToken)
        {
            var connectionId = request.ConnectionId;
            var callOffers = callsProvider.CallOffers as List<CallOffer>;
            callOffers?.RemoveAll(c => c.Caller.ConnectionId == connectionId);

            return Task.CompletedTask;
        }
    }
}
