using System.Collections.Generic;
using Xunit;

namespace server.DBSystem.Tests
{
    /// <summary>
    /// Class, that manages unit tests, which are related to the DB.
    /// </summary>
    public sealed class DbSystemUnitTests : IClassFixture<DbTestsFixture>
    {
        private readonly DbTestsFixture _dbManagerContainer;

        public DbSystemUnitTests(DbTestsFixture dbManagerContainer)
        {
            _dbManagerContainer = dbManagerContainer;
        }

        [Fact]
        public void GetCountOfTablesFromDb()
        {
            const int expectedCountOfTables = 1;

            Assert.Equal(expectedCountOfTables, _dbManagerContainer.DbManager.GetCountOfTablesInDb());
        }

        [Fact]
        public void TryLogIn5Accounts2Success3Fails()
        {
            var testUserCredentials = new[]
            {
                new UserCredentials("admin", "admin"),
                new UserCredentials("artem", "12345"),
                new UserCredentials("null", "null"),
                new UserCredentials("wrong", "wrong"),
                new UserCredentials("12345", "12345")
            };

            var expectedResults = new[]
            {
                true,
                true,
                false,
                false,
                false
            };

            var results = new List<bool>(expectedResults.Length);

            foreach (var userLoginData in testUserCredentials)
            {
                results.Add(_dbManagerContainer.DbManager.TryLogInUser(userLoginData, out _));
            }
            
            Assert.Equal(expectedResults, results);
        }
    }
}