using Beacon.Domain.Users;

namespace Beacon.Domain.Calls
{
    public class CallOffer(User caller, User callee)
    {
        public User Caller => caller;
        public User Callee => callee;
    }
}
