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
        public ActionResult<IEnumerable<QuizData>> GetQuizes(uint userId)
        {
            if (!_dbManager.TryGetUserData(userId, out var userData))
            {
                return BadRequest("There is no such userId");
            }
            if (userData.UserRole == UserRole.Teacher)
            {
                if (_dbManager.TryGetQuizDataByOwner(userId, out var quizDatas))
                {
                    return Ok(quizDatas);
                }
            }
            else
            {
                if (_dbManager.TryGetQuizDataByStudent(userId, out var quizDatas))
                {
                    return Ok(quizDatas);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<QuizData> AddQuiz([FromBody] JObject data)
        {
            var ownerId = data["ownerId"].ToObject<uint>();
            var quizQuestions = data["questions"].ToObject<IEnumerable<QuizQuestionData>>().ToList();
            if (!_dbManager.TryGetUserData(ownerId, out var userData) || userData.UserRole != UserRole.Teacher)
            {
                return BadRequest("There is no teacher with such userId");
            }
            if (_dbManager.TryCreateQuiz(ownerId, quizQuestions, out var quizData))
            {
                return Ok(quizData);
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult<bool> AddStudentToQuiz([FromBody] JObject data)
        {
            var userId = data["userId"].ToObject<uint>();
            var quizId = data["quizId"].ToObject<uint>();

            if (!_dbManager.TryGetUserData(userId, out var userData) || userData.UserRole == UserRole.Teacher)
            {
                return BadRequest("There is no student with such userId");
            }
            if (!_dbManager.TryAddStudentToQuiz(quizId, userId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult<bool> RemoveStudentFromQuiz([FromBody] JObject data)
        {
            var userId = data["userId"].ToObject<uint>();
            var quizId = data["quizId"].ToObject<uint>();

            if (!_dbManager.TryGetUserData(userId, out var userData) || userData.UserRole == UserRole.Teacher)
            {
                return BadRequest("There is no student with such userId");
            }
            if (!_dbManager.TryRemoveUserFromQuiz(quizId, userId))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}