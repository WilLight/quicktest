namespace server.DBSystem.UserDataContainers
{
    /// <summary>
    /// Class, that contains user's data.
    /// </summary>
    public sealed class UserData
    {

        public uint Id {get; set;}

        public UserCredentials Credentials {get; set;}

        public UserRole UserRole {get; set;}

        public UserData(uint id, UserCredentials credentials, UserRole userRole)
        {
            Id = id;
            Credentials = credentials;
            UserRole = userRole;
        }
    }
}