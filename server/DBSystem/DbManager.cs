﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using server.DBSystem.ClassroomDataContainers;
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
        private const string QuickTestDbName          = "quicktest";
        private const string UsersCollectionName      = "users";
        private const string ClassroomsCollectionName = "classrooms";

        private IMongoDatabase                  _database;
        private IMongoCollection<UserData>      _usersCollection;
        private IMongoCollection<ClassroomData> _classroomsCollection;
        private uint                            _lastUserId;
        private uint                            _lastClassroomId;

        /// <summary>
        /// Initialize manager's members and connect to the DB.
        /// </summary>
        public void Initialize ()
        {
            _database = new MongoClient(GetConnectionString()).GetDatabase(QuickTestDbName);
            _usersCollection = _database.GetCollection<UserData>(UsersCollectionName);
            _classroomsCollection = _database.GetCollection<ClassroomData>(ClassroomsCollectionName);

            _lastUserId = GetBiggestUserId();
            _lastClassroomId = GetBiggestClassroomId();
        }

        private string GetConnectionString ()
        {
        #if DEBUG
            return $"{MongoConnectStringPrefix}{LocalDbAddress}";
        #else
            return $"{MongoConnectStringPrefix}{Environment.GetEnvironmentVariable(MongoDbAddressEnvVarName)}";
        #endif
        }

        private uint GetBiggestUserId ()
        {
            var allUsers = _usersCollection.Find(new BsonDocument());

            if (allUsers.CountDocuments() == 0)
            {
                return 0;
            }

            return allUsers.SortByDescending(userDataRecord => userDataRecord.Id).First().Id;
        }

        private uint GetBiggestClassroomId ()
        {
            var allClassrooms = _classroomsCollection.Find(new BsonDocument());

            if (allClassrooms.CountDocuments() == 0)
            {
                return 0;
            }

            return allClassrooms.SortByDescending(classroomDataRecord => classroomDataRecord.RoomId).First().RoomId;
        }

        /// <summary>
        /// Try to get user's data for user with the same credentials.
        /// </summary>
        /// <param name="userCredentials">Credentials from the log in attempt (login and password).</param>
        /// <param name="userData">User data, that belongs to the user data record with the same credentials. (will be null when nothing will be found).</param>
        /// <returns>Returns true for successful search and false when nothing was find.</returns>
        public bool TryLogInUser (UserCredentials userCredentials, out UserData userData)
        {
            var foundedUsers = _usersCollection.Find(userDataRecord => userDataRecord.Credentials == userCredentials);

            return ProceedFoundedInfo(out userData, foundedUsers);
        }

        /// <summary>
        /// Try to register new user with provided credentials.
        /// </summary>
        /// <param name="role">Role of the new user.</param>
        /// <param name="userCredentials">Provided user credentials (login & password).</param>
        /// <param name="userData">Data of the new user (can be null when validation was failed).</param>
        /// <returns>Returns true when a new user was successfully registered and false when registration was failed because of data validation.</returns>
        public bool TryRegisterUser (UserRole role, UserCredentials userCredentials, out UserData userData)
        {
            if (TryLogInUser(userCredentials, out _))
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
        public void RemoveUser (uint userId)
        {
            _usersCollection.DeleteOne(new BsonDocument
                {
                    {"_id", userId}
                }
            );
        }

        public bool TryCreateClassroom (uint userId, List<uint> students, out ClassroomData classroomData)
        {
            if (!TryGetUserData(userId, out var userData) || userData.UserRole != UserRole.Teacher)
            {
                classroomData = null;

                return false;
            }

            uint newRoomId = ++_lastClassroomId;

            classroomData = new ClassroomData(newRoomId, userId, students, $"{newRoomId.ToString()}{userId.ToString()}");

            _classroomsCollection.InsertOne(classroomData);

            return true;
        }

        public bool TryGetUserData (uint userId, out UserData userData)
        {
            var foundedUsers = _usersCollection.Find(userDataRecord => userDataRecord.Id == userId);

            return ProceedFoundedInfo(out userData, foundedUsers);
        }

        private bool ProceedFoundedInfo<T> (out T userData, IFindFluent<T, T> foundedUsers)
        {
            using var foundedUserStream = foundedUsers.ToCursor();

            foundedUserStream.MoveNext();

            userData = foundedUserStream.Current.FirstOrDefault();

            return foundedUsers.CountDocuments() != 0;
        }

        public void RemoveClassroom (uint classroomId)
        {
            var foundedClassrooms = _classroomsCollection.Find(classroomDataRecord => classroomDataRecord.RoomId == classroomId);

            if (ProceedFoundedInfo(out ClassroomData classroomData, foundedClassrooms) == true)
            {
                _classroomsCollection.DeleteOne(classroomData.ToBsonDocument());
            }
        }

        public bool TryAddUserToClassroomById (uint classroomId, uint userId)
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
            
            _classroomsCollection.ReplaceOne(classroomData.ToBsonDocument(), newClassroomData);

            return true;
        }

        public bool TryGetClassroomData (uint classroomId, out ClassroomData classroomData)
        {
            var foundedClassrooms = _classroomsCollection.Find(userDataRecord => userDataRecord.RoomId == classroomId);

            return ProceedFoundedInfo(out classroomData, foundedClassrooms);
        }

    #if DEBUG
        private const string LocalDbAddress = "localhost:27017";

        /// <summary>
        /// Returns count of tables (documents) in the connected DB. FOR TESTS PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public int GetCountOfTablesInDb ()
        {
            using var allDatabasesStream = _database.ListCollections();

            allDatabasesStream.MoveNext();

            return allDatabasesStream.Current.Count();
        }

        /// <summary>
        /// Returns last user id from the users collection. FOR TEST PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public uint GetLastUserId ()
        {
            return _lastUserId;
        }

        /// <summary>
        /// Returns last classroom id from the classrooms collection. FOR TEST PURPOSES ONLY!
        /// </summary>
        /// <returns></returns>
        public uint GetLastClassroomId ()
        {
            return _lastClassroomId;
        }
    #endif
    }
}