using System;

namespace server.DBSystem.UserDataContainers
{
    /// <summary>
    /// Class, that contains user's log-in data (login & pass).
    /// </summary>
    public sealed class UserCredentials
    {
        private readonly string _login;
        private readonly string _password;

        public string Login => _login;

        public string Password => _password;

        public UserCredentials(string login, string password)
        {
            _login = login;
            _password = password;
        }

        #region Operators

        public static bool operator ==(UserCredentials item1, UserCredentials item2)
        {
            return item1?.Login == item2?.Login && item1?.Password == item2?.Password;
        }

        public static bool operator !=(UserCredentials item1, UserCredentials item2)
        {
            return item1?.Login != item2?.Login || item1?.Password != item2?.Password;
        }

        private bool Equals(UserCredentials other)
        {
            return _login == other._login && _password == other._password;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is UserCredentials other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_login, _password);
        }

        #endregion
    }
}