namespace server.DBSystem.UserDataContainers
{
    /// <summary>
    /// Class, that contains user's data.
    /// </summary>
    public sealed class UserData
    {
        private readonly uint _id;
        private readonly UserCredentials _credentials;
        private readonly UserRole _userRole;

        public uint Id => _id;

        public UserCredentials Credentials => _credentials;

        public UserRole UserRole => _userRole;

        public UserData(uint id, UserCredentials credentials, UserRole userRole)
        {
            _id = id;
            _credentials = credentials;
            _userRole = userRole;
        }
    }
}