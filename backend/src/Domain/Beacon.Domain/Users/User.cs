namespace Beacon.Domain.Users
{
    public class User(string name, string connectionId)
    {
        public string Name => name;
        public string ConnectionId => connectionId;
        public bool InCall { get; private set; }

        public void Call() => InCall = true;
        public void Drop() => InCall = false;
        public void Update(bool callState) => InCall = callState;
    }
}
