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
        private readonly uint       roomId;
        private readonly uint       _teacherId;
        private readonly List<uint> _studentsIds;
        private readonly string     inviteToken;

        public uint RoomId => roomId;

        public uint TeacherId => _teacherId;

        public List<uint> StudentsIds => _studentsIds;

        public string InviteToken
        {
            get
            {
                return inviteToken;
            }
        }

        public ClassroomData (uint roomId, uint teacherId, List<uint> studentsIds, string inviteToken)
        {
            this.roomId = roomId;
            _teacherId = teacherId;
            _studentsIds = studentsIds ?? new List<uint>();
            this.inviteToken = inviteToken;
        }

        public ClassroomData (ClassroomData anotherClassroomData)
        {
            roomId = anotherClassroomData.RoomId;
            _teacherId = anotherClassroomData.TeacherId;
            _studentsIds = anotherClassroomData.StudentsIds ?? new List<uint>();
            inviteToken = anotherClassroomData.InviteToken;
        }
    }
}