using Beacon.Application.Providers.Calls;
using Beacon.Domain.Users;
using MediatR;

namespace Beacon.Application.Features.Calls.Commands
{
    public record AddUserCall(User CallingUser, User TargetUser) : IRequest;

    public sealed class AddUserCallHandler(ICallsProvider callsProvider) : IRequestHandler<AddUserCall>
    {
        public Task Handle(AddUserCall request, CancellationToken cancellationToken)
        {
            List<User> users = new([request.CallingUser, request.TargetUser]);
            callsProvider.UserCalls.Add(new(users));

            return Task.CompletedTask;
        }
    }
}
