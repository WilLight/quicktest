using Xunit;

namespace server.DBSystem.Tests
{
    /// <summary>
    /// Class, that manages unit tests, which are related to the DB.
    /// </summary>
    public sealed class DbSystemUnitTests
    {
        [Fact]
        public void GetCountOfTablesFromDb()
        {
            const int expectedCountOfTables = 1;

            var testDbManager = new DbManager();
            
            testDbManager.Initialize();
            
            Assert.Equal(expectedCountOfTables, testDbManager.GetCountOfTablesInDb());
        }
    }
}