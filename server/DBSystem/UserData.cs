namespace server.DBSystem
{
    /// <summary>
    /// Class, that contains user's data.
    /// </summary>
    public sealed class UserData
    {
        private readonly uint _id;
        private readonly UserCredentials _credentials;

        public uint Id => _id;

        public UserCredentials Credentials => _credentials;
        
        public UserData(uint id, UserCredentials credentials)
        {
            _id = id;
            _credentials = credentials;
        }
    }
}