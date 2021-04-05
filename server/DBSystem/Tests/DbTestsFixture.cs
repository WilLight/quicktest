using System;

namespace server.DBSystem.Tests
{
    /// <summary>
    /// Fixture for DB testing class.
    /// </summary>
    public sealed class DbTestsFixture : IDisposable
    {
        private readonly DbManager _dbManager;

        public DbManager DbManager => _dbManager;

        public DbTestsFixture()
        {
            _dbManager = new DbManager();

            _dbManager.Initialize();
        }

        public void Dispose()
        {
            
        }
    }
}