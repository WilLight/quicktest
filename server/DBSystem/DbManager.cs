using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using server.DBSystem.UserDataContainers;

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
        private IMongoCollection<UserData> _usersCollection;
        private uint _lastUserId;

        /// <summary>
        /// Initialize manager's members and connect to the DB.
        /// </summary>
        public void Initialize()
        {
            _database = new MongoClient(GetConnectionString()).GetDatabase(QuickTestDbName);
            _usersCollection = _database.GetCollection<UserData>(UsersCollectionName);

            _lastUserId = GetBiggestUserId();
        }

        private string GetConnectionString()
        {
#if DEBUG
            return $"{MongoConnectStringPrefix}{LocalDbAddress}";
#else
            return $"{MongoConnectStringPrefix}{Environment.GetEnvironmentVariable(MongoDbAddressEnvVarName)}";
#endif
        }

        private uint GetBiggestUserId()
        {
            var allUsers = _usersCollection.Find(new BsonDocument());

            return allUsers.SortByDescending(userDataRecord => userDataRecord.Id).First().Id;
        }

        /// <summary>
        /// Try to get user's data for user with the same credentials.
        /// </summary>
        /// <param name="userCredentials">Credentials from the log in attempt (login and password).</param>
        /// <param name="userData">User data, that belongs to the user data record with the same credentials. (will be null when nothing will be found).</param>
        /// <returns>Returns true for successful search and false when nothing was find.</returns>
        public bool TryLogInUser(UserCredentials userCredentials, out UserData userData)
        {
            var foundedUsers = _usersCollection.Find(userDataRecord => userDataRecord.Credentials == userCredentials);
            
            using var foundedUserStream = foundedUsers.ToCursor();

            foundedUserStream.MoveNext();
            
            userData = foundedUserStream.Current.FirstOrDefault();

            return foundedUsers.CountDocuments() != 0;
        }

        /// <summary>
        /// Try to register new user with provided credentials.
        /// </summary>
        /// <param name="userCredentials">Provided user credentials (login & password).</param>
        /// <param name="userData">Data of the new user (can be null when validation was failed).</param>
        /// <returns>Returns true when a new user was successfully registered and false when registration was failed because of data validation.</returns>
        public bool TryRegisterUser(UserCredentials userCredentials, out UserData userData)
        {
            if (TryLogInUser(userCredentials, out _))
            {
                userData = null;
                
                return false;
            }
            
            userData = new UserData(++_lastUserId, userCredentials);
            
            _usersCollection.InsertOne(userData);

            return true;
        }

        /// <summary>
        /// Removes one user with the same id.
        /// </summary>
        /// <param name="userId">Id of the user, that need to be removed.</param>
        public void RemoveUser(uint userId)
        {
            _usersCollection.DeleteOne(new BsonDocument
                {
                    {"_id", userId}
                }
            );
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

        /// <summary>
        /// Returns last user id from the users collection. FOR TEST PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public uint GetLastUserId()
        {
            return _lastUserId;
        }
#endif
    }
}