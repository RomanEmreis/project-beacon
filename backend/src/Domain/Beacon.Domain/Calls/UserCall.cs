using Beacon.Domain.Users;

namespace Beacon.Domain.Calls
{
    public class UserCall(List<User> users)
    {
        public List<User> Users => users;
    }
}
