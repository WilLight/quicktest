using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using server.DBSystem.ClassroomDataContainers;
using server.DBSystem.QuizDataContainers;
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
        private const string ClassroomsCollectionName = "classrooms";
        private const string QuizesCollectionName = "quizes";
        private const string QuizAnswersCollectionName = "quizAnswers";

        private IMongoDatabase _database;
        private IMongoCollection<UserData> _usersCollection;
        private IMongoCollection<ClassroomData> _classroomsCollection;
        private IMongoCollection<QuizData> _quizesCollection;
        private IMongoCollection<QuizAnswerData> _quizAnswersCollection;
        private uint _lastUserId;
        private uint _lastClassroomId;
        private uint _lastQuizId;
        private uint _lastQuizAnswerId;

        /// <summary>
        /// Initialize manager's members and connect to the DB.
        /// </summary>
        public void Initialize()
        {
            _database = new MongoClient(GetConnectionString()).GetDatabase(QuickTestDbName);
            _usersCollection = _database.GetCollection<UserData>(UsersCollectionName);
            _classroomsCollection = _database.GetCollection<ClassroomData>(ClassroomsCollectionName);
            _quizesCollection = _database.GetCollection<QuizData>(QuizesCollectionName);
            _quizAnswersCollection = _database.GetCollection<QuizAnswerData>(QuizAnswersCollectionName);


            _lastUserId = GetBiggestUserId();
            _lastClassroomId = GetBiggestClassroomId();
            _lastQuizId = GetBiggestQuizId();
            _lastQuizAnswerId = GetBiggestQuizAnswerId();
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

            if (allUsers.CountDocuments() == 0)
            {
                return 0;
            }

            return allUsers.SortByDescending(userDataRecord => userDataRecord.Id).First().Id;
        }

        private uint GetBiggestClassroomId()
        {
            var allClassrooms = _classroomsCollection.Find(new BsonDocument());

            if (allClassrooms.CountDocuments() == 0)
            {
                return 0;
            }

            return allClassrooms.SortByDescending(classroomDataRecord => classroomDataRecord.RoomId).First().RoomId;
        }

        private uint GetBiggestQuizId()
        {
            var allQuizes = _quizesCollection.Find(new BsonDocument());

            if (allQuizes.CountDocuments() == 0)
            {
                return 0;
            }

            return allQuizes.SortByDescending(quizDataRecord => quizDataRecord.QuizId).First().QuizId;
        }

        private uint GetBiggestQuizAnswerId()
        {
            var allQuizAnswers = _quizAnswersCollection.Find(new BsonDocument());

            if (allQuizAnswers.CountDocuments() == 0)
            {
                return 0;
            }

            return allQuizAnswers.SortByDescending(quizAnswerDataRecord => quizAnswerDataRecord.QuizId).First().QuizId;
        }

        public bool TryGetClassroomsByTeacher(uint userId, out IEnumerable<ClassroomData> classrooms)
        {
            var foundClassrooms = _classroomsCollection.Find(classroom => classroom.TeacherId == userId);

            if (foundClassrooms.CountDocuments() != 0)
            {
                var classroomsCursor = foundClassrooms.ToCursor();
                classroomsCursor.MoveNext();
                classrooms = classroomsCursor.Current.AsEnumerable<ClassroomData>();
                return true;
            }
            classrooms = null;
            return false;
        }

        public bool TryGetClassroomsByStudent(uint userId, out IEnumerable<ClassroomData> classrooms)
        {
            var foundClassrooms = _classroomsCollection.Find(classroom => classroom.StudentsIds.Contains(userId));

            if (foundClassrooms.CountDocuments() != 0)
            {
                var classroomsCursor = foundClassrooms.ToCursor();
                classroomsCursor.MoveNext();
                classrooms = classroomsCursor.Current.AsEnumerable<ClassroomData>();
                return true;
            }
            classrooms = null;
            return false;
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

            return ProceedFoundedInfo(out userData, foundedUsers);
        }

        public bool TryGetUserByName(string login, out UserData userData)
        {
            var foundUser = _usersCollection.Find(userDataRecord => userDataRecord.Credentials.Login == login);
            if (ProceedFoundedInfo(out userData, foundUser))
            {
                return true;
            }
            userData = null;
            return false;
        }

        /// <summary>
        /// Try to register new user with provided credentials.
        /// </summary>
        /// <param name="role">Role of the new user.</param>
        /// <param name="userCredentials">Provided user credentials (login & password).</param>
        /// <param name="userData">Data of the new user (can be null when validation was failed).</param>
        /// <returns>Returns true when a new user was successfully registered and false when registration was failed because of data validation.</returns>
        public bool TryRegisterUser(UserRole role, UserCredentials userCredentials, out UserData userData)
        {
            if (TryLogInUser(userCredentials, out _))
            {
                userData = null;

                return false;
            }
            if (TryGetUserByName(userCredentials.Login, out userData))
            {
                userData = null;

                return false;
            }

            userData = new UserData(++_lastUserId, userCredentials, role);

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

        public bool TryCreateClassroom(uint userId, string roomName, List<uint> students, out ClassroomData classroomData)
        {
            if (!TryGetUserData(userId, out var userData) || userData.UserRole != UserRole.Teacher)
            {
                classroomData = null;

                return false;
            }

            uint newRoomId = ++_lastClassroomId;

            classroomData = new ClassroomData(newRoomId, userId, roomName, students, new List<uint>(), $"{newRoomId.ToString()}{userId.ToString()}");

            _classroomsCollection.InsertOne(classroomData);

            return true;
        }

        public bool TryGetUserData(uint userId, out UserData userData)
        {
            var foundedUsers = _usersCollection.Find(userDataRecord => userDataRecord.Id == userId);

            return ProceedFoundedInfo(out userData, foundedUsers);
        }

        private bool ProceedFoundedInfo<T>(out T userData, IFindFluent<T, T> foundedUsers)
        {
            using var foundedUserStream = foundedUsers.ToCursor();

            foundedUserStream.MoveNext();

            userData = foundedUserStream.Current.FirstOrDefault();

            return foundedUsers.CountDocuments() != 0;
        }

        public void RemoveClassroom(uint classroomId)
        {
            var foundedClassrooms = _classroomsCollection.Find(classroomDataRecord => classroomDataRecord.RoomId == classroomId);

            if (ProceedFoundedInfo(out ClassroomData classroomData, foundedClassrooms) == true)
            {
                _classroomsCollection.DeleteOne(classroom => classroom.RoomId == classroomId);
            }
        }

        public bool TryAddUserToClassroomById(uint classroomId, uint userId)
        {
            if (TryGetClassroomData(classroomId, out ClassroomData classroomData) == false)
            {
                return false;
            }

            if (TryGetUserData(userId, out _) == false)
            {
                return false;
            }

            ClassroomData newClassroomData = new ClassroomData(classroomData);
            newClassroomData.StudentsIds.Add(userId);

            _classroomsCollection.ReplaceOne(classroom => classroom.RoomId == classroomId, newClassroomData);

            return true;
        }

        public bool TryGetClassroomData(uint classroomId, out ClassroomData classroomData)
        {
            var foundedClassrooms = _classroomsCollection.Find(userDataRecord => userDataRecord.RoomId == classroomId);

            return ProceedFoundedInfo(out classroomData, foundedClassrooms);
        }

        public bool TryCreateQuiz(uint ownerId, string quizName, List<QuizQuestionData> questions, out QuizData quizData)
        {
            if (!TryGetUserData(ownerId, out var userData) && userData.UserRole != UserRole.Teacher)
            {
                quizData = null;

                return false;
            }

            uint newQuizId = ++_lastQuizId;

            quizData = new QuizData(newQuizId, ownerId, quizName, new List<uint>(), questions);

            _quizesCollection.InsertOne(quizData);

            return true;
        }

        public bool TryAddQuizToClassroom(uint quizId, uint classroomId)
        {
            if (!TryGetQuizData(quizId, out var quizData))
            {
                return false;
            }
            if (!TryGetClassroomData(classroomId, out var classroomData))
            {
                return false;
            }
            if (classroomData.QuizIds.Contains(quizId))
            {
                return false;
            }
            classroomData.QuizIds.Add(quizId);
            _classroomsCollection.ReplaceOne(classroom => classroom.RoomId == classroomId, classroomData);
            return true;
        }

        public bool TryGetQuizData(uint quizId, out QuizData quizData)
        {
            var foundQuizes = _quizesCollection.Find(quizDataRecord => quizDataRecord.QuizId == quizId);

            return ProceedFoundedInfo(out quizData, foundQuizes);
        }

        public bool TryGetQuizDataByUser(uint userId, out IEnumerable<QuizData> quizDatas)
        {
            if (TryGetUserData(userId, out var userData) && userData.UserRole == UserRole.Teacher)
            {
                var foundQuizes = _quizesCollection.Find(quizDatas => quizDatas.OwnerId == userId);
                if (foundQuizes.CountDocuments() != 0)
                {
                    var quizesCursor = foundQuizes.ToCursor();
                    quizesCursor.MoveNext();
                    quizDatas = quizesCursor.Current.AsEnumerable<QuizData>();
                    return true;
                }
            }
            quizDatas = null;
            return false;
        }

        public bool TryGetQuizDataByClassroom(uint roomId, out IEnumerable<QuizData> quizDatas)
        {
            var foundCloassrooms = _classroomsCollection.Find(classroomData => classroomData.RoomId == roomId);

            if (foundCloassrooms.CountDocuments() != 0)
            {
                var classroomsCursor = foundCloassrooms.ToCursor();
                classroomsCursor.MoveNext();
                var classroomDatas = classroomsCursor.Current.AsEnumerable<ClassroomData>();
                var quizIds = classroomDatas.First().QuizIds;
                var foundQuizes = _quizesCollection.Find(quizDatas => quizIds.Contains(quizDatas.QuizId));
                if (foundQuizes.CountDocuments() != 0)
                {
                    var quizesCursor = foundQuizes.ToCursor();
                    quizesCursor.MoveNext();
                    quizDatas = quizesCursor.Current.AsEnumerable<QuizData>();
                    return true;
                }
            }
            quizDatas = null;
            return false;
        }

        public bool TryCreateQuizAnswer(uint quizParentId, uint ownerId, IEnumerable<QuizQuestionData> answers, out QuizAnswerData quizAnswer)
        {
            if (!TryGetQuizData(quizParentId, out var quizData))
            {
                quizAnswer = null;
                return false;
            }
            else
            {
                uint newQuizAnswerId = ++_lastQuizAnswerId;

                quizAnswer = new QuizAnswerData(newQuizAnswerId, ownerId, answers.ToList());
                _quizAnswersCollection.InsertOne(quizAnswer);
                quizData.AnswerIds.Add(newQuizAnswerId);
                _quizesCollection.ReplaceOne(quiz => quiz.QuizId == quizData.QuizId, quizData);
                return true;
            }
        }

        public bool TryGetQuizAnswer(uint answerId, out QuizAnswerData quizAnswer)
        {
            var foundQuizAnswers = _quizAnswersCollection.Find(answerData => answerData.QuizId == answerId);
            if (foundQuizAnswers.CountDocuments() != 0)
            {
                var quizAnswerCursor = foundQuizAnswers.ToCursor();
                quizAnswerCursor.MoveNext();
                var quizAnswerData = quizAnswerCursor.Current.AsEnumerable<QuizAnswerData>();
                quizAnswer = quizAnswerData.First();
                return true;
            }
            quizAnswer = null;
            return false;
        }

        public bool TryGetQuizAnswersByQuiz(uint quizId, out IEnumerable<QuizAnswerData> quizAnswers)
        {
            if (!TryGetQuizData(quizId, out var quizData))
            {
                quizAnswers = null;
                return false;
            }
            else
            {
                var quizAnswerIds = quizData.AnswerIds;
                var quizAnswersList = new List<QuizAnswerData>();
                foreach (var item in quizAnswerIds)
                {
                    if (TryGetQuizAnswer(item, out var quizAnswer))
                    {
                        quizAnswersList.Add(quizAnswer);
                    }
                }
                if (quizAnswersList.Count != 0)
                {
                    quizAnswers = quizAnswersList;
                    return true;
                }
                else
                {
                    quizAnswers = null;
                    return false;
                }
            }
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

        /// <summary>
        /// Returns last classroom id from the classrooms collection. FOR TEST PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public uint GetLastClassroomId()
        {
            return _lastClassroomId;
        }
#endif
    }
}