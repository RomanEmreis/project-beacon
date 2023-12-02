using Beacon.Domain.Users;

namespace Beacon.Application.Providers.Users
{
    public interface IUsersProvider
    {
        ICollection<User> Users { get; }
    }
}
