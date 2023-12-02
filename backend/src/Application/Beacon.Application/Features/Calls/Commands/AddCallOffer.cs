using Beacon.Application.Providers.Calls;
using Beacon.Domain.Users;
using MediatR;

namespace Beacon.Application.Features.Calls.Commands
{
    public record AddCallOffer(User Caller, User Callee) : IRequest;

    public sealed class AddCallOfferHandler(ICallsProvider callsProvider) : IRequestHandler<AddCallOffer>
    {
        public Task Handle(AddCallOffer request, CancellationToken cancellationToken)
        {
            callsProvider.CallOffers.Add(new(request.Caller, request.Callee));
            return Task.CompletedTask;
        }
    }
}
