using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace server.DBSystem
{
    /// <summary>
    /// Class, that manages operations with data in a DB (like get/put data).
    /// </summary>
    public sealed class DbManager
    {
        private const string MongoDbAddressEnvVarName = "quicktest-mongo-db-address";
        private const string MongoConnectStringPrefix = "mongodb://";
        private const string QuickTestDbName = "quicktest";
        private const string UsersCollectionName = "users";
        
        private IMongoDatabase _database;

        /// <summary>
        /// Initialize manager's members and connect to the DB.
        /// </summary>
        public void Initialize()
        {
            _database = new MongoClient(GetConnectionString()).GetDatabase(QuickTestDbName);
        }

        private string GetConnectionString()
        {
#if DEBUG
            return $"{MongoConnectStringPrefix}{LocalDbAddress}";
#else
            return $"{MongoConnectStringPrefix}{Environment.GetEnvironmentVariable(MongoDbAddressEnvVarName)}";
#endif
        }

        /// <summary>
        /// Try to get user's data for user with the same credentials.
        /// </summary>
        /// <param name="userCredentials">Credentials from the log in attempt (login and password).</param>
        /// <param name="userData">User data, that belongs to the user data record with the same credentials. (will be null when nothing will be found).</param>
        /// <returns>Return true for successful search and false when nothing was find.</returns>
        public bool TryLogInUser(UserCredentials userCredentials, out UserData userData)
        {
            var usersTable = _database.GetCollection<UserData>(UsersCollectionName);

            var foundedUsers = usersTable.Find(userDataRecord => userDataRecord.Credentials == userCredentials);
            
            using var foundedUser = foundedUsers.ToCursor();

            foundedUser.MoveNext();
            
            userData = foundedUser.Current.FirstOrDefault();

            return foundedUsers.CountDocuments() != 0;
        }

#if DEBUG
        private const string LocalDbAddress = "localhost:27017";

        /// <summary>
        /// Returns count of tables (documents) in the connected DB. FOR TESTS PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public int GetCountOfTablesInDb()
        {
            using var allDatabasesStream = _database.ListCollections();

            allDatabasesStream.MoveNext();
            
            return allDatabasesStream.Current.Count();
        }
#endif
    }
}