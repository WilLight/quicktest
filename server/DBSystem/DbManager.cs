using System;
using System.Linq;
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

        private MongoClient _dbClient;

        /// <summary>
        /// Initialize manager's members and connect to the DB.
        /// </summary>
        public void Initialize()
        {
            _dbClient = new MongoClient(GetConnectionString());
        }

        private string GetConnectionString()
        {
#if DEBUG
            return $"{MongoConnectStringPrefix}{LocalDbAddress}";
#else
            return $"{MongoConnectStringPrefix}{Environment.GetEnvironmentVariable(MongoDbAddressEnvVarName)}";
#endif
        }

#if DEBUG
        private const string LocalDbAddress = "localhost:27017";

        /// <summary>
        /// Returns count of tables (documents) in the connected DB. FOR TESTS PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public int GetCountOfTablesInDb()
        {
            using var allDatabasesStream = _dbClient.GetDatabase(QuickTestDbName).ListCollections();

            allDatabasesStream.MoveNext();
            
            return allDatabasesStream.Current.Count();
        }
#endif
    }
}