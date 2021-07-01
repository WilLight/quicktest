using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using server.DBSystem;
using server.DBSystem.UserDataContainers;
using server.DBSystem.QuizDataContainers;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class QuizController : ControllerBase
    {
        private readonly ILogger<QuizController> _logger;
        private readonly DbManager _dbManager;

        public QuizController(ILogger<QuizController> logger, DbManager dbManager)
        {
            _logger = logger;
            _dbManager = dbManager;
            _dbManager.Initialize();
        }

        [HttpGet]
        public ActionResult<QuizData> GetQuiz(uint quizId)
        {
            if (_dbManager.TryGetQuizData(quizId, out var quizData))
            {
                return Ok(quizData);
            }
            return NotFound();
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizData>> GetUserQuizes(uint userId)
        {

            if (!_dbManager.TryGetUserData(userId, out var userData))
            {
                return BadRequest("There is no such userId");
            }
            if (userData.UserRole == UserRole.Teacher)
            {
                _dbManager.TryGetClassroomsByTeacher(userId, out var classrooms);
                List<QuizData> quizzes = new List<QuizData>();
                foreach (var classroom in classrooms)
                {
                    if (_dbManager.TryGetQuizDataByOwner(userId, out var quizDatas))
                    {
                        quizzes.AddRange(quizDatas);
                    }
                }
                if (quizzes.Count > 0)
                {
                    return Ok(quizzes);
                }
            }
            else
            {
                _dbManager.TryGetClassroomsByStudent(userId, out var classrooms);
                List<QuizData> quizzes = new List<QuizData>();
                foreach (var classroom in classrooms)
                {
                    if (_dbManager.TryGetQuizDataByOwner(userId, out var quizDatas))
                    {
                        quizzes.AddRange(quizDatas);
                    }
                }
                if (quizzes.Count > 0)
                {
                    return Ok(quizzes);
                }
            }

            return BadRequest();
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizData>> GetClassQuizes(uint classroomId)
        {

            if (!_dbManager.TryGetClassroomData(classroomId, out var userData))
            {
                return BadRequest("There is no such classroomId");
            }

            if (_dbManager.TryGetQuizDataByClassroom(classroomId, out var quizDatas))
            {
                return Ok(quizDatas);
            }


            return BadRequest();
        }

        [HttpPost]
        public ActionResult<QuizData> AddQuiz([FromBody] JObject data)
        {
            var ownerId = data["ownerId"].ToObject<uint>();
            var quizName = data["quizName"].ToObject<string>();
            var quizQuestions = data["questions"].ToObject<IEnumerable<QuizQuestionData>>().ToList();
            if (!_dbManager.TryGetUserData(ownerId, out var userData) || userData.UserRole != UserRole.Teacher)
            {
                return BadRequest("There is no teacher with such userId");
            }
            if (_dbManager.TryCreateQuiz(ownerId, quizName, quizQuestions, out var quizData))
            {
                return Ok(quizData);
            }
            return BadRequest();
        }
    }
}