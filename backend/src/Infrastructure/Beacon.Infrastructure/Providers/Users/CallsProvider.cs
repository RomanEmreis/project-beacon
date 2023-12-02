using Beacon.Application.Providers.Calls;
using Beacon.Domain.Calls;

namespace Beacon.Infrastructure.Providers.Users
{
    internal sealed class CallsProvider : ICallsProvider
    {
        private readonly List<UserCall> userCalls = [];
        private readonly List<CallOffer> callOffers = [];

        public ICollection<UserCall> UserCalls => userCalls;
        public ICollection<CallOffer> CallOffers => callOffers;
    }
}
