using Beacon.Domain.Calls;

namespace Beacon.Application.Providers.Calls
{
    public interface ICallsProvider
    {
        ICollection<UserCall> UserCalls { get; }
        ICollection<CallOffer> CallOffers { get; }
    }
}
