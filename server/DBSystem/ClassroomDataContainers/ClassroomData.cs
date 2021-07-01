using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace server.DBSystem.ClassroomDataContainers
{
    /// <summary>
    /// Class, that contains data of the classroom.
    /// </summary>
    [BsonIgnoreExtraElements]
    public sealed class ClassroomData
    {
        public uint RoomId { get; set; }

        public uint TeacherId { get; set; }

        public string RoomName { get; set; }

        public List<uint> StudentsIds { get; set; }

        public List<uint> QuizIds { get; set; }

        public string InviteToken { get; }

        public ClassroomData(uint roomId, uint teacherId, string roomName, List<uint> studentsIds, List<uint> quizIds, string inviteToken)
        {
            RoomId = roomId;
            TeacherId = teacherId;
            RoomName = roomName;
            StudentsIds = studentsIds ?? new List<uint>();
            QuizIds = quizIds;
            InviteToken = inviteToken;
        }

        public ClassroomData(ClassroomData anotherClassroomData)
        {
            RoomId = anotherClassroomData.RoomId;
            TeacherId = anotherClassroomData.TeacherId;
            RoomName = anotherClassroomData.RoomName;
            StudentsIds = anotherClassroomData.StudentsIds ?? new List<uint>();
            QuizIds = anotherClassroomData.QuizIds;
            InviteToken = anotherClassroomData.InviteToken;
        }
    }
}