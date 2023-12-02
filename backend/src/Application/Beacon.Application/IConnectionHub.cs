using Beacon.Domain.Users;

namespace Beacon.Application
{
    public interface IConnectionHub
    {
        Task UpdateUserList(IEnumerable<User> userList);
        Task CallAccepted(User acceptingUser);
        Task CallDeclined(User decliningUser, string reason);
        Task IncomingCall(User callingUser);
        Task ReceiveSignal(User signalingUser, string signal);
        Task CallEnded(User signalingUser, string signal);
    }
}
