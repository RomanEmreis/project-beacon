using Beacon.Application.Providers.Users;
using Beacon.Domain.Users;

namespace Beacon.Infrastructure.Providers.Calls
{
    internal sealed class UsersProvider : IUsersProvider
    {
        private readonly List<User> users = [];

        public ICollection<User> Users => users;
    }
}
