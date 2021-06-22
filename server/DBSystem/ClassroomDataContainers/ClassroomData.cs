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
        private readonly uint _roomId;
        private readonly uint _teacherId;
        private readonly List<uint> _studentsIds;
        private readonly string _inviteToken;

        public uint RoomId => _roomId;

        public uint TeacherId => _teacherId;

        public List<uint> StudentsIds => _studentsIds;

        public string InviteToken
        {
            get
            {
                return _inviteToken;
            }
        }

        public ClassroomData(uint roomId, uint teacherId, List<uint> studentsIds, string inviteToken)
        {
            _roomId = roomId;
            _teacherId = teacherId;
            _studentsIds = studentsIds ?? new List<uint>();
            _inviteToken = inviteToken;
        }

        public ClassroomData(ClassroomData anotherClassroomData)
        {
            _roomId = anotherClassroomData.RoomId;
            _teacherId = anotherClassroomData.TeacherId;
            _studentsIds = anotherClassroomData.StudentsIds ?? new List<uint>();
            _inviteToken = anotherClassroomData.InviteToken;
        }
    }
}