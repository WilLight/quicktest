using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using server.DBSystem;
using server.DBSystem.UserDataContainers;
using server.DBSystem.ClassroomDataContainers;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClassroomController : ControllerBase
    {
        private readonly ILogger<UserAuthController> _logger;
        private readonly DbManager _dbManager;

        public ClassroomController(ILogger<UserAuthController> logger, DbManager dbManager)
        {
            _logger = logger;
            _dbManager = dbManager;
            _dbManager.Initialize();
        }

        //TODO: Will need to use cookies to authorise all of these
        [HttpGet]
        public ActionResult<IEnumerable<ClassroomData>> GetClassrooms(uint userId)
        {
            if (!_dbManager.TryGetUserData(userId, out var userData))
            {
                return BadRequest("There is no such userId");
            }
            if (userData.UserRole == UserRole.Teacher)
            {
                if (_dbManager.TryGetClassroomsByTeacher(userId, out var classrooms))
                {
                    return Ok(classrooms);
                }
            }
            else
            {
                if (_dbManager.TryGetClassroomsByStudent(userId, out var classrooms))
                {
                    return Ok(classrooms);
                }
            }

            return NotFound();
        }

        [HttpGet]
        public ActionResult<ClassroomData> GetDetails(uint classroomId)
        {
            ClassroomData classroom;
            if (_dbManager.TryGetClassroomData(classroomId, out classroom))
            {
                return Ok(classroom);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<ClassroomData> Add(uint userId)
        {
            ClassroomData createdClassroom;
            if (_dbManager.TryCreateClassroom(userId, new List<uint>(), out createdClassroom))
            {
                return Ok(createdClassroom);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<bool> AddUser([FromBody] JObject data)
        {
            uint classroomId = data["classroomId"].ToObject<uint>();
            uint userId = data["userId"].ToObject<uint>();
            if (_dbManager.TryAddUserToClassroomById(classroomId, userId))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
