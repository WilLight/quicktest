using System.Collections.Generic;

namespace server.DBSystem.ClassroomDataContainers
{
    /// <summary>
    /// Class, that contains data of the classroom.
    /// </summary>
    public sealed class ClassroomData
    {
        private readonly uint _roomId;
        private readonly uint _teacherId;
        private readonly List<uint> _studentsIds;

        public ClassroomData(uint roomId, uint teacherId, List<uint> studentsIds)
        {
            _roomId = roomId;
            _teacherId = teacherId;
            _studentsIds = studentsIds;
        }
    }
}