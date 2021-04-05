using System.Collections.Generic;
using server.DBSystem.UserDataContainers;
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

        [Fact]
        public void GetLastUserIdFromTable()
        {
            const uint expectedId = 1;

            Assert.Equal(expectedId, _dbManagerContainer.DbManager.GetLastUserId());
        }

        [Fact]
        public void Create3NewUsersAnd1OldAndCheck()
        {
            var testUserCredentials = new[]
            {
                new UserCredentials("admin", "admin"),
                new UserCredentials("null", "null"),
                new UserCredentials("wrong", "wrong"),
                new UserCredentials("12345", "12345")
            };

            var expectedResults = new[]
            {
                false,
                true,
                true,
                true
            };

            var results = new List<bool>(expectedResults.Length);
            var idToRemoveBuffer = new List<uint>(expectedResults.Length);

            foreach (var userCredential in testUserCredentials)
            {
                var registerResult = _dbManagerContainer.DbManager.TryRegisterUser(userCredential, out var userData);
                
                results.Add(registerResult);

                if (userData != null)
                {
                    idToRemoveBuffer.Add(userData.Id);
                }
            }

            foreach (var idToRemove in idToRemoveBuffer)
            {
                _dbManagerContainer.DbManager.RemoveUser(idToRemove);
            }

            Assert.Equal(expectedResults, results);
        }
    }
}