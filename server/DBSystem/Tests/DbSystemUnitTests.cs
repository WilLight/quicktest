using System.Collections.Generic;
using server.DBSystem.ClassroomDataContainers;
using server.DBSystem.UserDataContainers;
using Xunit;

namespace server.DBSystem.Tests
{
#if DEBUG
    /// <summary>
    /// Class, that manages unit tests, which are related to the DB.
    /// </summary>
    public sealed class DbSystemUnitTests : IClassFixture<DbTestsFixture>
    {
        private readonly DbTestsFixture _dbManagerContainer;

        public DbSystemUnitTests (DbTestsFixture dbManagerContainer)
        {
            _dbManagerContainer = dbManagerContainer;
        }

        [Fact]
        public void GetCountOfTablesFromDb ()
        {
            const int expectedCountOfTables = 2;

            Assert.Equal(expectedCountOfTables, _dbManagerContainer.DbManager.GetCountOfTablesInDb());
        }

        [Fact]
        public void TryLogIn5Accounts2Success3Fails ()
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
                false,
                false,
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
        public void GetLastUserIdFromTable ()
        {
            const uint expectedId = 0;

            Assert.Equal(expectedId, _dbManagerContainer.DbManager.GetLastUserId());
        }

        [Fact]
        public void Create3NewUsersAnd1OldAndCheck ()
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
                true,
                true,
                true,
                true
            };

            var results = new List<bool>(expectedResults.Length);
            var idToRemoveBuffer = new List<uint>(expectedResults.Length);

            foreach (var userCredential in testUserCredentials)
            {
                var registerResult =
                    _dbManagerContainer.DbManager.TryRegisterUser(UserRole.Student, userCredential, out var userData);

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

        [Fact]
        public void GetLastClassroomIdFromTable ()
        {
            const uint expectedId = 0;

            Assert.Equal(expectedId, _dbManagerContainer.DbManager.GetLastClassroomId());
        }

        [Fact]
        public void CreateClassroom ()
        {
            ClassroomData classroomData = CreateNewClassroom();
            Assert.NotNull(classroomData);
            _dbManagerContainer.DbManager.RemoveClassroom(classroomData.RoomId);
        }

        private ClassroomData CreateNewClassroom ()
        {
            DbManager dbManagerInstance = _dbManagerContainer.DbManager;
            UserCredentials fakeUserCredentials = new UserCredentials(Faker.Name.First(), Faker.Name.Last());
            dbManagerInstance.TryRegisterUser(UserRole.Teacher, fakeUserCredentials, out UserData newUserData);
            dbManagerInstance.TryCreateClassroom(newUserData.Id, null, out ClassroomData newClassroomData);

            return newClassroomData;
        }
        
        [Fact]
        public void AddUserToClassroomById ()
        {
            ClassroomData classroomData = CreateNewClassroom();
            UserCredentials fakeUserCredentials = new UserCredentials(Faker.Name.First(), Faker.Name.Last());
            _dbManagerContainer.DbManager.TryRegisterUser(UserRole.Student, fakeUserCredentials, out var userData);
            _dbManagerContainer.DbManager.TryAddUserToClassroomById(classroomData.RoomId, userData.Id);
            Assert.True(_dbManagerContainer.DbManager.TryAddUserToClassroomById(classroomData.RoomId, userData.Id));
            
        }
    }
#endif
}